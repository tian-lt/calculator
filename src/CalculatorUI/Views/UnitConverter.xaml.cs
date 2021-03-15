using System;
using CalculatorApp;
using CalculatorApp.Common;
using CalculatorApp.Controls;
using CalculatorApp.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CalculatorApp
{
    public sealed partial class UnitConverter : UserControl
    {
        public UnitConverter()
        {
            this.InitializeComponent();
        }

        public Windows.UI.Xaml.HorizontalAlignment FlowDirectionHorizontalAlignment
        {
            get => this.m_FlowDirectionHorizontalAlignment;
        }

        private Windows.UI.Xaml.HorizontalAlignment m_FlowDirectionHorizontalAlignment;

        public void AnimateConverter()
        {
            if(uiSettings.Value.AnimationsEnabled)
            {
                AnimationStory.Begin();
            }
        }

        public CalculatorApp.ViewModel.UnitConverterViewModel Model
        {
            get => (CalculatorApp.ViewModel.UnitConverterViewModel)this.DataContext;
        }

        public Windows.UI.Xaml.FlowDirection LayoutDirection
        {
            get => this.m_layoutDirection;
        }

        void SetDefaultFocus()
        {
            Control[] focusPrecedence = new Control[] { Value1, CurrencyRefreshBlockControl, OfflineBlock, ClearEntryButtonPos0 };

            foreach(Control control in focusPrecedence)
            {
                if(control.Focus(FocusState.Programmatic))
                {
                    break;
                }
            }
        }

        private void OnValueKeyDown(Object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Space)
            {
                OnValueSelected(sender);
            }

        }
        private void OnValueSelected(Object sender)
        {

            var value = ((CalculationResult)sender);
            // update the font size since the font is changed to bold
            value.UpdateTextState();
            ((UnitConverterViewModel)this.DataContext).OnValueActivated(AsActivatable(value));

        }

        private static Lazy<UISettings> uiSettings = new Lazy<UISettings>(true);
        private Windows.UI.Xaml.FlowDirection m_layoutDirection;
    }
}
