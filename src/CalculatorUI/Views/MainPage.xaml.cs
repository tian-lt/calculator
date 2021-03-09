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

using WUC = Windows.UI.Core;
using WUXI = Windows.UI.Xaml.Input;
using WUXD = Windows.UI.Xaml.Data;
using WUXC = Windows.UI.Xaml.Controls;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CalculatorApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            Stopwatch sw = new Stopwatch();
            Debug.WriteLine("  -- MainPage()");

            sw.Start();
            this.m_model = new ViewModel.ApplicationViewModel();
            sw.Stop();
            Debug.WriteLine($"  -- Model Loaded, duration: {sw.Elapsed}");

            this.InitializeComponent();
        }

        public CalculatorApp.ViewModel.ApplicationViewModel Model
        {
            get => this.m_model;
        }

        private CalculatorApp.ViewModel.ApplicationViewModel m_model;
    }
}
