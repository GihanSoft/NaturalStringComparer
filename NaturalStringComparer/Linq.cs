using Gihan.Helpers.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gihan.Helpers.Linq
{
    public static class Linq
    {
        public static void NaturalSort<T>(this List<T> src)
            => src.Sort(NaturalStringComparer<T>.Default);

        public static IOrderedEnumerable<TSource> NaturalOrderBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
                => source.OrderBy(keySelector, NaturalStringComparer<TKey>.Default);

        public static IOrderedEnumerable<TSource> NaturalOrderByDescending<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) 
                => source.OrderByDescending(keySelector, NaturalStringComparer<TKey>.Default);

        public static IOrderedEnumerable<TSource> NaturalThenBy<TSource, TKey>
            (this IOrderedEnumerable<TSource> sources, Func<TSource, TKey> keySelector)
                => sources.ThenBy(keySelector, NaturalStringComparer<TKey>.Default);

        public static IOrderedEnumerable<TSource> NaturalThenByDescending<TSource, TKey>
            (this IOrderedEnumerable<TSource> sources, Func<TSource, TKey> keySelector)
                => sources.ThenByDescending(keySelector, NaturalStringComparer<TKey>.Default);
    }
}
