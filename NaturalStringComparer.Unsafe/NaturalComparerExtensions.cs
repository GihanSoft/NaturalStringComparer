// -----------------------------------------------------------------------
// <copyright file="NaturalComparerExtensions.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.String;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// NaturalComparer Extensions.
/// </summary>
public static class NaturalComparerExtensions
{
    /// <summary>
    /// Naturally sort a list.
    /// </summary>
    /// <typeparam name="TSource">List type.</typeparam>
    /// <param name="src">list to sort.</param>
    /// <param name="keySelector">key selector that select string to use in natural comparison.</param>
    /// <param name="stringComparison">base string comparison mode.</param>
    public static void NaturalSort<TSource>(
        this List<TSource> src,
        Func<TSource, string?>? keySelector = null,
        StringComparison stringComparison = StringComparison.Ordinal)
    {
        keySelector ??= o => o?.ToString();
        src.Sort((x, y) =>
            NaturalComparer.Compare(keySelector(x), keySelector(y), stringComparison));
    }

    /// <summary>
    /// Naturally sort a list.
    /// </summary>
    /// <typeparam name="TSource">List type.</typeparam>
    /// <param name="src">list to sort.</param>
    /// <param name="keySelector">key selector that select string to use in natural comparison.</param>
    /// <param name="stringComparison">base string comparison mode.</param>
    public static void NaturalSort<TSource>(
        this TSource[] src,
        Func<TSource, string?>? keySelector = null,
        StringComparison stringComparison = StringComparison.Ordinal)
    {
        keySelector ??= o => o?.ToString();
        Array.Sort(src, (x, y) =>
            NaturalComparer.Compare(keySelector(x), keySelector(y), stringComparison));
    }

    public static IOrderedEnumerable<TSource> NaturalOrderBy<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, string?>? keySelector = null,
        StringComparison stringComparison = StringComparison.Ordinal)
    {
        keySelector ??= o => o?.ToString();
        return source.OrderBy(keySelector, new NaturalComparer(stringComparison));
    }

    public static IOrderedEnumerable<TSource> NaturalOrderByDescending<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, string?>? keySelector = null,
        StringComparison stringComparison = StringComparison.Ordinal)
    {
        keySelector ??= o => o?.ToString();
        return source.OrderByDescending(keySelector, new NaturalComparer(stringComparison));
    }

    public static IOrderedEnumerable<TSource> NaturalThenBy<TSource>(
        this IOrderedEnumerable<TSource> source,
        Func<TSource, string?>? keySelector = null,
        StringComparison stringComparison = StringComparison.Ordinal)
    {
        keySelector ??= o => o?.ToString();
        return source.ThenBy(keySelector, new NaturalComparer(stringComparison));
    }

    public static IOrderedEnumerable<TSource> NaturalThenByDescending<TSource>(
        this IOrderedEnumerable<TSource> source,
        Func<TSource, string?>? keySelector = null,
        StringComparison stringComparison = StringComparison.Ordinal)
    {
        keySelector ??= o => o?.ToString();
        return source.ThenByDescending(keySelector, new NaturalComparer(stringComparison));
    }
}