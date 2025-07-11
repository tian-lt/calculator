// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Windows.Globalization;
using Windows.UI.Xaml;

using CalculatorApp.Model;
using CalculatorApp.ViewModel.Common;
using CalculatorApp.ViewModel.Common.Automation;

namespace CalculatorApp.ManagedViewModels
{
    public class UnitCategoryViewModel
    {
        private readonly ViewMode _id;
        private readonly string _name;
        private readonly bool _supportsNegative;

        public UnitCategoryViewModel(ViewMode id, string name, bool supportsNegative)
        {
            _id = id;
            _name = name;
            _supportsNegative = supportsNegative;
        }

        public ViewMode Id => _id;
        public string Name => _name;
        public Visibility NegateVisibility => _supportsNegative ? Visibility.Visible : Visibility.Collapsed;
    }

    public class UnitViewModel
    {
        private readonly UnitKind _id;
        private readonly string _name;
        private readonly string _abbr;
        private readonly string _accessibleName;
        private readonly bool _isSource;
        private readonly bool _isTarget;
        private readonly bool _isWhimsical;

        public UnitViewModel(
             string categoryName, UnitKind id, string kindName, bool isSource = false, bool isTarget = false, bool isWhimsical = false)
        {
            var res = AppResourceProvider.GetInstance();
            var unitName = kindName.Substring(categoryName.Length);
            _id = id;
            _name = res.GetResourceString($"UnitName{unitName}");
            _abbr = res.GetResourceString($"UnitAbbreviation{unitName}");
            _accessibleName = _name;
            _isSource = isSource;
            _isTarget = isTarget;
            _isWhimsical = isWhimsical;
        }

        public UnitKind Id => _id;
        public string Name => _name;
        public string Abbreviation => _abbr;
        public string AccessibleName => _accessibleName;
    }

    public class SupplementaryResultViewModel
    {
    }

    public class UnitConverterViewModel : Observable<UnitConverterViewModel>, INotifyPropertyChanged
    {
        private readonly Dictionary<ViewMode, List<UnitViewModel>> _units = CreateUnits();
        private readonly UnitConverter<string, ViewMode> _converter = new UnitConverter<string, ViewMode>();
        private readonly List<UnitCategoryViewModel> _catogries = new List<UnitCategoryViewModel>();
        private UnitCategoryViewModel _currentCategory;
        private string _valueFrom = "0";
        private string _valueTo = "0";
        private string _value1;
        private string _value2;

        public IList<UnitCategoryViewModel> Categories => _catogries;

        public IList<SupplementaryResultViewModel> SupplementaryResults { get; set; }

        public Visibility SupplementaryVisibility => SupplementaryResults.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility CurrencySymbolVisibility =>
            (string.IsNullOrEmpty(CurrencySymbol1) || string.IsNullOrEmpty(CurrencySymbol2)) ?
            Visibility.Collapsed :
            Visibility.Visible;

        public ViewMode Mode { get; set; }

        public string Value1
        {
            get => _value1;
            set
            {
                if (_value1 != value)
                {
                    _value1 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Value2
        {
            get => _value2;
            set
            {
                if (_value2 != value)
                {
                    _value2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public UnitCategoryViewModel CurrentCategory
        {
            get => _currentCategory;
            set
            {
                if (_currentCategory != value)
                {
                    _currentCategory = value;
                    if (_currentCategory != null)
                    {
                        IsCurrencyCurrentCategory = _currentCategory.Id == ViewMode.Currency;
                    }
                    RaisePropertyChanged(nameof(Mode));
                }
            }
        }

        public string CurrencySymbol1 { get; set; }
        public string CurrencySymbol2 { get; set; }

        public bool Value1Active { get; set; }
        public bool Value2Active { get; set; }

        public string Value1AutomationName { get; set; }
        public string Value2AutomationName { get; set; }
        public string Unit1AutomationName { get; set; }
        public string Unit2AutomationName { get; set; }
        public NarratorAnnouncement Announcement { get; set; }
        public bool IsDecimalEnabled { get; set; }
        public bool IsDropDownOpen { get; set; }
        public bool IsDropDownEnabled { get; set; }
        public bool IsCurrencyLoadingVisible { get; set; }
        public bool IsCurrencyCurrentCategory { get; private set; }
        public string CurrencyRatioEquality { get; set; }
        public string CurrencyRatioEqualityAutomationName { get; set; }
        public string CurrencyTimestamp { get; set; }
        public NetworkAccessBehavior NetworkBehavior { get; set; }
        public bool CurrencyDataLoadFailed { get; set; }
        public bool CurrencyDataIsWeekOld { get; set; }


        public UnitConverterViewModel()
        {
            PropertyChanged += OnPropertyChanged;
            var navCategory = NavCategoryStates.CreateConverterCategoryGroup();
            foreach (var cat in navCategory.Categories)
            {
                _catogries.Add(new UnitCategoryViewModel(cat.ViewMode, cat.Name, cat.SupportsNegative));
            }

        }

        public void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
        }

        public async Task OnPasteCommand(object param)
        {
            if (!CopyPasteManager.HasStringToPaste())
            {
                return;
            }
            var text = await CopyPasteManager.GetStringToPaste(
                Mode,
                NavCategoryStates.GetGroupType(Mode),
                NumberBase.Unknown,
                BitLength.BitLengthUnknown);
            OnPaste(text);
        }

        public void OnCopyCommand(object param)
        {
            CopyPasteManager.CopyToClipboard(_valueFrom);
        }

        public void OnPaste(string text)
        {
            // If pastedString is invalid("NoOp") then display pasteError else process the string
            if (CopyPasteManager.IsErrorMessage(text))
            {
                DisplayError();
                return;
            }

            TraceLogger.GetInstance().LogInputPasted(Mode);
        }

        private void DisplayError()
        {
            const string SIDS_DOMAIN = "100"; //SIDS_DOMAIN is for "invalid input"
            var errMsg = AppResourceProvider.GetInstance().GetCEngineString(SIDS_DOMAIN);
            Value1 = errMsg;
            Value2 = errMsg;
        }

        private static Dictionary<ViewMode, List<UnitViewModel>> CreateUnits()
        {
            var units = new Dictionary<ViewMode, List<UnitViewModel>>();
            var regionCode = new GeographicRegion().CodeTwoLetter;
            bool useUSCustomaryAndFahrenheit = regionCode == "US" || regionCode == "FM" || regionCode == "MH" || regionCode == "PW";
            bool useUSCustomary = useUSCustomaryAndFahrenheit || regionCode == "LR";
            bool useSI = !useUSCustomary;

            units.Add(ViewMode.Area, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SquareMillimeter, nameof(UnitKind.Area_SquareMillimeter)),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SquareCentimeter, nameof(UnitKind.Area_SquareCentimeter)),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SquareMeter, nameof(UnitKind.Area_SquareMeter), useUSCustomary, useSI),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_Hectare, nameof(UnitKind.Area_Hectare)),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SquareKilometer, nameof(UnitKind.Area_SquareKilometer)),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SquareInch, nameof(UnitKind.Area_SquareInch)),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SquareFoot, nameof(UnitKind.Area_SquareFoot), useSI, useUSCustomary),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SquareYard, nameof(UnitKind.Area_SquareYard)),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_Acre, nameof(UnitKind.Area_Acre)),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SquareMile, nameof(UnitKind.Area_SquareMile)),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_Hand, nameof(UnitKind.Area_Hand), false, false, true),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_Paper, nameof(UnitKind.Area_Paper), false, false, true),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_SoccerField, nameof(UnitKind.Area_SoccerField), false, false, true),
                new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_Castle, nameof(UnitKind.Area_Castle), false, false, true),
            });
            if (regionCode == "JP" || regionCode == "TW" || regionCode == "KP" || regionCode == "KR")
            {
                units[ViewMode.Area].Add(new UnitViewModel(nameof(ViewMode.Area), UnitKind.Area_Pyeong, nameof(UnitKind.Area_Pyeong)));
            }

            units.Add(ViewMode.Data, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Bit, nameof(UnitKind.Data_Bit)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Nibble, nameof(UnitKind.Data_Nibble)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Byte, nameof(UnitKind.Data_Byte)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Kilobit, nameof(UnitKind.Data_Kilobit)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Kibibits, nameof(UnitKind.Data_Kibibits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Kilobyte, nameof(UnitKind.Data_Kilobyte)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Kibibytes, nameof(UnitKind.Data_Kibibytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Megabit, nameof(UnitKind.Data_Megabit)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Mebibits, nameof(UnitKind.Data_Mebibits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Megabyte, nameof(UnitKind.Data_Megabyte), false, true),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Mebibytes, nameof(UnitKind.Data_Mebibytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Gigabit, nameof(UnitKind.Data_Gigabit)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Gibibits, nameof(UnitKind.Data_Gibibits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Gigabyte, nameof(UnitKind.Data_Gigabyte), true),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Gibibytes, nameof(UnitKind.Data_Gibibytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Terabit, nameof(UnitKind.Data_Terabit)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Tebibits, nameof(UnitKind.Data_Tebibits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Terabyte, nameof(UnitKind.Data_Terabyte)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Tebibytes, nameof(UnitKind.Data_Tebibytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Petabit, nameof(UnitKind.Data_Petabit)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Pebibits, nameof(UnitKind.Data_Pebibits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Petabyte, nameof(UnitKind.Data_Petabyte)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Pebibytes, nameof(UnitKind.Data_Pebibytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Exabits, nameof(UnitKind.Data_Exabits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Exbibits, nameof(UnitKind.Data_Exbibits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Exabytes, nameof(UnitKind.Data_Exabytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Exbibytes, nameof(UnitKind.Data_Exbibytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Zetabits, nameof(UnitKind.Data_Zetabits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Zebibits, nameof(UnitKind.Data_Zebibits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Zetabytes, nameof(UnitKind.Data_Zetabytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Zebibytes, nameof(UnitKind.Data_Zebibytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Yottabit, nameof(UnitKind.Data_Yottabit)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Yobibits, nameof(UnitKind.Data_Yobibits)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Yottabyte, nameof(UnitKind.Data_Yottabyte)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_Yobibytes, nameof(UnitKind.Data_Yobibytes)),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_FloppyDisk, nameof(UnitKind.Data_FloppyDisk), false, false, true),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_CD, nameof(UnitKind.Data_CD), false, false, true),
                new UnitViewModel(nameof(ViewMode.Data), UnitKind.Data_DVD, nameof(UnitKind.Data_DVD), false, false, true),
            });
            units.Add(ViewMode.Energy, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_ElectronVolt, nameof(UnitKind.Energy_ElectronVolt)),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_Joule, nameof(UnitKind.Energy_Joule), true),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_Kilojoule, nameof(UnitKind.Energy_Kilojoule)),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_Calorie, nameof(UnitKind.Energy_Calorie)),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_Kilocalorie, nameof(UnitKind.Energy_Kilocalorie), false, true),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_FootPound, nameof(UnitKind.Energy_FootPound)),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_BritishThermalUnit, nameof(UnitKind.Energy_BritishThermalUnit)),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_Battery, nameof(UnitKind.Energy_Battery), false, false, true),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_Banana, nameof(UnitKind.Energy_Banana), false, false, true),
                new UnitViewModel(nameof(ViewMode.Energy), UnitKind.Energy_SliceOfCake, nameof(UnitKind.Energy_SliceOfCake), false, false, true),
            });

            return units;
        }
    }
}
