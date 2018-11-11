using Gihan.Helpers.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gihan.Helpers.Linq
{
    public static class Linq
    {
        public static void NaturalSort<T>(this List<T> src)
        {
            src.Sort(NaturalStringComparer<T>.Default);
        }

        public static IOrderedEnumerable<TSource> NaturalOrderBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.OrderBy(keySelector, NaturalStringComparer<TKey>.Default);
        }

        static IOrderedEnumerable<TSource> NaturalOrderByDescending<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.OrderByDescending(keySelector, NaturalStringComparer<TKey>.Default);
        }
    }
}
