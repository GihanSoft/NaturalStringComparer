
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gihan.Helpers.StringHelper;
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
            var folders = new List<string> { "43", "43.5" };
            folders.Sort(NaturalStringComparer<string>.Default);
        }
    }
}
