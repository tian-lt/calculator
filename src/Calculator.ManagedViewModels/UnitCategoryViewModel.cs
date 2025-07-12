// Licensed under the MIT License.

using Windows.UI.Xaml;

using CalculatorApp.ViewModel.Common;

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
}
