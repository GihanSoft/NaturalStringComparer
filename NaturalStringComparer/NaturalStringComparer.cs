using System;
using System.Collections.Generic;

namespace Gihan.Helpers.StringHelper
{
    public class NaturalStringComparer : IComparer<string>
    {
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
                    var iDiff = StringComparer.OrdinalIgnoreCase.Compare(x[xp], y[yp]);
                    if (iDiff != 0)
                        return iDiff;
                }
            }
            if (yp < y.Length)
                return -1;
            return 0;
        }


        private static int StrToInt(string Str, int startIndex = 0)
        {
            Str = Str.Substring(startIndex);
            var DigitsEnd = 0;
            while (char.IsDigit(Str[DigitsEnd]))
                DigitsEnd++;
            Str = Str.Substring(0, DigitsEnd);
            return int.Parse(Str);
        }
    }
}

