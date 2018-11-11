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
            var result = NaturalStringComparer.Default.Compare("vsdj 11", "vsdj 2");
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
            var r = NaturalStringComparer.Default.Compare("aaa", "aaa");
            Assert.True(r == 0);
        }

        [Fact]
        public void Test3()
        {
            var r = NaturalStringComparer.Default.Compare("num 1", "num 2");
            Assert.True(r < 0);
        }

        [Fact]
        public void TestBugOfOnePunchMan()
        {
            var ss = new List<string> { "43.zip", "43.5.zip" };
            ss.Sort(NaturalStringComparer<string>.Default);
            
            Assert.Equal("43.5.zip", ss[0]);
        }
    }
}
