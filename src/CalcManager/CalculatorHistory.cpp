// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#include <cassert>
#include "CalculatorHistory.h"

using namespace std;
using namespace CalculationManager;

namespace
{
    wstring GetGeneratedExpression(const std::vector<HistoryToken>& tokens)
    {
        wstring expression;
        bool isFirst = true;

        for (auto const& token : tokens)
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                expression += L' ';
            }
            expression.append(token.OpCodeString);
        }

        return expression;
    }
} // namespace

CalculatorHistory::CalculatorHistory(size_t maxSize)
    : m_maxHistorySize(maxSize)
{
}

size_t CalculatorHistory::AddToHistory(std::vector<HistoryToken> tokens, std::vector<std::unique_ptr<IExpressionCommand>> commands, std::wstring result)
{
    auto expr = GetGeneratedExpression(tokens);
    return AddItem(HistoryItem{ std::move(tokens), std::move(commands), std::move(expr), std::move(result) });
}

size_t CalculatorHistory::AddItem(HistoryItem item)
{
    if (m_historyItems.size() >= m_maxHistorySize)
    {
        m_historyItems.erase(m_historyItems.begin());
    }
    m_historyItems.push_back(std::move(item));
    return static_cast<unsigned>(m_historyItems.size() - 1);
}

bool CalculatorHistory::RemoveItem(size_t idx)
{
    if (idx < m_historyItems.size())
    {
        m_historyItems.erase(m_historyItems.begin() + idx);
        return true;
    }
    return false;
}

const std::vector<HistoryItem>& CalculatorHistory::GetHistory() const
{
    return m_historyItems;
}

const HistoryItem& CalculatorHistory::GetHistoryItem(size_t idx) const
{
    assert(idx < m_historyItems.size());
    return m_historyItems.at(idx);
}

void CalculatorHistory::ClearHistory()
{
    m_historyItems.clear();
}
