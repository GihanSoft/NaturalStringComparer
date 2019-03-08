using System;
using System.Collections.Generic;

namespace Gihan.Helpers.String
{
    public class NaturalStringComparer : IComparer<string>
    {
        private static NaturalStringComparer _default;
        public static NaturalStringComparer Default => _default ?? (_default = new NaturalStringComparer());

        public int Compare(string x, string y)
        {
            if (string.IsNullOrEmpty(x)) throw new ArgumentException("Argument is Empty", nameof(x));
            if (string.IsNullOrEmpty(y)) throw new ArgumentException("Argument is Empty", nameof(y));

            var xEnumer = x.GetEnumerator();
            var yEnumer = y.GetEnumerator();
            var xNotExit = xEnumer.MoveNext();
            var yNotExit = yEnumer.MoveNext();

            while (xNotExit && yNotExit)
            {
                if (char.IsNumber(xEnumer.Current) && char.IsNumber(yEnumer.Current))
                {
                    var xNumStr = "" + xEnumer.Current;
                    var yNumStr = "" + yEnumer.Current;
                    xNotExit = xEnumer.MoveNext();
                    yNotExit = yEnumer.MoveNext();

                    while (xNotExit && char.IsDigit(xEnumer.Current))
                    {
                        xNumStr += xEnumer.Current;
                        xNotExit = xEnumer.MoveNext();
                    }
                    while (yNotExit && char.IsDigit(yEnumer.Current))
                    {
                        yNumStr += yEnumer.Current;
                        yNotExit = yEnumer.MoveNext();
                    }

                    var xNum = double.Parse(xNumStr);
                    var yNum = double.Parse(yNumStr);

                    if (xNum != yNum) return xNum.CompareTo(yNum);
                    else if (!xNotExit && !yNotExit)
                        return 0;
                }
                if (xEnumer.Current != yEnumer.Current)
                    return xEnumer.Current.CompareTo(yEnumer.Current);
                xNotExit = xEnumer.MoveNext();
                yNotExit = yEnumer.MoveNext();
            }
            if (xNotExit)
                return 1;
            else if (yNotExit)
                return -1;
            else
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