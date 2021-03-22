﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using CalculatorApp;
using CalculatorApp.Controls;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

using Windows.Foundation.Collections;

namespace CalculatorApp
{
    namespace Controls
    {
        public sealed class FlipButtons : Windows.UI.Xaml.Controls.Primitives.ToggleButton
        {
            public NumbersAndOperatorsEnum ButtonId
            {
                get { return (NumbersAndOperatorsEnum)GetValue(ButtonIdProperty); }
                set { SetValue(ButtonIdProperty, value); }
            }

            // Using a DependencyProperty as the backing store for ButtonId.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty ButtonIdProperty =
                DependencyProperty.Register("ButtonId", typeof(NumbersAndOperatorsEnum), typeof(FlipButtons), new PropertyMetadata(default(NumbersAndOperatorsEnum)));


            public Windows.UI.Xaml.Media.Brush HoverBackground
            {
                get { return (Windows.UI.Xaml.Media.Brush)GetValue(HoverBackgroundProperty); }
                set { SetValue(HoverBackgroundProperty, value); }
            }

            // Using a DependencyProperty as the backing store for HoverBackground.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty HoverBackgroundProperty =
                DependencyProperty.Register("HoverBackground", typeof(Windows.UI.Xaml.Media.Brush), typeof(FlipButtons), new PropertyMetadata(default(Windows.UI.Xaml.Media.Brush)));



            public Windows.UI.Xaml.Media.Brush HoverForeground
            {
                get { return (Windows.UI.Xaml.Media.Brush)GetValue(HoverForegroundProperty); }
                set { SetValue(HoverForegroundProperty, value); }
            }

            // Using a DependencyProperty as the backing store for HoverForeground.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty HoverForegroundProperty =
                DependencyProperty.Register("HoverForeground", typeof(Windows.UI.Xaml.Media.Brush), typeof(FlipButtons), new PropertyMetadata(default(Windows.UI.Xaml.Media.Brush)));



            public Windows.UI.Xaml.Media.Brush PressBackground
            {
                get { return (Windows.UI.Xaml.Media.Brush)GetValue(PressBackgroundProperty); }
                set { SetValue(PressBackgroundProperty, value); }
            }

            // Using a DependencyProperty as the backing store for PressBackground.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty PressBackgroundProperty =
                DependencyProperty.Register("PressBackground", typeof(Windows.UI.Xaml.Media.Brush), typeof(FlipButtons), new PropertyMetadata(default(Windows.UI.Xaml.Media.Brush)));



            public Windows.UI.Xaml.Media.Brush PressForeground
            {
                get { return (Windows.UI.Xaml.Media.Brush)GetValue(PressForegroundProperty); }
                set { SetValue(PressForegroundProperty, value); }
            }

            // Using a DependencyProperty as the backing store for PressForeground.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty PressForegroundProperty =
                DependencyProperty.Register("PressForeground", typeof(Windows.UI.Xaml.Media.Brush), typeof(FlipButtons), new PropertyMetadata(default(Windows.UI.Xaml.Media.Brush)));



            protected override void OnKeyDown(KeyRoutedEventArgs e)
            {
                // Ignore the Enter key
                if (e.Key == VirtualKey.Enter)
                {
                    return;
                }

                base.OnKeyDown(e);
            }

            protected override void OnKeyUp(KeyRoutedEventArgs e)
            {
                // Ignore the Enter key
                if (e.Key == VirtualKey.Enter)
                {
                    return;
                }

                base.OnKeyUp(e);
            }

            private void OnButtonIdPropertyChanged(NumbersAndOperatorsEnum oldValue, NumbersAndOperatorsEnum newValue)
            {

                this.CommandParameter = newValue;

            }

        }

    }
}