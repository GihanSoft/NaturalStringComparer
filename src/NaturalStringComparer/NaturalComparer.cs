// -----------------------------------------------------------------------
// <copyright file="NaturalComparer.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.String;

using System;
using System.Collections.Generic;
using System.Numerics;

/// <summary>
/// Natural Comparer.
/// </summary>
public class NaturalComparer : IComparer<string?>, IComparer<ReadOnlyMemory<char>>
{
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
    /// Compares 2 <see cref="string"/>.
    /// </summary>
    /// <param name="x">1st string.</param>
    /// <param name="y">2nd string.</param>
    /// <returns>value that show comparison result.</returns>
    public int Compare(string? x, string? y)
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

        return Compare(x.AsSpan(), y.AsSpan(), this.stringComparison);
    }

    public int Compare(ReadOnlySpan<char> x, ReadOnlySpan<char> y)
    {
        return Compare(x, y, this.stringComparison);
    }

    public int Compare(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
    {
        return Compare(x.Span, y.Span, stringComparison);
    }

    /// <summary>
    /// Compares 2 <see cref="string"/>.
    /// </summary>
    /// <param name="x">1st string.</param>
    /// <param name="y">2nd string.</param>
    /// <param name="stringComparison"><see cref="StringComparison"/> mode.</param>
    /// <returns>value that show comparison result.</returns>
    public static int Compare(ReadOnlySpan<char> x, ReadOnlySpan<char> y, StringComparison stringComparison)
    {
        var length = Math.Min(x.Length, y.Length);

        for (var i = 0; i < length; i++)
        {
            var xCh = x[i];
            var yCh = y[i];

            if (char.IsDigit(xCh) && char.IsDigit(yCh))
            {
                var xOut = GetNumber(x.Slice(i), out var xNum);
                var yOut = GetNumber(y.Slice(i), out var yNum);

                var compareResult = xNum.CompareTo(yNum);
                if (compareResult != 0)
                {
                    return compareResult;
                }

                i = -1;
                length = Math.Min(xOut.Length, yOut.Length);
                if (length == 0 && xOut.Length == yOut.Length && x.Length != y.Length)
                {
                    return y.Length < x.Length ? -1 : 1; // "033" < "33" === true
                }
                else
                {
                    x = xOut;
                    y = yOut;
                    continue;
                }
            }

            if (xCh != yCh)
            {
                return x.Slice(i, 1).CompareTo(y.Slice(i, 1), stringComparison);
            }
        }

        return x.Length.CompareTo(y.Length);
    }

    private static ReadOnlySpan<char> GetNumber(ReadOnlySpan<char> span, out IComparable number)
    {
        var i = 0;
        while (i < span.Length && char.IsDigit(span[i]))
        {
            i++;
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var parseInput = span[..i];
        try
        {
            number = ulong.Parse(parseInput);
        }
        catch (OverflowException)
        {
            number = BigInteger.Parse(parseInput);
        }

        return span[i..];
#else
        var parsInput = span.Slice(0, i).ToString();
        try
        {
            number = ulong.Parse(parsInput);
        }
        catch (OverflowException)
        {
            number = BigInteger.Parse(parsInput);
        }

        return span.Slice(i);
#endif
    }
}