// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

//
// CalculatorProgrammerBitFlipPanel.xaml.h
// Declaration of the CalculatorProgrammerBitFlipPanel class
//

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using CalculatorApp.Common;
using CalculatorApp.Controls;
using CalculatorApp.ViewModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;


namespace CalculatorApp
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public sealed partial class CalculatorProgrammerBitFlipPanel
    {
        public CalculatorProgrammerBitFlipPanel()
        {
            m_updatingCheckedStates = false;
            InitializeComponent();
            AssignFlipButtons();
        }

        public bool ShouldEnableBit(BitLength length, int index)
        {
            return index <= GetIndexOfLastBit(length);
        }

        public StandardCalculatorViewModel Model
        {
            get { return (StandardCalculatorViewModel)DataContext; }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SubscribePropertyChanged();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            UnsubscribePropertyChanged();
        }

        private void SubscribePropertyChanged()
        {
            if (Model != null)
            {
                Model.PropertyChanged += OnPropertyChanged;
                m_currentValueBitLength = Model.ValueBitLength;
                UpdateCheckedStates(true);
            }
        }

        private void UnsubscribePropertyChanged()
        {
            if (Model != null)
            {
                Model.PropertyChanged -= OnPropertyChanged;
            }
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == StandardCalculatorViewModel.BinaryDigitsPropertyName)
            {
                UpdateCheckedStates(false);
                m_currentValueBitLength = Model.ValueBitLength;
            }
            else if (
                e.PropertyName == StandardCalculatorViewModel.IsBitFlipCheckedPropertyName
                || e.PropertyName == StandardCalculatorViewModel.IsProgrammerPropertyName)
            {
                if (Model.IsBitFlipChecked && Model.IsProgrammer)
                {
                    // OnBitToggle won't update the automation properties when this control isn't displayed
                    // We need to update all automation properties names manually when the BitFlipPanel is displayed again
                    UpdateAutomationPropertiesNames();
                }
            }
        }

        private void AssignFlipButtons()
        {
            Debug.Assert(m_flipButtons.Length == 64);

            m_flipButtons[0] = Bit0;
            m_flipButtons[1] = Bit1;
            m_flipButtons[2] = Bit2;
            m_flipButtons[3] = Bit3;
            m_flipButtons[4] = Bit4;
            m_flipButtons[5] = Bit5;
            m_flipButtons[6] = Bit6;
            m_flipButtons[7] = Bit7;
            m_flipButtons[8] = Bit8;
            m_flipButtons[9] = Bit9;
            m_flipButtons[10] = Bit10;
            m_flipButtons[11] = Bit11;
            m_flipButtons[12] = Bit12;
            m_flipButtons[13] = Bit13;
            m_flipButtons[14] = Bit14;
            m_flipButtons[15] = Bit15;
            m_flipButtons[16] = Bit16;
            m_flipButtons[17] = Bit17;
            m_flipButtons[18] = Bit18;
            m_flipButtons[19] = Bit19;
            m_flipButtons[20] = Bit20;
            m_flipButtons[21] = Bit21;
            m_flipButtons[22] = Bit22;
            m_flipButtons[23] = Bit23;
            m_flipButtons[24] = Bit24;
            m_flipButtons[25] = Bit25;
            m_flipButtons[26] = Bit26;
            m_flipButtons[27] = Bit27;
            m_flipButtons[28] = Bit28;
            m_flipButtons[29] = Bit29;
            m_flipButtons[30] = Bit30;
            m_flipButtons[31] = Bit31;
            m_flipButtons[32] = Bit32;
            m_flipButtons[33] = Bit33;
            m_flipButtons[34] = Bit34;
            m_flipButtons[35] = Bit35;
            m_flipButtons[36] = Bit36;
            m_flipButtons[37] = Bit37;
            m_flipButtons[38] = Bit38;
            m_flipButtons[39] = Bit39;
            m_flipButtons[40] = Bit40;
            m_flipButtons[41] = Bit41;
            m_flipButtons[42] = Bit42;
            m_flipButtons[43] = Bit43;
            m_flipButtons[44] = Bit44;
            m_flipButtons[45] = Bit45;
            m_flipButtons[46] = Bit46;
            m_flipButtons[47] = Bit47;
            m_flipButtons[48] = Bit48;
            m_flipButtons[49] = Bit49;
            m_flipButtons[50] = Bit50;
            m_flipButtons[51] = Bit51;
            m_flipButtons[52] = Bit52;
            m_flipButtons[53] = Bit53;
            m_flipButtons[54] = Bit54;
            m_flipButtons[55] = Bit55;
            m_flipButtons[56] = Bit56;
            m_flipButtons[57] = Bit57;
            m_flipButtons[58] = Bit58;
            m_flipButtons[59] = Bit59;
            m_flipButtons[60] = Bit60;
            m_flipButtons[61] = Bit61;
            m_flipButtons[62] = Bit62;
            m_flipButtons[63] = Bit63;
        }

        private void OnBitToggled(object sender, RoutedEventArgs e)
        {
            if (m_updatingCheckedStates)
            {
                return;
            }

            // Handle this the bit toggled event only if it is coming from BitFlip mode.
            // Any input from the Numpad may also result in toggling the bit as their state is bound to the BinaryDisplayValue.
            // Also, if the mode is switched to other Calculator modes when the BitFlip panel is open,
            // a race condition exists in which the IsProgrammerMode property is still true and the UpdatePrimaryResult() is called,
            // which continuously alters the Display Value and the state of the Bit Flip buttons.
            if (Model.IsBitFlipChecked && Model.IsProgrammer)
            {
                var flipButton = (FlipButtons)sender;
                int index = (int)flipButton.Tag;
                flipButton.SetValue(AutomationProperties.NameProperty, GenerateAutomationPropertiesName(index, flipButton.IsChecked.Value));
                Model.ButtonPressed.Execute(flipButton.ButtonId);
            }
        }

        private string GenerateAutomationPropertiesName(int position, bool value)
        {
            var resourceLoader = AppResourceProvider.GetInstance();
            string automationNameTemplate = resourceLoader.GetResourceString("BitFlipItemAutomationName");
            string bitPosition;
            if (position == 0)
            {
                bitPosition = resourceLoader.GetResourceString("LeastSignificantBit");
            }
            else
            {
                int lastPosition = -1;
                if (Model != null)
                {
                    lastPosition = GetIndexOfLastBit(Model.ValueBitLength);
                }

                if (position == lastPosition)
                {
                    bitPosition = resourceLoader.GetResourceString("MostSignificantBit");
                }
                else
                {
                    string indexName = resourceLoader.GetResourceString(position.ToString());
                    string bitPositionTemplate = resourceLoader.GetResourceString("BitPosition");
                    bitPosition = LocalizationStringUtil.GetLocalizedString(bitPositionTemplate, indexName);
                }
            }
            return LocalizationStringUtil.GetLocalizedString(automationNameTemplate, bitPosition, value ? "1" : "0");
        }

        private void UpdateCheckedStates(bool updateAutomationPropertiesNames)
        {
            Debug.Assert(!m_updatingCheckedStates);
            Debug.Assert(m_flipButtons.Length == s_numBits);

            if (Model == null)
            {
                return;
            }

            m_updatingCheckedStates = true;
            // CSHARP_MIGRATION:
            // iterator and index move at the same time with same step
            // add index validation in the loop
            int index = 0;
            bool mustUpdateTextOfMostSignificantDigits = m_currentValueBitLength != Model.ValueBitLength;
            int previousMSDPosition = GetIndexOfLastBit(m_currentValueBitLength);
            int newMSDPosition = GetIndexOfLastBit(Model.ValueBitLength);
            foreach (bool val in Model.BinaryDigits)
            {
                if (index < m_flipButtons.Length)
                {
                    bool hasValueChanged = m_flipButtons[index].IsChecked.Value != val;
                    m_flipButtons[index].IsChecked = val;
                    if (updateAutomationPropertiesNames
                        || hasValueChanged
                        || (mustUpdateTextOfMostSignificantDigits && (index == previousMSDPosition || index == newMSDPosition)))
                    {
                        m_flipButtons[index].SetValue(AutomationProperties.NameProperty, GenerateAutomationPropertiesName(index, val));
                    }
                    ++index;
                }
            }

            m_updatingCheckedStates = false;
        }

        private void UpdateAutomationPropertiesNames()
        {
            foreach (FlipButtons flipButton in m_flipButtons)
            {
                int index = (int)flipButton.Tag;
                flipButton.SetValue(AutomationProperties.NameProperty, GenerateAutomationPropertiesName(index, flipButton.IsChecked.Value));
            }
        }

        // CSHARP_MIGRATION: TODO:
        // c++ const method. c# readonly method is available for c# 8.0+
        private int GetIndexOfLastBit(BitLength length)
        {
            switch (length)
            {
                case BitLength.BitLengthQWord:
                    return 63;
                case BitLength.BitLengthDWord:
                    return 31;
                case BitLength.BitLengthWord:
                    return 15;
                case BitLength.BitLengthByte:
                    return 7;
            }
            return -1;
        }

        private static readonly uint s_numBits = 64;
        private FlipButtons[] m_flipButtons = new FlipButtons[s_numBits];
        private bool m_updatingCheckedStates;
        private BitLength m_currentValueBitLength;
    }
}
