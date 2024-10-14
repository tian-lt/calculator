// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#pragma once
#include <vector>

#include "ExpressionCommandInterface.h"
#include "Header Files/IHistoryDisplay.h"

namespace CalculationManager
{
    struct HistoryItem
    {
        std::vector<HistoryToken> Tokens;
        std::vector<std::unique_ptr<IExpressionCommand>> Commands;
        std::wstring Expression;
        std::wstring Result;
    };

    class CalculatorHistory : public IHistoryDisplay
    {
    public:
        CalculatorHistory(size_t maxSize);
        size_t AddToHistory(std::vector<HistoryToken> tokens, std::vector<std::unique_ptr<IExpressionCommand>> commands, std::wstring result) override;
        const std::vector<HistoryItem>& GetHistory() const;
        const HistoryItem& GetHistoryItem(size_t idx) const;
        void ClearHistory();
        size_t AddItem(HistoryItem item);
        bool RemoveItem(size_t idx);
        size_t MaxHistorySize() const
        {
            return m_maxHistorySize;
        }

    private:
        std::vector<HistoryItem> m_historyItems;
        const size_t m_maxHistorySize;
    };
}
