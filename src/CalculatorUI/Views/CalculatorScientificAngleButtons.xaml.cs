// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

//
// CalculatorScientificAngleButtons.xaml.h
// Declaration of the CalculatorScientificAngleButtons class
//

using CalculatorApp.ViewModel;
using Windows.UI.Xaml;

namespace CalculatorApp
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public sealed partial class CalculatorScientificAngleButtons
    {
        public CalculatorScientificAngleButtons()
        {
            m_isErrorVisualState = false;
            InitializeComponent();
        }

        public StandardCalculatorViewModel Model
        {
            get { return (StandardCalculatorViewModel)DataContext; }
        }

        public System.Windows.Input.ICommand ButtonPressed
        {
            get
            {
                if (donotuse_ButtonPressed == null)
                {
                    donotuse_ButtonPressed = new CalculatorApp.Common.DelegateCommand<CalculatorScientificAngleButtons>(this, OnAngleButtonPressed);
                }
                return donotuse_ButtonPressed;
            }
        }
        private System.Windows.Input.ICommand donotuse_ButtonPressed;

        public bool IsErrorVisualState { get; set; }

        private void OnAngleButtonPressed(object commandParameter)
        {
            string buttonId = (string)commandParameter;

            DegreeButton.Visibility = Visibility.Collapsed;
            RadianButton.Visibility = Visibility.Collapsed;
            GradsButton.Visibility = Visibility.Collapsed;

            if (buttonId == "0")
            {
                Model.SwitchAngleType(NumbersAndOperatorsEnum.Radians);
                RadianButton.Visibility = Visibility.Visible;
                RadianButton.Focus(FocusState.Programmatic);
            }
            else if (buttonId == "1")
            {
                Model.SwitchAngleType(NumbersAndOperatorsEnum.Grads);
                GradsButton.Visibility = Visibility.Visible;
                GradsButton.Focus(FocusState.Programmatic);
            }
            else if (buttonId == "2")
            {
                Model.SwitchAngleType(NumbersAndOperatorsEnum.Degree);
                DegreeButton.Visibility = Visibility.Visible;
                DegreeButton.Focus(FocusState.Programmatic);
            }
        }

        private void FToEButton_Toggled(object sender, RoutedEventArgs e)
        {
            var viewModel = (StandardCalculatorViewModel)DataContext;
            viewModel.FtoEButtonToggled();
        }

        private bool m_isErrorVisualState;
    }
}
