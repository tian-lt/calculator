// Licensed under the MIT License.

namespace CalculatorApp.Model
{
    public class Unit
    {
        public string Name { get; set; }
        public string AccessibleName { get; set; }
        public string Abbreviation { get; set; }
        bool IsWhimsical { get; set; }
        int UnitId { get; set; }

        public override string ToString()
        {
            return AccessibleName;
        }
    }
}
