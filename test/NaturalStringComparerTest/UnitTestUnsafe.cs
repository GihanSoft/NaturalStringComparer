namespace NaturalStringComparerTest
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    using GihanSoft.String;

    using Xunit;

    public class UnitTestUnsafe
    {
        private readonly Random rnd = new Random();
        private const int n = 1000000;
        private readonly int[] nums = new int[n];

        private readonly NaturalComparer _sut_Ordinal = new(StringComparison.Ordinal);

        public UnitTestUnsafe()
        {
            for (var i = 0; i < nums.Length; i++)
            {
                nums[i] = rnd.Next();
            }
        }

        [Fact]
        public void TestLargeNumber()
        {
            var num1 = 19015611190635;
            var num2 = 13808910990635;
            var str1 = num1 + "G";
            var str2 = num2 + "X";

            var result = NaturalComparer.Compare(str1, str2, StringComparison.Ordinal);
            Assert.Equal(num1.CompareTo(num2), result);
        }

        [Fact]
        public void TestTooLargeNumber()
        {
            var num1 = BigInteger.Parse("190156111906351901561119063519015611190635");
            var num2 = BigInteger.Parse("138089109906351380891099063513808910990635");
            var str1 = num1 + "G";
            var str2 = num2 + "X";

            var result = NaturalComparer.Compare(str1, str2, StringComparison.Ordinal);
            Assert.Equal(num1.CompareTo(num2), result);
        }

        [Fact]
        public void TestShortAndTooLargeNumber()
        {
            var num1 = BigInteger.Parse("190156111906351901561119063519015611190635");
            var num2 = 138;
            var str1 = num1 + "G";
            var str2 = num2 + "X";

            var result = NaturalComparer.Compare(str1, str2, StringComparison.Ordinal);
            Assert.Equal(num1.CompareTo(num2), result);
        }

        [Fact]
        public void Test00()
        {
            var num1 = rnd.Next();
            var num2 = rnd.Next();
            var result = NaturalComparer.Compare($"text {num1}", $"text {num2}", StringComparison.Ordinal);
            Assert.Equal(num1.CompareTo(num2), result);
        }

        [Fact]
        public void Test01()
        {
            var r1 = NaturalComparer.Compare("_qwerty", "0qwerty", StringComparison.InvariantCulture);
            Assert.True(r1 < 0);
            var r2 = NaturalComparer.Compare("[qwerty", "_qwerty", StringComparison.Ordinal);
            Assert.True(r2 < 0);
        }

        [Fact]
        public void Test02()
        {
            var r = NaturalComparer.Compare("some text", "some text", StringComparison.Ordinal);
            Assert.True(r == 0);
        }

        [Fact]
        public void Test03()
        {
            var r = NaturalComparer.Compare("number 1", "number 2", StringComparison.Ordinal);
            Assert.True(r < 0);
        }

        [Fact]
        public void Test04()
        {
            var r = NaturalComparer.Compare("12 hi 2", "3 hi 11", StringComparison.Ordinal);
            Assert.True(r > 0);
        }

        [Fact]
        public void Test05()
        {
            var r = NaturalComparer.Compare("text 1326 with", "text 999", StringComparison.Ordinal);
            Assert.True(r > 0);
        }

        [Fact]
        public void Test06()
        {
            var r = NaturalComparer.Compare("1278.jpg", "33.jpg", StringComparison.Ordinal);
            Assert.True(r > 0);
        }

        [Fact]
        public void Test07()
        {
            var r = NaturalComparer.Compare("compare 01278 and 3", "compare 1278 and 1000", StringComparison.Ordinal);
            Assert.True(r < 0);
        }

        [Fact]
        public void Test08()
        {
            var r = NaturalComparer.Compare("033", "33", StringComparison.Ordinal);
            Assert.True(r < 0);
        }

        [Fact]
        public void Test08_2()
        {
            var r = NaturalComparer.Compare("33", "33", StringComparison.Ordinal);
            Assert.Equal(0, r);
        }

        [Fact]
        public void Test09()
        {
            var num1 = 911651651;
            var num2 = 911651615;
            var r = NaturalComparer.Compare($"{num1}", $"{num2}", StringComparison.Ordinal);
            Assert.Equal(num1.CompareTo(num2), r);
        }

        [Fact]
        public void Test10()
        {
            var r = NaturalComparer.Compare("hi2", "hi2hi", StringComparison.Ordinal);
            Assert.True(r < 0);
        }

        [Fact]
        public void Test21()
        {
            for (var i = 0; i < n - 1; i++)
            {
                var result = NaturalComparer.Compare($"a long text to show better difference of compare methods {nums[i]}", $"a long text to show better difference of compare methods {nums[i + 1]} hghf", StringComparison.Ordinal);
                Assert.Equal(nums[i].CompareTo(nums[i + 1]), result);
            }
        }

        [Fact]
        void TT()
        {
            var stringList = new List<string>
            {
                "number1", "number2", "number3", "number4", "number10", "number15", "number22", "number26"
                , "number9", "number33", "number5", "number12"
            };

            stringList.Sort(new NaturalComparer(StringComparison.Ordinal));
            //or
            stringList.NaturalSort(); // need "using Gihan.Helpers.Linq;"

            Console.WriteLine("Natural Sort:");
            foreach (var item in stringList)
            {
                Console.WriteLine(item);
            }
            stringList.Sort();
            Console.WriteLine();
            Console.WriteLine("Normal Sort:");
            foreach (var item in stringList)
            {
                Console.WriteLine(item);
            }
            /* output
            Natural Sort:
            number1
            number2
            number3
            number4
            number5
            number9
            number10
            number12
            number15
            number22
            number26
            number33

            Normal Sort:
            number1
            number10
            number12
            number15
            number2
            number22
            number26
            number3
            number33
            number4
            number5
            number9
            */
        }

        public static TheoryData<string, string, int> StringCompareTestData { get; } = new()
        {
            { null, null, 0 },
            { null, "z", -1 },
            { "a", null, 1 },
            { "val1", "val2", -1 },
            { "val2", "val2", 0 },
            { $"val{ulong.MaxValue}", $"val{ulong.MaxValue}", 0 },
            { $"val{new BigInteger(ulong.MaxValue) + 1}", $"val{new BigInteger(ulong.MaxValue) + 1}", 0 },
        };

        [Theory]
        [MemberData(nameof(StringCompareTestData))]
        public void StringCompareTest(string input1, string input2, int expected)
        {
            var actual = _sut_Ordinal.Compare(input1, input2);
            Assert.Equal(expected, actual);
        }
    }
}
