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

            // US + Federated States of Micronesia, Marshall Islands, Palau
            bool useUSCustomaryAndFahrenheit = regionCode == "US" || regionCode == "FM" || regionCode == "MH" || regionCode == "PW";

            // useUSCustomaryAndFahrenheit + Liberia
            // Source: https://en.wikipedia.org/wiki/Metrication
            bool useUSCustomary = useUSCustomaryAndFahrenheit || regionCode == "LR";

            // Use 'Système International' (International System of Units - Metrics)
            bool useSI = !useUSCustomary;

            // useUSCustomaryAndFahrenheit + the Bahamas, the Cayman Islands and Liberia
            // Source: http://en.wikipedia.org/wiki/Fahrenheit
            bool useFahrenheit = useUSCustomaryAndFahrenheit || regionCode == "BS" || regionCode == "KY" || regionCode == "LR";

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
                // Use 坪(Tsubo), or Pyeong in Korean, a Japanese unit of floorspace.
                // https://en.wikipedia.org/wiki/Japanese_units_of_measurement#Area
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

            units.Add(ViewMode.Length, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Angstrom, nameof(UnitKind.Length_Angstrom)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Nanometer, nameof(UnitKind.Length_Nanometer)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Micron, nameof(UnitKind.Length_Micron)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Millimeter, nameof(UnitKind.Length_Millimeter)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Centimeter, nameof(UnitKind.Length_Centimeter), useUSCustomary, useSI),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Meter, nameof(UnitKind.Length_Meter)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Kilometer, nameof(UnitKind.Length_Kilometer)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Inch, nameof(UnitKind.Length_Inch), useSI, useUSCustomary),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Foot, nameof(UnitKind.Length_Foot)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Yard, nameof(UnitKind.Length_Yard)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Mile, nameof(UnitKind.Length_Mile)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_NauticalMile, nameof(UnitKind.Length_NauticalMile)),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Paperclip, nameof(UnitKind.Length_Paperclip), false, false, true),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_Hand, nameof(UnitKind.Length_Hand), false, false, true),
                new UnitViewModel(nameof(ViewMode.Length), UnitKind.Length_JumboJet, nameof(UnitKind.Length_JumboJet), false, false, true),
            });

            units.Add(ViewMode.Temperature, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Temperature), UnitKind.Temperature_DegreesCelsius, nameof(UnitKind.Temperature_DegreesCelsius), useFahrenheit, !useFahrenheit),
                new UnitViewModel(nameof(ViewMode.Temperature), UnitKind.Temperature_DegreesFahrenheit, nameof(UnitKind.Temperature_DegreesFahrenheit), !useFahrenheit, useFahrenheit),
                new UnitViewModel(nameof(ViewMode.Temperature), UnitKind.Temperature_Kelvin, nameof(UnitKind.Temperature_Kelvin)),
            });

            units.Add(ViewMode.Time, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Time), UnitKind.Time_Microsecond, nameof(UnitKind.Time_Microsecond)),
                new UnitViewModel(nameof(ViewMode.Time), UnitKind.Time_Millisecond, nameof(UnitKind.Time_Millisecond)),
                new UnitViewModel(nameof(ViewMode.Time), UnitKind.Time_Second, nameof(UnitKind.Time_Second)),
                new UnitViewModel(nameof(ViewMode.Time), UnitKind.Time_Minute, nameof(UnitKind.Time_Minute), false, true),
                new UnitViewModel(nameof(ViewMode.Time), UnitKind.Time_Hour, nameof(UnitKind.Time_Hour), true),
                new UnitViewModel(nameof(ViewMode.Time), UnitKind.Time_Day, nameof(UnitKind.Time_Day)),
                new UnitViewModel(nameof(ViewMode.Time), UnitKind.Time_Week, nameof(UnitKind.Time_Week)),
                new UnitViewModel(nameof(ViewMode.Time), UnitKind.Time_Year, nameof(UnitKind.Time_Year)),
            });

            units.Add(ViewMode.Speed, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_CentimetersPerSecond, nameof(UnitKind.Speed_CentimetersPerSecond)),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_MetersPerSecond, nameof(UnitKind.Speed_MetersPerSecond)),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_KilometersPerHour, nameof(UnitKind.Speed_KilometersPerHour), useUSCustomary, useSI),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_FeetPerSecond, nameof(UnitKind.Speed_FeetPerSecond)),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_MilesPerHour, nameof(UnitKind.Speed_MilesPerHour), useSI, useUSCustomary),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_Knot, nameof(UnitKind.Speed_Knot)),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_Mach, nameof(UnitKind.Speed_Mach)),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_Turtle, nameof(UnitKind.Speed_Turtle), false, false, true),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_Horse, nameof(UnitKind.Speed_Horse), false, false, true),
                new UnitViewModel(nameof(ViewMode.Speed), UnitKind.Speed_Jet, nameof(UnitKind.Speed_Jet), false, false, true),
            });

            units.Add(ViewMode.Volume, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_Milliliter, nameof(UnitKind.Volume_Milliliter), useUSCustomary, useSI),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_CubicCentimeter, nameof(UnitKind.Volume_CubicCentimeter)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_Liter, nameof(UnitKind.Volume_Liter)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_CubicMeter, nameof(UnitKind.Volume_CubicMeter)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_TeaspoonUS, nameof(UnitKind.Volume_TeaspoonUS), useSI, useUSCustomary && regionCode != "GB"),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_TablespoonUS, nameof(UnitKind.Volume_TablespoonUS)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_FluidOunceUS, nameof(UnitKind.Volume_FluidOunceUS)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_CupUS, nameof(UnitKind.Volume_CupUS)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_PintUS, nameof(UnitKind.Volume_PintUS)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_QuartUS, nameof(UnitKind.Volume_QuartUS)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_GallonUS, nameof(UnitKind.Volume_GallonUS)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_CubicInch, nameof(UnitKind.Volume_CubicInch)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_CubicFoot, nameof(UnitKind.Volume_CubicFoot)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_CubicYard, nameof(UnitKind.Volume_CubicYard)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_TeaspoonUK, nameof(UnitKind.Volume_TeaspoonUK), false, useUSCustomary && regionCode == "GB"),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_TablespoonUK, nameof(UnitKind.Volume_TablespoonUK)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_FluidOunceUK, nameof(UnitKind.Volume_FluidOunceUK)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_PintUK, nameof(UnitKind.Volume_PintUK)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_QuartUK, nameof(UnitKind.Volume_QuartUK)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_GallonUK, nameof(UnitKind.Volume_GallonUK)),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_CoffeeCup, nameof(UnitKind.Volume_CoffeeCup), false, false, true),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_Bathtub, nameof(UnitKind.Volume_Bathtub), false, false, true),
                new UnitViewModel(nameof(ViewMode.Volume), UnitKind.Volume_SwimmingPool, nameof(UnitKind.Volume_SwimmingPool), false, false, true),
            });

            units.Add(ViewMode.Weight, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Carat, nameof(UnitKind.Weight_Carat)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Milligram, nameof(UnitKind.Weight_Milligram)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Centigram, nameof(UnitKind.Weight_Centigram)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Decigram, nameof(UnitKind.Weight_Decigram)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Gram, nameof(UnitKind.Weight_Gram)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Decagram, nameof(UnitKind.Weight_Decagram)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Hectogram, nameof(UnitKind.Weight_Hectogram)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Kilogram, nameof(UnitKind.Weight_Kilogram), useUSCustomary, useSI),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Tonne, nameof(UnitKind.Weight_Tonne)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Ounce, nameof(UnitKind.Weight_Ounce)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Pound, nameof(UnitKind.Weight_Pound), useSI, useUSCustomary),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Stone, nameof(UnitKind.Weight_Stone)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_ShortTon, nameof(UnitKind.Weight_ShortTon)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_LongTon, nameof(UnitKind.Weight_LongTon)),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Snowflake, nameof(UnitKind.Weight_Snowflake), false, false, true),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_SoccerBall, nameof(UnitKind.Weight_SoccerBall), false, false, true),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Elephant, nameof(UnitKind.Weight_Elephant), false, false, true),
                new UnitViewModel(nameof(ViewMode.Weight), UnitKind.Weight_Whale, nameof(UnitKind.Weight_Whale), false, false, true),
            });

            units.Add(ViewMode.Pressure, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Pressure), UnitKind.Pressure_Atmosphere, nameof(UnitKind.Pressure_Atmosphere), true),
                new UnitViewModel(nameof(ViewMode.Pressure), UnitKind.Pressure_Bar, nameof(UnitKind.Pressure_Bar), false, true),
                new UnitViewModel(nameof(ViewMode.Pressure), UnitKind.Pressure_KiloPascal, nameof(UnitKind.Pressure_KiloPascal)),
                new UnitViewModel(nameof(ViewMode.Pressure), UnitKind.Pressure_MillimeterOfMercury, nameof(UnitKind.Pressure_MillimeterOfMercury)),
                new UnitViewModel(nameof(ViewMode.Pressure), UnitKind.Pressure_Pascal, nameof(UnitKind.Pressure_Pascal)),
                new UnitViewModel(nameof(ViewMode.Pressure), UnitKind.Pressure_PSI, nameof(UnitKind.Pressure_PSI)),
            });

            units.Add(ViewMode.Angle, new List<UnitViewModel> {
                new UnitViewModel(nameof(ViewMode.Angle), UnitKind.Angle_Degree, nameof(UnitKind.Angle_Degree), true),
                new UnitViewModel(nameof(ViewMode.Angle), UnitKind.Angle_Radian, nameof(UnitKind.Angle_Radian), false, true),
                new UnitViewModel(nameof(ViewMode.Angle), UnitKind.Angle_Gradian, nameof(UnitKind.Angle_Gradian)),
            });
            return units;
        }
    }
}
