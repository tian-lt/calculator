// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#pragma once

#include <string>
#include <vector>

#include "../ExpressionCommandInterface.h"

// Callback interface to be implemented by the clients of CCalcEngine if they require equation history

struct HistoryToken
{
    std::wstring OpCodeString;
    int CommandIndex;
};

class IHistoryDisplay
{
public:
    virtual ~IHistoryDisplay() = default;
    virtual size_t AddToHistory(std::vector<HistoryToken> tokens, std::vector<std::unique_ptr<IExpressionCommand>> commands, std::wstring result) = 0;
};
