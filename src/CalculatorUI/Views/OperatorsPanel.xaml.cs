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

using CalculatorApp;
using CalculatorApp.ViewModel;
using CalculatorApp.Common;
using CalculatorApp.Converters;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CalculatorApp
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public sealed partial class OperatorsPanel : UserControl
    {
        public CalculatorApp.ViewModel.StandardCalculatorViewModel Model
        {
            get => (CalculatorApp.ViewModel.StandardCalculatorViewModel)this.DataContext;
        }

        public OperatorsPanel()
        {
            this.InitializeComponent();
        }



        public bool IsBitFlipChecked
        {
            get { return (bool)GetValue(IsBitFlipCheckedProperty); }
            set { SetValue(IsBitFlipCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBitFlipChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBitFlipCheckedProperty =
            DependencyProperty.Register(nameof(IsBitFlipChecked), typeof(bool), typeof(OperatorsPanel), new PropertyMetadata(default(bool), (sender, args)=>
            {
                var self = (OperatorsPanel)sender;
                self.OnIsBitFlipCheckedPropertyChanged((bool)args.OldValue, (bool)args.NewValue);
            }));




        public bool IsErrorVisualState
        {
            get { return (bool)GetValue(IsErrorVisualStateProperty); }
            set { SetValue(IsErrorVisualStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsErrorVisualState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsErrorVisualStateProperty =
            DependencyProperty.Register(nameof(IsErrorVisualState), typeof(bool), typeof(OperatorsPanel), new PropertyMetadata(default(bool), (sender, args)=> 
            {
                var self = (OperatorsPanel)sender;
                self.OnIsErrorVisualStatePropertyChanged((bool)args.OldValue, (bool)args.NewValue);
            }));


        void OnIsBitFlipCheckedPropertyChanged(bool oldValue, bool newValue)
        {

            if (newValue)
            {
                EnsureProgrammerBitFlipPanel();
            }

        }

        private void OnIsErrorVisualStatePropertyChanged(bool oldValue, bool newValue)
        {

            //if (Model.IsStandard)
            //{
            //    StandardOperators.IsErrorVisualState = newValue;
            //}
            //else if (Model.IsScientific)
            //{
            //    ScientificOperators.IsErrorVisualState = newValue;
            //}
            //else if (Model.IsProgrammer)
            //{
            //    ProgrammerRadixOperators.IsErrorVisualState = newValue;
            //}

        }

        private void EnsureScientificOps()
        {

            //if (!ScientificOperators)
            //{
            //    this.FindName("ScientificOperators");
            //}

        }

        private void EnsureProgrammerRadixOps()
        {

            //if (!ProgrammerRadixOperators)
            //{
            //    this.FindName("ProgrammerRadixOperators");
            //}

            //ProgrammerRadixOperators.checkDefaultBitShift();

        }

        private void EnsureProgrammerBitFlipPanel()
        {

            //if (!BitFlipPanel)
            //{
            //    this.FindName("BitFlipPanel");
            //}

        }


    }
}
