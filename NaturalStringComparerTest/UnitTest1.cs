using System;
using System.Collections.Generic;
using Gihan.Helpers.String;
using Gihan.Helpers.Linq;
using Xunit;
using System.Linq;

namespace NaturalStringComparerTest
{
    public class UnitTest1
    {
        private readonly Random rnd = new Random();

        [Fact]
        public void Test0()
        {
            var num1 = rnd.Next();
            var num2 = rnd.Next();
            var result = NaturalStringComparer.Default.Compare($"text {num1}", $"text {num2}");
            Assert.Equal(num1.CompareTo(num2), result);
        }

        [Fact]
        public void Test1()
        {
            var r = NaturalStringComparer.Default.Compare("_qwerty", "0qwerty");
            Assert.True(r > 0);
        }

        [Fact]
        public void Test2()
        {
            var r = NaturalStringComparer.Default.Compare("some text", "some text");
            Assert.True(r == 0);
        }

        [Fact]
        public void Test3()
        {
            var r = NaturalStringComparer.Default.Compare("number 1", "number 2");
            Assert.True(r < 0);
        }

        [Fact]
        public void Test4()
        {
            var r = NaturalStringComparer.Default.Compare("12 hi 2", "3 hi 11");
            Assert.True(r > 0);
        }

        [Fact]
        public void Test5()
        {
            var r = NaturalStringComparer.Default.Compare("text 1326 with", "text 999");
            Assert.True(r > 0);
        }

        [Fact]
        public void Test6()
        {
            var r = NaturalStringComparer.Default.Compare("1278.jpg", "33.jpg");
            Assert.True(r > 0);
        }

        [Fact]
        public void Test7()
        {
            var r = NaturalStringComparer.Default.Compare("compare 01278 and 3", "compare 1278 and 1000");
            Assert.True(r < 0);
        }

        [Fact]
        public void Test8()
        {
            var r = NaturalStringComparer.Default.Compare("033", "33");
            Assert.Equal(0, r);
        }

        [Fact]
        public void Test9()
        {
            var num1 = 911651651;
            var num2 = 911651615;
            var r = NaturalStringComparer.Default.Compare($"{num1}", $"{num2}");
            Assert.Equal(num1.CompareTo(num2), r);
        }

        [Fact]
        public void Test10()
        {
            var r = NaturalStringComparer.Default.Compare("hi2", "hi2hi");
            Assert.True(r < 0);
        }
    }
}
