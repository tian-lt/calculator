// Licensed under the MIT License.

namespace CalculatorApp.Model
{
    public struct UnitCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool SupportsNegative { get; set; }
    }
}
