// -----------------------------------------------------------------------
// <copyright file="NaturalComparer.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.String;

using System;
using System.Collections.Generic;

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
            if (char.IsDigit(x[i]) && char.IsDigit(y[i]))
            {
                var xOut = GetNumber(x.Slice(i), out var xNumAsSpan);
                var yOut = GetNumber(y.Slice(i), out var yNumAsSpan);

                var compareResult = CompareNumValues(xNumAsSpan, yNumAsSpan);

                if (compareResult != 0)
                {
                    return compareResult;
                }

                i = -1;
                length = Math.Min(xOut.Length, yOut.Length);

                x = xOut;
                y = yOut;
                continue;
            }

            var charCompareResult = x.Slice(i, 1).CompareTo(y.Slice(i, 1), stringComparison);
            if (charCompareResult != 0)
            {
                return charCompareResult;
            }
        }

        return x.Length.CompareTo(y.Length);
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

    private static int CompareNumValues(ReadOnlySpan<char> numValue1, ReadOnlySpan<char> numValue2)
    {
        var num1AsSpan = TrimZero(numValue1);
        var num2AsSpan = TrimZero(numValue2);

        if (num1AsSpan.Length < num2AsSpan.Length)
        {
            return -1;
        }

        if (num1AsSpan.Length > num2AsSpan.Length)
        {
            return 1;
        }

        var compareResult = num1AsSpan.CompareTo(num2AsSpan, StringComparison.Ordinal);

        if (compareResult != 0)
        {
            return Math.Sign(compareResult);
        }

        if (numValue2.Length == numValue1.Length)
        {
            return compareResult;
        }

        return numValue2.Length < numValue1.Length ? -1 : 1; // "033" < "33" === true
    }

    private static ReadOnlySpan<char> TrimZero(ReadOnlySpan<char> numValue)
    {
        while (numValue.Length > 0 && numValue[0] == '0')
        {
            numValue = numValue.Slice(1);
        }

        return numValue;
    }
}
