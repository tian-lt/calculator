
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorApp.Model.UnitTests
{
    [TestClass]
    public class UnitConverterTests
    {
        [TestMethod]
        public void RatioTransition()
        {
            var converter = new UnitConverter<string>();
            converter.ClaimRatio("a", "b", 2m);
            AssertEqual(6m, converter.Convert("a", "b", 3m));
            AssertEqual(1.5m, converter.Convert("b", "a", 3m));

            converter.ClaimRatio("a", "c", 4m);
            AssertEqual(40m, converter.Convert("a", "c", 10m));
            AssertEqual(10m, converter.Convert("c", "a", 40m));
            AssertEqual(2m, converter.Convert("b", "c", 1m));
            AssertEqual(3m, converter.Convert("c", "b", 6m));

            converter.ClaimRatio("b", "d", 8m);
            AssertEqual(24m, converter.Convert("b", "d", 3m));
            AssertEqual(4m, converter.Convert("d", "b", 32m));
        }

        [TestMethod]
        public void RatioUnion()
        {
            var converter = new UnitConverter<string>();
            converter.ClaimRatio("a", "aa", 2m);
            converter.ClaimRatio("b", "bb", 3m);
            converter.ClaimRatio("a", "b", 2m);
            AssertEqual(2m, converter.Convert("a", "b", 1m));
            AssertEqual(6m, converter.Convert("a", "bb", 1m));
            AssertEqual(3m, converter.Convert("aa", "bb", 1m));
            AssertEqual(1m, converter.Convert("bb", "aa", 3m));
            AssertEqual(2m, converter.Convert("bb", "a", 12m));
            AssertEqual(2m, converter.Convert("bb", "aa", 6m));

            converter.ClaimRatio("c", "cc1", 10m);
            converter.ClaimRatio("c", "cc2", 20m);
            converter.ClaimRatio("b", "c", 5);
            AssertEqual(50m, converter.Convert("aa", "cc1", 1m));
            AssertEqual(1m, converter.Convert("cc2", "aa", 100m));
        }

        [TestMethod]
        public void TranslatedRatio()
        {
            var converter = new UnitConverter<string>();
            converter.ClaimRatio("fahrenheit", "celsius", 1.8m, 32m);
            converter.ClaimRatio("kelvin", "celsius", 1m, 273.15m);
            AssertEqual(99.5m, converter.Convert("fahrenheit", "celsius", 37.5m));
            AssertEqual(32m, converter.Convert("fahrenheit", "celsius", 0m));
            AssertEqual(50m, converter.Convert("celsius", "fahrenheit", 122m));
            AssertEqual(-15m, converter.Convert("celsius", "fahrenheit", 5m));
            AssertEqual(273.15m, converter.Convert("kelvin", "celsius", 0m));
            AssertEqual(310.65m, converter.Convert("kelvin", "celsius", 37.5m));
            AssertEqual(-173.15m, converter.Convert("celsius", "kelvin", 100m));
            AssertEqual(-279.67m, converter.Convert("fahrenheit", "kelvin", 100m));
            AssertEqual(268.15m, converter.Convert("kelvin", "fahrenheit", 23m));
        }

        [TestMethod]
        public void BadRelationship()
        {
            var converter = new UnitConverter<string>();
            Assert.IsTrue(ExpectFailure(() => { converter.ClaimRatio("a", "b", 0m); }));
            Assert.IsTrue(ExpectFailure(() => { converter.ClaimRatio("a", "b", 0m, 1m); }));
            Assert.IsTrue(ExpectFailure(() => { converter.ClaimRatio("a", "a", 1m); }));
            converter.ClaimRatio("a", "b", 1m);
            converter.ClaimRatio("c", "d", 1m);
            Assert.IsTrue(ExpectFailure(() => { converter.Convert("a", "d", 1m); }));
        }

        private static bool ExpectFailure(Action op)
        {
            bool res = false;
            try
            {
                op();
            }
            catch (Exception)
            {
                res = true;
            }
            return res;
        }

        private static void AssertEqual(decimal expected, decimal actual)
        {
            if (Math.Abs(expected - actual) > UnitConverter<string>.Precision)
            {
                Assert.Fail($"expected = {expected}, actual = {actual}");
            }
        }
    }
}
