
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
            Assert.IsTrue(ConsiderEqual(6m, converter.Convert("a", "b", 3m)));
            Assert.IsTrue(ConsiderEqual(1.5m, converter.Convert("b", "a", 3m)));

            converter.ClaimRatio("a", "c", 4m);
            Assert.IsTrue(ConsiderEqual(40m, converter.Convert("a", "c", 10m)));
            Assert.IsTrue(ConsiderEqual(10m, converter.Convert("c", "a", 40m)));
            Assert.IsTrue(ConsiderEqual(2m, converter.Convert("b", "c", 1m)));
            Assert.IsTrue(ConsiderEqual(3m, converter.Convert("c", "b", 6m)));

            converter.ClaimRatio("b", "d", 8m);
            Assert.IsTrue(ConsiderEqual(24m, converter.Convert("b", "d", 3m)));
            Assert.IsTrue(ConsiderEqual(4m, converter.Convert("d", "b", 32m)));
        }

        [TestMethod]
        public void RatioUnion()
        {
            var converter = new UnitConverter<string>();
            converter.ClaimRatio("a", "aa", 2m);
            converter.ClaimRatio("b", "bb", 3m);
            converter.ClaimRatio("a", "b", 2m);
            Assert.IsTrue(ConsiderEqual(2m, converter.Convert("a", "b", 1m)));
            Assert.IsTrue(ConsiderEqual(6m, converter.Convert("a", "bb", 1m)));
            Assert.IsTrue(ConsiderEqual(3m, converter.Convert("aa", "bb", 1m)));
            Assert.IsTrue(ConsiderEqual(1m, converter.Convert("bb", "aa", 3m)));
            Assert.IsTrue(ConsiderEqual(2m, converter.Convert("bb", "a", 12m)));
            Assert.IsTrue(ConsiderEqual(2m, converter.Convert("bb", "aa", 6m)));

            converter.ClaimRatio("c", "cc1", 10m);
            converter.ClaimRatio("c", "cc2", 20m);
            converter.ClaimRatio("b", "c", 5);
            Assert.IsTrue(ConsiderEqual(50m, converter.Convert("aa", "cc1", 1m)));
            Assert.IsTrue(ConsiderEqual(1m, converter.Convert("cc2", "aa", 100m)));
        }

        [TestMethod]
        public void BadRelationship()
        {
            var converter = new UnitConverter<string>();
            Assert.IsTrue(ExpectFailure(() => { converter.ClaimRatio("a", "b", 0m); }));
            Assert.IsTrue(ExpectFailure(() => { converter.ClaimRatio("a", "a", 1m); }));
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

        private static bool ConsiderEqual(decimal a, decimal b)
        {
            return Math.Abs(a - b) < UnitConverter<string>.Precision;
        }
    }
}
