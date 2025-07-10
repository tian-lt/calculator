// Licensed under the MIT License.

using CalculatorApp.ViewModel.Common;

namespace CalculatorApp.Model
{
    public class UnitConverterModel
    {
        private readonly UnitConverter<UnitKind, ViewMode> _converter = new UnitConverter<UnitKind, ViewMode>();

        public void Initialize()
        {
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_Acre, 4046.8564224m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_SquareFoot, 0.09290304m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_SquareYard, 0.83612736m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_SquareMillimeter, 0.000001m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_SquareCentimeter, 0.0001m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_SquareKilometer, 1000000);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_SquareInch, 0.00064516m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_SquareMile, 2589988.110336m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_Hectare, 0.012516104m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_Paper, 0.06032246m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_SoccerField, 10869.66m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_Castle, 100000m);
            _converter.ClaimRatio(UnitKind.Area_SquareMeter, UnitKind.Area_Pyeong, 400m / 121m);
            _converter.Classify(UnitKind.Area_SquareMeter, ViewMode.Area);

            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Bit, 0.000000125m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Nibble, 0.0000005m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Byte, 0.000001m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Kilobyte, 0.001m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Gigabyte, 1000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Terabyte, 1000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Petabyte, 1000000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Exabytes, 1000000000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Zetabytes, 1000000000000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Yottabyte, 1000000000000000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Kilobit, 0.000125m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Megabit, 0.125m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Gigabit, 125m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Terabit, 125000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Petabit, 125000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Exabits, 125000000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Zetabits, 125000000000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Yottabit, 125000000000000000m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Gibibits, 134.217728m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Gibibytes, 1073.741824m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Kibibits, 0.000128m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Kibibytes, 0.001024m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Mebibits, 0.131072m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Mebibytes, 1.048576m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Pebibits, 140737488.355328m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Pebibytes, 1125899906.842624m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Tebibits, 137438.953472m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Tebibytes, 1099511.627776m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Exbibits, 144115188075.855872m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Exbibytes, 1152921504606.846976m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Zebibits, 147573952589676.412928m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Zebibytes, 1180591620717411.303424m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Yobibits, 151115727451828646.838272m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_Yobibytes, 1208925819614629174.706176m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_FloppyDisk, 1.474560m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_CD, 700m);
            _converter.ClaimRatio(UnitKind.Data_Megabyte, UnitKind.Data_DVD, 4700m);
            _converter.Classify(UnitKind.Data_Megabyte, ViewMode.Data);

            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_Calorie, 4.184m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_Kilocalorie, 4184m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_BritishThermalUnit, 1055.056m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_Kilojoule, 1000m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_Kilowatthour, 3600000m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_ElectronVolt, 0.0000000000000000001602176565m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_FootPound, 1.3558179483314m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_Battery, 9000m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_Banana, 439614m);
            _converter.ClaimRatio(UnitKind.Energy_Joule, UnitKind.Energy_SliceOfCake, 1046700m);
            _converter.Classify(UnitKind.Energy_Joule, ViewMode.Energy);

            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Inch, 0.0254m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Foot, 0.3048m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Yard, 0.9144m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Mile, 1609.344m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Micron, 0.000001m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Millimeter, 0.001m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Nanometer, 0.000000001m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Angstrom, 0.0000000001m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Centimeter, 0.01m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Kilometer, 1000m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_NauticalMile, 1852m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Paperclip, 0.035052m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_Hand, 0.18669m);
            _converter.ClaimRatio(UnitKind.Length_Meter, UnitKind.Length_JumboJet, 76m);
            _converter.Classify(UnitKind.Length_Meter, ViewMode.Length);

            _converter.ClaimRatio(UnitKind.Power_Watt, UnitKind.Power_BritishThermalUnitPerMinute, 17.58426666666667m);
            _converter.ClaimRatio(UnitKind.Power_Watt, UnitKind.Power_FootPoundPerMinute, 0.0225969658055233m);
            _converter.ClaimRatio(UnitKind.Power_Watt, UnitKind.Power_Kilowatt, 1000m);
            _converter.ClaimRatio(UnitKind.Power_Watt, UnitKind.Power_Horsepower, 745.69987158227022m);
            _converter.ClaimRatio(UnitKind.Power_Watt, UnitKind.Power_LightBulb, 60m);
            _converter.ClaimRatio(UnitKind.Power_Watt, UnitKind.Power_Horse, 745.7m);
            _converter.ClaimRatio(UnitKind.Power_Watt, UnitKind.Power_TrainEngine, 2982799.486329081m);
            _converter.Classify(UnitKind.Power_Watt, ViewMode.Power);

            _converter.ClaimRatio(UnitKind.Time_Second, UnitKind.Time_Day, 86400m);
            _converter.ClaimRatio(UnitKind.Time_Second, UnitKind.Time_Week, 604800m);
            _converter.ClaimRatio(UnitKind.Time_Second, UnitKind.Time_Year, 31557600m);
            _converter.ClaimRatio(UnitKind.Time_Second, UnitKind.Time_Millisecond, 0.001m);
            _converter.ClaimRatio(UnitKind.Time_Second, UnitKind.Time_Microsecond, 0.000001m);
            _converter.ClaimRatio(UnitKind.Time_Second, UnitKind.Time_Minute, 60m);
            _converter.ClaimRatio(UnitKind.Time_Second, UnitKind.Time_Hour, 3600m);
            _converter.Classify(UnitKind.Time_Second, ViewMode.Time);

            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_CupUS, 236.588237m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_PintUS, 473.176473m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_PintUK, 568.26125m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_QuartUS, 946.352946m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_QuartUK, 1136.5225m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_GallonUS, 3785.411784m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_GallonUK, 4546.09m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_Liter, 1000m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_TeaspoonUS, 4.92892159375m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_TablespoonUS, 14.78676478125m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_CubicCentimeter, 1m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_CubicYard, 764554.857984m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_CubicMeter, 1000000m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_CubicInch, 16.387064m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_CubicFoot, 28316.846592m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_FluidOunceUS, 29.5735295625m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_FluidOunceUK, 28.4130625m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_TeaspoonUK, 5.91938802083333333333m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_TablespoonUK, 17.7581640625m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_CoffeeCup, 236.5882m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_Bathtub, 378541.2m);
            _converter.ClaimRatio(UnitKind.Volume_Milliliter, UnitKind.Volume_SwimmingPool, 3750000000m);
            _converter.Classify(UnitKind.Volume_Milliliter, ViewMode.Volume);

            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Hectogram, 0.1m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Decagram, 0.01m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Gram, 0.001m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Pound, 0.45359237m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Ounce, 0.028349523125m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Milligram, 0.000001m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Centigram, 0.00001m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Decigram, 0.0001m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_LongTon, 1016.0469088m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Tonne, 1000m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Stone, 6.35029318m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Carat, 0.0002m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_ShortTon, 907.18474m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Snowflake, 0.000002m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_SoccerBall, 0.4325m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Elephant, 4000m);
            _converter.ClaimRatio(UnitKind.Weight_Kilogram, UnitKind.Weight_Whale, 90000m);
            _converter.Classify(UnitKind.Weight_Kilogram, ViewMode.Weight);

            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_FeetPerSecond, 30.48m);
            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_KilometersPerHour, 27.777777777777777777778m);
            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_Knot, 51.44m);
            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_Mach, 34030m);
            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_MetersPerSecond, 100m);
            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_MilesPerHour, 44.7m);
            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_Turtle, 8.94m);
            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_Horse, 2011.5m);
            _converter.ClaimRatio(UnitKind.Speed_CentimetersPerSecond, UnitKind.Speed_Jet, 24585m);
            _converter.Classify(UnitKind.Speed_CentimetersPerSecond, ViewMode.Speed);

            _converter.ClaimRatio(UnitKind.Angle_Degree, UnitKind.Angle_Radian, 57.29577951308233m);
            _converter.ClaimRatio(UnitKind.Angle_Degree, UnitKind.Angle_Gradian, 0.9m);
            _converter.Classify(UnitKind.Angle_Degree, ViewMode.Angle);

            _converter.ClaimRatio(UnitKind.Pressure_Atmosphere, UnitKind.Pressure_Bar, 0.9869232667160128m);
            _converter.ClaimRatio(UnitKind.Pressure_Atmosphere, UnitKind.Pressure_KiloPascal, 0.0098692326671601m);
            _converter.ClaimRatio(UnitKind.Pressure_Atmosphere, UnitKind.Pressure_MillimeterOfMercury, 0.0013155687145324m);
            _converter.ClaimRatio(UnitKind.Pressure_Atmosphere, UnitKind.Pressure_Pascal, 9.869232667160128e-6m);
            _converter.ClaimRatio(UnitKind.Pressure_Atmosphere, UnitKind.Pressure_PSI, 0.068045961016531m);
            _converter.Classify(UnitKind.Pressure_Atmosphere, ViewMode.Pressure);

            _converter.ClaimRatio(UnitKind.Temperature_DegreesCelsius, UnitKind.Temperature_DegreesFahrenheit, 1.8m, 32m);
            _converter.ClaimRatio(UnitKind.Temperature_DegreesCelsius, UnitKind.Temperature_Kelvin, 1m, 273.15m);
        }

        public decimal Convert(UnitKind from, UnitKind to, decimal value) =>
            _converter.Convert(from, to, value);
    }
}
