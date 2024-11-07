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
public sealed class NaturalComparer : IComparer<string?>, IComparer<ReadOnlyMemory<char>>
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
                var xOut = GetNumber(x.Slice(i), out var xNumAsSpan);
                var yOut = GetNumber(y.Slice(i), out var yNumAsSpan);

                int compareResult;

                if (IsUlong(xNumAsSpan) && IsUlong(yNumAsSpan))
                {
#if NETSTANDARD2_1_OR_GREATER || NET8_0_OR_GREATER
                    var xNum = ulong.Parse(xNumAsSpan);
                    var yNum = ulong.Parse(yNumAsSpan);
#else
                    var xNum = ulong.Parse(xNumAsSpan.ToString());
                    var yNum = ulong.Parse(yNumAsSpan.ToString());
#endif
                    compareResult = xNum.CompareTo(yNum);
                }
                else
                {
#if NETSTANDARD2_1_OR_GREATER || NET8_0_OR_GREATER
                    var xNum = BigInteger.Parse(xNumAsSpan);
                    var yNum = BigInteger.Parse(yNumAsSpan);
#else
                    var xNum = BigInteger.Parse(xNumAsSpan.ToString());
                    var yNum = BigInteger.Parse(yNumAsSpan.ToString());
#endif
                    compareResult = xNum.CompareTo(yNum);
                }

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

                x = xOut;
                y = yOut;
                continue;
            }

            if (xCh != yCh)
            {
                return x.Slice(i, 1).CompareTo(y.Slice(i, 1), stringComparison);
            }
        }

        return x.Length.CompareTo(y.Length);
    }

    private static bool IsUlong(ReadOnlySpan<char> number)
    {
        while (number.Length > 0 && number[0] == '0')
        {
            number = number.Slice(1);
        }

        // 18446744073709551615
        return number switch {
            { Length: <= 19 } => true,
            { Length: > 20 } => false,
            ['1', < '8', ..] => true,
            ['1', '8', < '4', ..] => true,
            ['1', '8', '4', < '4', ..] => true,
            ['1', '8', '4', '4', < '6', ..] => true,
            ['1', '8', '4', '4', '6', < '7', ..] => true,
            ['1', '8', '4', '4', '6', '7', < '4', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', < '4', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', < '7', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', < '3', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', '3', < '7', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', '3', '7', '0', < '9', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', '3', '7', '0', '9', < '5', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', '3', '7', '0', '9', '5', < '5', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', '3', '7', '0', '9', '5', '5', '0', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', '3', '7', '0', '9', '5', '5', '1', < '6', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', '3', '7', '0', '9', '5', '5', '1', '6', '0', ..] => true,
            ['1', '8', '4', '4', '6', '7', '4', '4', '0', '7', '3', '7', '0', '9', '5', '5', '1', '6', '1', <= '5'] => true,
            _ => false
        };
    }

    private static ReadOnlySpan<char> GetNumber(ReadOnlySpan<char> span, out ReadOnlySpan<char> number)
    {
        var i = 0;
        while (i < span.Length && char.IsDigit(span[i]))
        {
            i++;
        }

        number = span.Slice(0, i);
        return span.Slice(i);
    }
}
