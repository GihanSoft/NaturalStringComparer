using System;
using System.Collections.Generic;

namespace Gihan.Helpers.StringHelper
{
    public class NaturalStringComparer : IComparer<string>
    {
        private static NaturalStringComparer _default;
        private static readonly object Look = new object();

        public static NaturalStringComparer Default
        {
            get
            {
                lock (Look)
                {
                    return _default ?? (_default = new NaturalStringComparer());
                }
            }
        }

        public int Compare(string x, string y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (string.IsNullOrEmpty(x)) throw new ArgumentException("Arguman is Empty", nameof(x));
            if (string.IsNullOrEmpty(y)) throw new ArgumentException("Arguman is Empty", nameof(y));

            int xp = 0, yp = 0;
            while (xp < x.Length)
            {
                if (yp == y.Length)
                    return 1;
                if (char.IsDigit(x[xp]) && char.IsDigit(y[yp]))
                {
                    var xNum = StrToInt(x, xp);
                    var yNum = StrToInt(y, yp);
                    var nDiff = Comparer<int>.Default.Compare(xNum, yNum);
                    if (nDiff != 0)
                        return nDiff;

                    while (char.IsDigit(x[xp]))
                        xp++;
                    while (char.IsDigit(y[yp]))
                        yp++;
                }
                else
                {
                    var iDiff = string.Compare(x[xp].ToString(), y[yp].ToString(), StringComparison.Ordinal);
                    if (iDiff != 0)
                        return iDiff;
                    xp++;
                    yp++;
                }
            }
            if (yp < y.Length)
                return -1;
            return 0;
        }

        private static int StrToInt(string str, int startIndex = 0)
        {
            str = str.Substring(startIndex);
            var digitsEnd = 0;
            while (digitsEnd < str.Length && char.IsDigit(str[digitsEnd]))
                digitsEnd++;
            str = str.Substring(0, digitsEnd);
            return int.Parse(str);
        }
    }
}

