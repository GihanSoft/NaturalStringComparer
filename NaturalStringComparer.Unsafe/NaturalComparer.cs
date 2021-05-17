// -----------------------------------------------------------------------
// <copyright file="NaturalComparer.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.String
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Natural Comparer.
    /// </summary>
    public class NaturalComparer : IComparer<string?>
    {
        private static readonly Lazy<NaturalComparer> LazyCurrentCulture = new(() => new NaturalComparer(StringComparison.CurrentCulture));
        private static readonly Lazy<NaturalComparer> LazyCurrentCultureIgnoreCase = new(() => new NaturalComparer(StringComparison.CurrentCultureIgnoreCase));
#if !NETSTANDARD1_0
        private static readonly Lazy<NaturalComparer> LazyInvariantCulture = new (() => new NaturalComparer(StringComparison.InvariantCulture));
        private static readonly Lazy<NaturalComparer> LazyInvariantCultureIgnoreCase = new (() => new NaturalComparer(StringComparison.InvariantCultureIgnoreCase));
#endif
        private static readonly Lazy<NaturalComparer> LazyOrdinal = new(() => new NaturalComparer(StringComparison.Ordinal));
        private static readonly Lazy<NaturalComparer> LazyOrdinalIgnoreCase = new(() => new NaturalComparer(StringComparison.OrdinalIgnoreCase));

        private readonly StringComparison stringComparison;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalComparer"/> class.
        /// </summary>
        /// <param name="stringComparison">string comparison mode.</param>
        public NaturalComparer(StringComparison stringComparison = StringComparison.Ordinal)
        {
            this.stringComparison = stringComparison;
        }

        /// <summary>
        /// Gets default Comparer that uses <see cref="StringComparison.CurrentCulture"/>.
        /// </summary>
        public static NaturalComparer CurrentCulture => LazyCurrentCulture.Value;

        /// <summary>
        /// Gets default Comparer that uses <see cref="StringComparison.CurrentCultureIgnoreCase"/>.
        /// </summary>
        public static NaturalComparer CurrentCultureIgnoreCase => LazyCurrentCultureIgnoreCase.Value;

#if !NETSTANDARD1_0
        /// <summary>
        /// Gets default Comparer that uses <see cref="StringComparison.InvariantCulture"/>.
        /// </summary>
        public static NaturalComparer InvariantCulture => LazyInvariantCulture.Value;

        /// <summary>
        /// Gets default Comparer that uses <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
        /// </summary>
        public static NaturalComparer InvariantCultureIgnoreCase => LazyInvariantCultureIgnoreCase.Value;
#endif
        /// <summary>
        /// Gets default Comparer that uses <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        public static NaturalComparer Ordinal => LazyOrdinal.Value;

        /// <summary>
        /// Gets default Comparer that uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
        /// </summary>
        public static NaturalComparer OrdinalIgnoreCase => LazyOrdinalIgnoreCase.Value;

        /// <summary>
        /// Compares 2 <see cref="string"/>.
        /// </summary>
        /// <param name="x">1st string.</param>
        /// <param name="y">2nd string.</param>
        /// <param name="stringComparison"><see cref="StringComparison"/> mode.</param>
        /// <returns>value that show comparison result.</returns>
        public static int Compare(string? x, string? y, StringComparison stringComparison)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            unsafe
            {
                fixed (char* xp = x, yp = y)
                {
                    var xPointer = xp;
                    var yPointer = yp;

                    while (*xPointer != 0)
                    {
                        if (*yPointer == 0)
                        {
                            return 1;
                        }

                        if (char.IsDigit(*xPointer) && char.IsDigit(*yPointer))
                        {
                            var xNum = GetNumber(&xPointer);
                            var yNum = GetNumber(&yPointer);
                            if (xNum != yNum)
                            {
                                return xNum > yNum ? 1 : -1;
                            }
                        }

                        if (*xPointer != *yPointer)
                        {
                            return string.Compare((*xPointer).ToString(), (*yPointer).ToString(), stringComparison);
                        }

                        yPointer++;
                        xPointer++;
                    }

                    return *yPointer == 0 ? 0 : -1;
                }
            }
        }

        /// <summary>
        /// Compares 2 <see cref="string"/>.
        /// </summary>
        /// <param name="x">1st string.</param>
        /// <param name="y">2nd string.</param>
        /// <returns>value that show comparison result.</returns>
        public int Compare(string? x, string? y) => Compare(x, y, stringComparison);

        private static unsafe int GetNumber(char** pointer)
        {
            var number = 0;
            while (**pointer != 0 && char.IsDigit(**pointer))
            {
                number = (10 * number) + Convert.ToByte((char)(**pointer - 48));
                (*pointer)++;
            }
            return number;
        }
    }
}
