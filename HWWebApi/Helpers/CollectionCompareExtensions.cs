using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HWWebApi.Helpers
{
    public static class CollectionCompareExtensions
    {
        public static bool IsEquals<T>(this ICollection<T> first, ICollection<T> second, IEqualityComparer<T> equality = null)
        {
            return first.Count == second.Count &&
                   !first.Except(second, equality).Any() &&
                   !second.Except(first, equality).Any();
        }
    }
}
