#pragma warning disable

#if !NET7_0_OR_GREATER

using System.Linq;
using System.Text;

namespace System.Collections.Generic;

internal static class IEnumerableExtensions
{
    public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source) => source.OrderBy(item => item);

    public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source, IComparer<T> comparer) => source.OrderBy(item => item, comparer);
}

#endif
