// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// This class provides the concrete implementation for the ICalcDisplay interface
// that is declared in the Calculation Manager Library.
#include "pch.h"
#include "CalculatorDisplay.h"
#include "StandardCalculatorViewModel.h"

using namespace CalculatorApp;
using namespace CalculatorApp::ViewModel;
using namespace CalculatorApp::ViewModel::Common;
using namespace CalculationManager;
using namespace Platform;
using namespace std;

CalculatorApp::ViewModel::Common::CalculatorDisplay::CalculatorDisplay()
{
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::SetCallback(Platform::WeakReference callbackReference)
{
    m_callbackReference = callbackReference;
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::SetHistoryCallback(Platform::WeakReference callbackReference)
{
    m_historyCallbackReference = callbackReference;
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::SetPrimaryDisplay(_In_ const wstring& displayStringValue, _In_ bool isError)
{
    if (m_callbackReference)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->SetPrimaryDisplay(StringReference(displayStringValue.c_str()), isError);
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::SetParenthesisNumber(_In_ unsigned int parenthesisCount)
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->SetParenthesisCount(parenthesisCount);
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::OnNoRightParenAdded()
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->OnNoRightParenAdded();
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::SetIsInError(bool isError)
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->IsInError = isError;
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::SetExpressionDisplay(
    _Inout_ std::shared_ptr<std::vector<std::pair<std::wstring, int>>> const& tokens,
    _Inout_ std::shared_ptr<std::vector<std::shared_ptr<IExpressionCommand>>> const& commands)
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->SetExpressionDisplay(tokens, commands);
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::SetMemorizedNumbers(_In_ const vector<std::wstring>& newMemorizedNumbers)
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->SetMemorizedNumbers(newMemorizedNumbers);
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::OnHistoryItemAdded(_In_ unsigned int addedItemIndex)
{
    if (m_historyCallbackReference != nullptr)
    {
        if (auto historyVM = m_historyCallbackReference.Resolve<ViewModel::HistoryViewModel>())
        {
            historyVM->OnHistoryItemAdded(addedItemIndex);
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::MaxDigitsReached()
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->OnMaxDigitsReached();
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::BinaryOperatorReceived()
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->OnBinaryOperatorReceived();
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::MemoryItemChanged(unsigned int indexOfMemory)
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->OnMemoryItemChanged(indexOfMemory);
        }
    }
}

void CalculatorApp::ViewModel::Common::CalculatorDisplay::InputChanged()
{
    if (m_callbackReference != nullptr)
    {
        if (auto calcVM = m_callbackReference.Resolve<ViewModel::StandardCalculatorViewModel>())
        {
            calcVM->OnInputChanged();
        }
    }
}
