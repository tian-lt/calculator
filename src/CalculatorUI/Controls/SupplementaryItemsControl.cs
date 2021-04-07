// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CalculatorApp.ViewModel;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;

namespace CalculatorApp
{
    namespace Controls
    {
        public sealed class SupplementaryItemsControl : Windows.UI.Xaml.Controls.ItemsControl
        {
            public SupplementaryItemsControl()
            {
            }

            protected override DependencyObject GetContainerForItemOverride()
            {
                return new SupplementaryContentPresenter();
            }

            protected override void PrepareContainerForItemOverride(DependencyObject element, Object item)
            {
                base.PrepareContainerForItemOverride(element, item);

                var supplementaryResult = (item as SupplementaryResult);
                if (supplementaryResult != null)
                {
                    AutomationProperties.SetName(element, supplementaryResult.GetLocalizedAutomationName());
                }
            }
        }

        public sealed class SupplementaryContentPresenter : Windows.UI.Xaml.Controls.ContentPresenter
        {
            public SupplementaryContentPresenter()
            {
            }

            protected override AutomationPeer OnCreateAutomationPeer()
            {
                return new SupplementaryContentPresenterAP(this);
            }
        }

        sealed class SupplementaryContentPresenterAP : Windows.UI.Xaml.Automation.Peers.FrameworkElementAutomationPeer
        {
            protected override Windows.UI.Xaml.Automation.Peers.AutomationControlType GetAutomationControlTypeCore()
            {
                return Windows.UI.Xaml.Automation.Peers.AutomationControlType.Text;
            }

            protected override IList<Windows.UI.Xaml.Automation.Peers.AutomationPeer> GetChildrenCore()
            {
                return null;
            }

            internal SupplementaryContentPresenterAP(SupplementaryContentPresenter owner)
                : base(owner)
            {
            }
        }
    }
}

