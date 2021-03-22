﻿using System;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CalculatorApp
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public sealed partial class MemoryListItem : UserControl
    {
        public MemoryListItem()
        {
            this.InitializeComponent();
        }



        public CalculatorApp.ViewModel.MemoryItemViewModel Model
        {
            get { return (CalculatorApp.ViewModel.MemoryItemViewModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(nameof(Model), typeof(CalculatorApp.ViewModel.MemoryItemViewModel), typeof(MemoryListItem), new PropertyMetadata(default(CalculatorApp.ViewModel.MemoryItemViewModel)));

        protected override void OnPointerEntered(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            base.OnPointerEntered(e);

            // Only show hover buttons when the user is using mouse or pen.
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse
                || e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                VisualStateManager.GoToState(this, "MemoryButtonsVisible", true);
            }

        }

        protected override void OnPointerExited(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            base.OnPointerExited(e);

            VisualStateManager.GoToState(this, "MemoryButtonsHidden", true);

        }

        private void OnMemoryAddButtonClicked(Object sender, RoutedEventArgs e)
        {

            Model.MemoryAdd();

        }

        private void OnClearButtonClicked(Object sender, RoutedEventArgs e)
        {

            Model.Clear();

        }
        private void OnMemorySubtractButtonClicked(Object sender, RoutedEventArgs e)
        {

            Model.MemorySubtract();

        }

        private void OnClearSwipeInvoked(Microsoft.UI.Xaml.Controls.SwipeItem sender, Microsoft.UI.Xaml.Controls.SwipeItemInvokedEventArgs e)
        {

            Model.Clear();


        }

        private void OnMemoryAddSwipeInvoked(Microsoft.UI.Xaml.Controls.SwipeItem sender, Microsoft.UI.Xaml.Controls.SwipeItemInvokedEventArgs e)
        {

            Model.MemoryAdd();

        }

        private void OnMemorySubtractSwipeInvoked(Microsoft.UI.Xaml.Controls.SwipeItem sender, Microsoft.UI.Xaml.Controls.SwipeItemInvokedEventArgs e)
        {

            Model.MemorySubtract();

        }


    }
}
