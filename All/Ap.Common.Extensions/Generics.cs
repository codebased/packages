using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ap.Common.Extensions
{
    public static class Generics
    {
        public static void IsNullThrowInvalidOperationException<T>(this T source, string message) where T : class
        {
            if (source == null)
                throw new InvalidOperationException(message);
        }

        public static bool IsNotNull<T>(this T source) where T : class
        {
            return source != null;
        }

        public static bool IsNullOrEmpty<T>(this T source) where T : class
        {
            return !source.IsNotNullOrEmpty();
        }

        public static bool IsNotNullOrEmpty<T>(this T source) where T : class
        {

            if (source.IsNotNull())
            {
                if (source is IEnumerable<T>)
                {
                    return (source as IEnumerable<T>).Any();
                }

                return true;
            }

            return false;
        }
    }
}
