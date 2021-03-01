using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    [Windows.Foundation.Metadata.WebHostHidden]
    public sealed partial class CalculatorProgrammerDisplayPanel : UserControl
    {
        public ICommand BitLengthButtonPressed
        {
            get
            {
                if(donotuse_BitLengthButtonPressed == null)
                {
                    donotuse_BitLengthButtonPressed =
                        new Common.DelegateCommand<CalculatorProgrammerDisplayPanel>(this, OnBitLengthButtonPressed);
                }
                return donotuse_BitLengthButtonPressed;
            }
        }

        private ICommand donotuse_BitLengthButtonPressed;

        public ViewModel.StandardCalculatorViewModel Model
        {
            get
            {
                Debug.Assert(this.DataContext as ViewModel.StandardCalculatorViewModel != null, "static_cast result must NOT be null");
                return this.DataContext as ViewModel.StandardCalculatorViewModel;
            }
        }

        public bool IsErrorVisualState
        {
            get
            {
                return m_isErrorVisualState;
            }

            set
            {
                if(m_isErrorVisualState != value)
                {
                    m_isErrorVisualState = value;
                    string newState = m_isErrorVisualState ? "ErrorLayout" : "NoErrorLayout";
                    VisualStateManager.GoToState(this, newState, false);
                }
            }
        }

        public CalculatorProgrammerDisplayPanel()
        {
            this.m_isErrorVisualState = false;
            this.InitializeComponent();
        }

        private void OnBitLengthButtonPressed(object parameter)
        {
            string buttonId = parameter.ToString();

            QwordButton.Visibility = Visibility.Collapsed;
            DwordButton.Visibility = Visibility.Collapsed;
            WordButton.Visibility = Visibility.Collapsed;
            ByteButton.Visibility = Visibility.Collapsed;
            if (buttonId == "0")
            {
                Model.ValueBitLength = Common.BitLength.BitLengthDWord;
                DwordButton.Visibility = Visibility.Visible;
                DwordButton.Focus(FocusState.Programmatic);
            }
            else if (buttonId == "1")
            {
                Model.ValueBitLength = Common.BitLength.BitLengthWord;
                WordButton.Visibility = Visibility.Visible;
                WordButton.Focus(FocusState.Programmatic);
            }
            else if (buttonId == "2")
            {
                Model.ValueBitLength = Common.BitLength.BitLengthByte;
                ByteButton.Visibility = Visibility.Visible;
                ByteButton.Focus(FocusState.Programmatic);
            }
            else if (buttonId == "3")
            {
                Model.ValueBitLength = Common.BitLength.BitLengthQWord;
                QwordButton.Visibility = Visibility.Visible;
                QwordButton.Focus(FocusState.Programmatic);
            }
        }

        private bool m_isErrorVisualState;
    }
}
