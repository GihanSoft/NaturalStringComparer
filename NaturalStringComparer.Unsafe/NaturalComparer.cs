// -----------------------------------------------------------------------
// <copyright file="NaturalComparer.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.String;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Natural Comparer.
/// </summary>
public class NaturalComparer : IComparer<string?>
{
    private static readonly Lazy<NaturalComparer> LazyCurrentCulture = new(() => new NaturalComparer(StringComparison.CurrentCulture));
    private static readonly Lazy<NaturalComparer> LazyCurrentCultureIgnoreCase = new(() => new NaturalComparer(StringComparison.CurrentCultureIgnoreCase));
#if !NETSTANDARD1_1
    private static readonly Lazy<NaturalComparer> LazyInvariantCulture = new(() => new NaturalComparer(StringComparison.InvariantCulture));
    private static readonly Lazy<NaturalComparer> LazyInvariantCultureIgnoreCase = new(() => new NaturalComparer(StringComparison.InvariantCultureIgnoreCase));
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

#if !NETSTANDARD1_1

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
        if (ReferenceEquals(x, y)) { return 0; }

        if (x is null) { return -1; }

        if (y is null) { return 1; }

        var xSpan = x.AsSpan();
        var ySpan = y.AsSpan();

        while (xSpan.Length > 0 && ySpan.Length > 0)
        {
            ref var x0 = ref MemoryMarshal.GetReference(xSpan);
            ref var y0 = ref MemoryMarshal.GetReference(ySpan);
            if (char.IsDigit(x0) && char.IsDigit(y0))
            {
                var xNum = GetNumber(ref xSpan);
                var yNum = GetNumber(ref ySpan);

                var comp = xNum.CompareTo(yNum);
                if (comp != 0) { return comp; }

                continue;
            }

            if (x0 != y0)
            {
                return MemoryExtensions.CompareTo(xSpan.Slice(0, 1), ySpan.Slice(0, 1), stringComparison);
            }

            xSpan = xSpan.Slice(1);
            ySpan = ySpan.Slice(1);
        }

        return x.Length - y.Length;
    }

    /// <summary>
    /// Compares 2 <see cref="string"/>.
    /// </summary>
    /// <param name="x">1st string.</param>
    /// <param name="y">2nd string.</param>
    /// <returns>value that show comparison result.</returns>
    public int Compare(string? x, string? y) => Compare(x, y, this.stringComparison);

    private static int GetNumber(ref ReadOnlySpan<char> span)
    {
        var l = 0;
        while (l < span.Length && char.IsDigit(span[l])) { l++; }

#if NETSTANDARD1_1
        var num = int.Parse(span.Slice(0, l).ToString(), NumberStyles.None);
#else
        var num = int.Parse(span.Slice(0, l), NumberStyles.None);
#endif

        span = span.Slice(l);
        return num;
    }
}