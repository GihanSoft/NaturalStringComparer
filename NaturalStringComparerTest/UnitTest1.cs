using System;
using Xunit;

namespace NaturalStringComparerTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var nc = new Gihan.Helpers.StringHelper.NaturalStringComparer();
            nc.Compare("_qwerty", "0qwerty");
        }
    }
}
