namespace NaturalStringComparerTest
{
    using System;

    using GihanSoft.String;

    using Xunit;

    public class UnitTestUnsafe
    {
        private readonly Random rnd = new Random();
        private const int n = 1000000;
        private readonly int[] nums = new int[n];

        public UnitTestUnsafe()
        {
            for (var i = 0; i < nums.Length; i++)
            {
                nums[i] = rnd.Next();
            }
        }

        [Fact]
        public void Test00()
        {
            var num1 = rnd.Next();
            var num2 = rnd.Next();
            var result = NaturalComparer.Ordinal.Compare($"text {num1}", $"text {num2}");
            Assert.Equal(num1.CompareTo(num2), result);
        }

        [Fact]
        public void Test01()
        {
            var r1 = NaturalComparer.Compare("_qwerty", "0qwerty", StringComparison.InvariantCulture);
            Assert.True(r1 < 0);
            var r2 = NaturalComparer.Ordinal.Compare("[qwerty", "_qwerty");
            Assert.True(r2 < 0);
        }

        [Fact]
        public void Test02()
        {
            var r = NaturalComparer.Ordinal.Compare("some text", "some text");
            Assert.True(r == 0);
        }

        [Fact]
        public void Test03()
        {
            var r = NaturalComparer.Ordinal.Compare("number 1", "number 2");
            Assert.True(r < 0);
        }

        [Fact]
        public void Test04()
        {
            var r = NaturalComparer.Ordinal.Compare("12 hi 2", "3 hi 11");
            Assert.True(r > 0);
        }

        [Fact]
        public void Test05()
        {
            var r = NaturalComparer.Ordinal.Compare("text 1326 with", "text 999");
            Assert.True(r > 0);
        }

        [Fact]
        public void Test06()
        {
            var r = NaturalComparer.Ordinal.Compare("1278.jpg", "33.jpg");
            Assert.True(r > 0);
        }

        [Fact]
        public void Test07()
        {
            var r = NaturalComparer.Ordinal.Compare("compare 01278 and 3", "compare 1278 and 1000");
            Assert.True(r < 0);
        }

        [Fact]
        public void Test08()
        {
            var r = NaturalComparer.Ordinal.Compare("033", "33");
            Assert.Equal(0, r);
        }

        [Fact]
        public void Test09()
        {
            var num1 = 911651651;
            var num2 = 911651615;
            var r = NaturalComparer.Ordinal.Compare($"{num1}", $"{num2}");
            Assert.Equal(num1.CompareTo(num2), r);
        }

        [Fact]
        public void Test10()
        {
            var r = NaturalComparer.Ordinal.Compare("hi2", "hi2hi");
            Assert.True(r < 0);
        }

        [Fact]
        public void Test21()
        {
            for (var i = 0; i < n - 1; i++)
            {
                var result = NaturalComparer.Ordinal.Compare(
                    $"a long text to show better difference of compare methods {nums[i]}",
                    $"a long text to show better difference of compare methods {nums[i + 1]} hghf");
                Assert.Equal(nums[i].CompareTo(nums[i + 1]), result);
            }
        }
    }
}
