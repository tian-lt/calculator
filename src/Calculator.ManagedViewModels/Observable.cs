// Licensed under the MIT License.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalculatorApp.ManagedViewModels
{
    public class Observable<T> where T : Observable<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke((T)this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
