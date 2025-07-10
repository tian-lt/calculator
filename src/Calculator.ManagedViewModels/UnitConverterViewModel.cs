// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel;

using CalculatorApp.Model;
using CalculatorApp.ViewModel.Common;

namespace CalculatorApp.ManagedViewModels
{
    public class UnitConverterViewModel2 :  Observable<UnitConverterViewModel2>, INotifyPropertyChanged
    {
        private List<UnitCategory> _catogries;

        public IList<UnitCategory> Categories => _catogries;

        public ViewMode Mode { get; set; }
    }
}
