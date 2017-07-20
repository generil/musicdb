using System;
using System.Collections.Generic;
using System.Linq;

namespace Music.Helper
{
    public static class IsNullOrEmptyExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source != null)
            {
                return !source.Any();
            }
            return true;
        }
    }
}