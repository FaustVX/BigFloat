using System;
using Xunit;
using Xunit.Abstractions;

namespace BigFloat.UnitTest
{
    public class BigFloatUnitTest
    {
        private readonly ITestOutputHelper output;

        public BigFloatUnitTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestToStringDigits()
        {
            for (var exp = 4; exp >= -4; exp--)
            {
                var testDigits = (decimal)(Math.PI * Math.Pow(10.0, exp));
                output.WriteLine(testDigits.ToString());

                var bigFloat = new System.Numerics.BigFloat(testDigits);
                var str = bigFloat.ToString();
                output.WriteLine(str);

                var compare = decimal.Parse(str);
                Assert.Equal(testDigits, compare);
            }
        }

        [Fact]
        public void TestToStringZeroes()
        {
            for (var exp = 4; exp >= -4; exp--)
            {
                var testDigits = (decimal)(Math.Pow(10.0, exp));
                output.WriteLine(testDigits.ToString());

                var bigFloat = new System.Numerics.BigFloat(testDigits);
                var str = bigFloat.ToString(20, true);
                output.WriteLine(str);

                var compare = decimal.Parse(str);
                Assert.Equal(testDigits, compare);
            }
        }
    }
}
