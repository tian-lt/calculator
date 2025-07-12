// Licensed under the MIT License.

using CalculatorApp.ViewModel.Common;

namespace CalculatorApp.ManagedViewModels
{
    public class SupplementaryResultViewModel
    {
        private readonly UnitViewModel _unit;
        private readonly string _value;

        public SupplementaryResultViewModel(UnitViewModel unit, string value)
        {
            _unit = unit;
            _value = value;
        }

        public UnitViewModel Unit => _unit;
        public string Value => _value;
        public bool IsWhimsical => _unit.IsWhimsical;

        public string GetLocalizedAutomationName()
        {
            var fmt = AppResourceProvider.GetInstance().GetResourceString("SupplementaryUnit_AutomationName");
            return LocalizationStringUtil.GetLocalizedString(fmt, _value, _unit.Name);
        }
    }
}
