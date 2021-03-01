// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using CalculatorApp.Converters;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Interop;
using Windows.UI.Xaml.Data;

namespace CalculatorApp
{
    namespace Converters
    {
        [Windows.Foundation.Metadata.WebHostHidden]
        public sealed class ItemSizeToVisibilityConverter : Windows.UI.Xaml.Data.IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                var items = (value as int? );
                var boolValue = (items != null && (items.Value == 0));
                return BooleanToVisibilityConverter.Convert(boolValue);
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }

        public sealed class ItemSizeToVisibilityNegationConverter : Windows.UI.Xaml.Data.IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                var items = (value as int?);
                var boolValue = (items != null && (items.Value > 0));
                return BooleanToVisibilityConverter.Convert(boolValue);

            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
    }
}

