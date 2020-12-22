using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Threading;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CalculatorUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //TestCalcViewModel();
        }

        void TestCalcViewModel()
        {
            //CalculatorApp.ViewModel.ApplicationViewModel mdl0 = null;
            //mdl0 = new CalculatorApp.ViewModel.ApplicationViewModel();

            Thread thrd = new Thread(()=>
            {
                CalculatorApp.ViewModel.ApplicationViewModel mdl = null;
                mdl = new CalculatorApp.ViewModel.ApplicationViewModel();
            });
            thrd.Start();
            thrd.Join();
        }

    }
}
