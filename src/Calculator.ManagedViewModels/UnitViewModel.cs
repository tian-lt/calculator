// Licensed under the MIT License.

using CalculatorApp.Model;
using CalculatorApp.ViewModel.Common;

namespace CalculatorApp.ManagedViewModels
{
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
        public bool IsSource => _isSource;
        public bool IsTarget => _isTarget;
        public string Abbreviation => _abbr;
        public string AccessibleName => _accessibleName;
        public bool IsWhimsical => _isWhimsical;
    }
}
