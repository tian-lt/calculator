// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using CalculatorApp;
using CalculatorApp.Common;
using CalculatorApp.Controls;

using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Data;

namespace CalculatorApp
{
    namespace Controls
    {
        public sealed class RadixButton : Windows.UI.Xaml.Controls.RadioButton
        {
            public RadixButton()
            {}

            internal string GetRawDisplayValue()
            {
                string radixContent = Content.ToString();
                return LocalizationSettings.GetInstance().RemoveGroupSeparators(radixContent);
            }
        }
    }
}
