using System;
using System.Collections.Generic;

namespace Gihan.Helpers.String
{
    public class NaturalStringComparer : IComparer<string>
    {
        private static NaturalStringComparer _default;
        public static NaturalStringComparer Default => _default ?? (_default = new NaturalStringComparer());

        private static int StrToInt(string str, int startIndex = 0)
        {
            str = str.Substring(startIndex);
            var digitsEnd = 0;
            while (digitsEnd < str.Length && char.IsDigit(str[digitsEnd]))
                digitsEnd++;
            str = str.Substring(0, digitsEnd);
            return int.Parse(str);
        }

        public int Compare(string x, string y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (string.IsNullOrEmpty(x)) throw new ArgumentException("Argument is Empty", nameof(x));
            if (string.IsNullOrEmpty(y)) throw new ArgumentException("Argument is Empty", nameof(y));

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

                    while (xp < x.Length && char.IsDigit(x[xp]))
                        xp++;
                    while (yp < y.Length && char.IsDigit(y[yp]))
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
    }

    /// <summary>
    /// Compare any type based on result of their ToString() objects;
    /// </summary>
    /// <typeparam name="T">
    /// it can be any type
    /// </typeparam>
    public class NaturalStringComparer<T> : IComparer<T>
    {
        private static NaturalStringComparer<T> _default;
        public static NaturalStringComparer<T> Default => _default ?? (_default = new NaturalStringComparer<T>());

        public int Compare(T x, T y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            return NaturalStringComparer.Default.Compare(x.ToString(), y.ToString());
        }
    }
}