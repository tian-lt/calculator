// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using CalculatorApp.Model;
using CalculatorApp.ViewModel.Common;

namespace CalculatorApp.ManagedViewModels
{
    public class UnitCategoryViewModel
    {
    }

    public class UnitConverterViewModel : Observable<UnitConverterViewModel>, INotifyPropertyChanged
    {
        private UnitConverter<string, ViewMode> _converter = new UnitConverter<string, ViewMode>();
        private List<UnitCategoryViewModel> _catogries;
        private string _valueFrom = "0";
        private string _valueTo = "0";

        public IList<UnitCategoryViewModel> Categories => _catogries;

        public ViewMode Mode { get; set; }

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

        private void OnPaste(string text) { }
    }
}
