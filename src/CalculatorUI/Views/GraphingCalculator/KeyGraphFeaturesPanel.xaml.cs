// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using CalculatorApp;
using CalculatorApp.ViewModel;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;

namespace CalculatorApp
{
    public sealed partial class KeyGraphFeaturesPanel : System.ComponentModel.INotifyPropertyChanged
    {
        public KeyGraphFeaturesPanel()
        {
            InitializeComponent();
            this.Loaded += KeyGraphFeaturesPanel_Loaded;
        }

        // CSHARP_MIGRATION: TODO:
        // look for macros. removed virtual for sealed class
        //public OBSERVABLE_OBJECT();
        //public OBSERVABLE_PROPERTY_RW(CalculatorApp.ViewModel.EquationViewModel , ViewModel);
        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged(string p)
        {
#if !UNIT_TESTS
            PropertyChanged(this, new PropertyChangedEventArgs(p));
#endif
        }

        public CalculatorApp.ViewModel.EquationViewModel ViewModel
        {
            get { return m_viewModel; }
            set
            {
                if (m_viewModel != value)
                {
                    m_viewModel = value;
                    // CSHARP_MIGRATION: TODO:
                    // L means wchar_t. size difference between wchar_t and char
                    // RaisePropertyChanged(L#n);
                    RaisePropertyChanged("ViewModel");
                }
            }
        }

        public event Windows.UI.Xaml.RoutedEventHandler KeyGraphFeaturesClosed;

        public static Windows.UI.Xaml.Media.SolidColorBrush
                     ToSolidColorBrush(Windows.UI.Color color)
        {
            return new Windows.UI.Xaml.Media.SolidColorBrush(color);
        }

        private void KeyGraphFeaturesPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            BackButton.Focus(FocusState.Programmatic);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            KeyGraphFeaturesClosed(this, new RoutedEventArgs());
        }

        private CalculatorApp.ViewModel.EquationViewModel m_viewModel;
    }
}
