// Licensed under the MIT License.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorApp.Model.UnitTests
{
    [TestClass]
    public class UnitConverterTests
    {
        private class StringUnitConverter : UnitConverter<string, string> { }

        [TestMethod]
        public void RatioTransition()
        {
            var converter = new StringUnitConverter();
            converter.ClaimRatio("b", "a", 2m);
            AssertEqual(6m, converter.Convert("b", "a", 3m));
            AssertEqual(1.5m, converter.Convert("a", "b", 3m));

            converter.ClaimRatio("c", "a", 4m);
            AssertEqual(40m, converter.Convert("c", "a", 10m));
            AssertEqual(10m, converter.Convert("a", "c", 40m));
            AssertEqual(2m, converter.Convert("c", "b", 1m));
            AssertEqual(3m, converter.Convert("b", "c", 6m));

            converter.ClaimRatio("d", "b", 8m);
            AssertEqual(24m, converter.Convert("d", "b", 3m));
            AssertEqual(4m, converter.Convert("b", "d", 32m));
        }

        [TestMethod]
        public void RatioUnion()
        {
            var converter = new StringUnitConverter();
            converter.ClaimRatio("aa", "a", 2m);
            converter.ClaimRatio("bb", "b", 3m);
            converter.ClaimRatio("b", "a", 2m);
            AssertEqual(2m, converter.Convert("b", "a", 1m));
            AssertEqual(6m, converter.Convert("bb", "a", 1m));
            AssertEqual(3m, converter.Convert("bb", "aa", 1m));
            AssertEqual(1m, converter.Convert("aa", "bb", 3m));
            AssertEqual(2m, converter.Convert("a", "bb", 12m));
            AssertEqual(2m, converter.Convert("aa", "bb", 6m));

            converter.ClaimRatio("cc1", "c", 10m);
            converter.ClaimRatio("cc2", "c", 20m);
            converter.ClaimRatio("c", "b", 5);
            AssertEqual(50m, converter.Convert("cc1", "aa", 1m));
            AssertEqual(1m, converter.Convert("aa", "cc2", 100m));
        }

        [TestMethod]
        public void TranslatedRatio()
        {
            var converter = new StringUnitConverter();
            converter.ClaimRatio("celsius", "fahrenheit", 1.8m, 32m);
            converter.ClaimRatio("celsius", "kelvin", 1m, 273.15m);
            AssertEqual(99.5m, converter.Convert("celsius", "fahrenheit", 37.5m));
            AssertEqual(32m, converter.Convert("celsius", "fahrenheit", 0m));
            AssertEqual(50m, converter.Convert("fahrenheit", "celsius", 122m));
            AssertEqual(-15m, converter.Convert("fahrenheit", "celsius", 5m));
            AssertEqual(273.15m, converter.Convert("celsius", "kelvin", 0m));
            AssertEqual(310.65m, converter.Convert("celsius", "kelvin", 37.5m));
            AssertEqual(-173.15m, converter.Convert("kelvin", "celsius", 100m));
            AssertEqual(-279.67m, converter.Convert("kelvin", "fahrenheit", 100m));
            AssertEqual(268.15m, converter.Convert("fahrenheit", "kelvin", 23m));
        }

        [TestMethod]
        public void BadRelationship()
        {
            var converter = new StringUnitConverter();
            Assert.IsTrue(ExpectFailure(() => { converter.ClaimRatio("a", "b", 0m); }));
            Assert.IsTrue(ExpectFailure(() => { converter.ClaimRatio("a", "b", 0m, 1m); }));
            Assert.IsTrue(ExpectFailure(() => { converter.ClaimRatio("a", "a", 1m); }));
            converter.ClaimRatio("a", "b", 1m);
            converter.ClaimRatio("c", "d", 1m);
            Assert.IsTrue(ExpectFailure(() => { converter.Convert("a", "d", 1m); }));
        }

        [TestMethod]
        public void Classify()
        {
            var converter = new StringUnitConverter();
            converter.ClaimRatio("aa", "a", 5m);
            converter.ClaimRatio("bb", "b", 2m);
            converter.Classify("aa", "A");
            converter.Classify("bb", "B");
            Assert.AreEqual("A", converter.Category("a"));
            Assert.AreEqual("A", converter.Category("aa"));
            Assert.AreEqual("B", converter.Category("b"));
            Assert.AreEqual("B", converter.Category("bb"));
            converter.ClaimRatio("a", "b", 1m);
            Assert.AreEqual("A", converter.Category("b"));
            Assert.AreEqual("A", converter.Category("bb"));
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
            if (Math.Abs(expected - actual) > StringUnitConverter.Precision)
            {
                Assert.Fail($"expected = {expected}, actual = {actual}");
            }
        }
    }
}
