using System.Collections.Generic;
using Gihan.Helpers.String;
using Xunit;

namespace NaturalStringComparerTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test0()
        {
            var result = NaturalStringComparer.Default.Compare("text 11", "text 2");
            Assert.True(result > 0);
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
    }
}
