﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Xaml.Data;

namespace CalculatorApp
{
    namespace Common
    {
        public sealed class ValidSelectedItemConverter : Windows.UI.Xaml.Data.IValueConverter
        {
            public ValidSelectedItemConverter()
            { }

            public object Convert(object value, Type targetType, object parameter, string language)
            {
                // Pass through as we don't want to change the value from the source
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                if (value != null)
                {
                    return value;
                }

                // Stop the binding if the object is nullptr
                return Windows.UI.Xaml.DependencyProperty.UnsetValue;
            }
        }

        public sealed class ValidSelectedIndexConverter : Windows.UI.Xaml.Data.IValueConverter
        {

            public ValidSelectedIndexConverter()
            { }

            public object Convert(object value, Type targetType, object parameter, string language)
            {
                // Pass through as we don't want to change the value from the source
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                // The value to be valid has to be a boxed int32 value
                // extract that value and ensure it is valid, ie >= 0
                if (value != null)
                {
                    var box = value as Windows.Foundation.IPropertyValue;
                    if (box != null && box.Type == Windows.Foundation.PropertyType.Int32)
                    {
                        int index = box.GetInt32();
                        if (index >= 0)
                        {
                            return value;
                        }
                    }
                }
                // The value is not valid therefore stop the binding right here
                return Windows.UI.Xaml.DependencyProperty.UnsetValue;
            }
        }
    }
}
