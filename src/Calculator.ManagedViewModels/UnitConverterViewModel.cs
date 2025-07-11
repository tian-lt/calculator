// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using CalculatorApp.Model;
using CalculatorApp.ViewModel.Common;
using CalculatorApp.ViewModel.Common.Automation;
using Windows.UI.Xaml;

namespace CalculatorApp.ManagedViewModels
{
    public class UnitCategoryViewModel
    {
        private readonly ViewMode _id;
        private readonly string _name;
        private bool _supportsNegative;

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

    }

    public class SupplementaryResultViewModel
    {
    }

    public class UnitConverterViewModel : Observable<UnitConverterViewModel>, INotifyPropertyChanged
    {
        private UnitConverter<string, ViewMode> _converter = new UnitConverter<string, ViewMode>();
        private List<UnitCategoryViewModel> _catogries = new List<UnitCategoryViewModel>();
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

        public UnitCategoryViewModel CurrentCategory { get; set; }

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
        public bool IsCurrencyCurrentCategory { get; }
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
    }
}
