using System;
using System.Collections.Generic;
using System.Linq;

namespace Bobasoft
{
    public static class LinqExtensions
    {
        //======================================================
        #region _Public methods_

        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dict,
                                     Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            foreach (var cur in dict.Where(condition).ToList())
                dict.Remove(cur.Key);
        }

        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> condition)
        {
            if (collection.IsReadOnly)
                throw new InvalidOperationException("Cannot perform operation on read only collection");

            foreach (var item in collection.Where(condition).ToList())
                collection.Remove(item);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
        }

#if !SILVERLIGHT
        public static IEnumerable<T> WhereOr<T, T1>(this IEnumerable<T> collection, Func<T, T1, bool> condition, params T1[] range)
        {
            var predicate = PredicateBuilder.False<T>();

            foreach (var o in range)
            {
                var o1 = o;
                predicate = predicate.Or(obj => condition(obj, o1));
            }

            return collection.Where(predicate.Compile());
        }

        public static IEnumerable<T> WhereAnd<T, T1>(this IEnumerable<T> collection, Func<T, T1, bool> condition, params T1[] range)
        {
            var predicate = PredicateBuilder.True<T>();

            foreach (var o in range)
            {
                var o1 = o;
                predicate = predicate.And(obj => condition(obj, o1));
            }

            return collection.Where(predicate.Compile());
        }
#endif

        #endregion
    }
}