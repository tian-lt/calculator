// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#pragma once

#include "Utils.h"

namespace CalculatorApp
{
    // CSHARP_MIGRATION: TODO: this is a temporary solution to export Utils' capabilities up to C# UI layer
    public ref class ViewModelUtilities sealed
    {
    public:
        static int GetWindowId();
    };
}


