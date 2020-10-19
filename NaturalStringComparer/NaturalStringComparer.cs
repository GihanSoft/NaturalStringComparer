using System;
using System.Collections.Generic;

namespace GihanSoft
{
    public class NaturalStringComparer : IComparer<string>
    {
        private static NaturalStringComparer _default;
        public static NaturalStringComparer Default => _default ?? (_default = new NaturalStringComparer());

        private static (int num, bool ended) GetNumber(CharEnumerator enumer)
        {
            var str = "";
            var ended = false;
            if (!char.IsDigit(enumer.Current))
                return (-1, ended);
            while (true)
            {
                str += enumer.Current;
                ended = !enumer.MoveNext();
                if (ended || !char.IsDigit(enumer.Current))
                    break;
            }
            return (int.Parse(str), ended);
        }

        public static int CompareXY(string x, string y)
        {
            if (x.Length == 0)
                if (y.Length == 0)
                    return 0;
                else
                    return -1;
            if (y.Length == 0)
                return 1;
            if (x is null) throw new ArgumentNullException(nameof(x));
            if (y is null) throw new ArgumentNullException(nameof(y));

            var xEnumer = x.GetEnumerator();
            var yEnumer = y.GetEnumerator();

            while (true)
            {
                var xEnded = !xEnumer.MoveNext();
                var yEnded = !yEnumer.MoveNext();

                if (xEnded)
                    if (yEnded)
                        return 0;
                    else
                        return -1;
                if (yEnded)
                    return 1;

                if (char.IsDigit(xEnumer.Current) && char.IsDigit(yEnumer.Current))
                {
                    int xNum, yNum;
                    (xNum, xEnded) = GetNumber(xEnumer);
                    (yNum, yEnded) = GetNumber(yEnumer);
                    if (xNum != yNum)
                        return xNum.CompareTo(yNum);
                }

                if (xEnded)
                    if (yEnded)
                        return 0;
                    else
                        return -1;
                if (yEnded)
                    return 1;

                if (xEnumer.Current != yEnumer.Current)
                    return xEnumer.Current.CompareTo(yEnumer.Current);
            }
        }

        public int Compare(string x, string y) => CompareXY(x, y);
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

        public static int CompareXY(T x, T y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            return NaturalStringComparer.Default.Compare(x.ToString(), y.ToString());
        }

        public int Compare(T x, T y) => CompareXY(x, y);
    }
}