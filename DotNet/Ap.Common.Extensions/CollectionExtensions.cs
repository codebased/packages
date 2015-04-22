using System;
using System.Collections.Generic;
using System.Linq;

namespace Ap.Common.Extensions
{
    /// <summary>
    /// Defines extension methods for collection type
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Add a collection of objects.
        /// </summary>
        /// <typeparam name="T">ICollection of any type.</typeparam>
        /// <param name="collection">Object of type ICollection</param>
        /// <param name="list">List of Objects to be inserted.</param>
        /// <returns>updated Collection object with new objects</returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> list)
        {
            var enumerable = list as IList<T> ?? new List<T>();
            if (!enumerable.Any())
            {
                return collection;
            }

            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            //// @ there is no point of converting a collection to list and then add one by one, thus it will eat more memory... 
            //// I could only find the way to concat one by one. even default Concat is returning me of type IEnumerable<T> 
            //// that cannot be used here...
            foreach (var item in enumerable)
            {
                collection.Add(item);
            }

            return collection;
        }

        /// <summary>
        ///   Searches for an element that matches the conditions defined by the specified
        ///    predicate, and returns the first occurrence within the entire System.Collections.Generic.List<T/>.
        /// </summary>
        /// <typeparam name="T">ICollection of any type.</typeparam>
        /// <param name="collection">Object of type ICollection</param>
        /// <param name="predicate"> Represents the method that defines a set of criteria and determines whetherthe specified object meets those criteria.</param>
        /// <returns> The first element that matches the conditions defined by the specified predicate,if found; otherwise, the default value for type T.</returns>
        public static T Find<T>(this ICollection<T> collection, Predicate<T> predicate)
        {
            return collection.ToList().Find(predicate);
        }

        /// <summary>
        ///  Retrieves all the elements that match the conditions defined by the specified
        /// predicate.
        /// </summary>
        /// <typeparam name="T">ICollection of any type.</typeparam>
        /// <param name="collection">Object of type ICollection</param>
        /// <param name="predicate"> Represents the method that defines a set of criteria and determines whetherthe specified object meets those criteria.</param>
        /// <returns> A System.Collections.Generic.List<T/> containing all the elements that matchthe conditions defined by the specified predicate, if found; otherwise, anempty System.Collections.Generic.List<T/>..</returns>
        public static ICollection<T> FindAll<T>(this ICollection<T> collection, Predicate<T> predicate)
        {
            return collection.ToList().FindAll(predicate);
        }

        /// <summary>
        ///  Searches for an element that matches the conditions defined by the specified
        ///     predicate, and returns the zero-based index of the first occurrence within
        ///    the entire System.Collections.Generic.List<T/>.
        /// </summary>
        /// <typeparam name="T">ICollection of any type.</typeparam>
        /// <param name="collection">Object of type ICollection</param>
        /// <param name="predicate"> Represents the method that defines a set of criteria and determines whetherthe specified object meets those criteria.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        public static int FindIndex<T>(this ICollection<T> collection, Predicate<T> predicate)
        {
            return FindIndex(collection, 0, predicate);
        }

        /// <summary>
        ///  Searches for an element that matches the conditions defined by the specified
        ///     predicate, and returns the zero-based index of the first occurrence within
        ///    the entire System.Collections.Generic.List<T/>.
        /// </summary>
        /// <typeparam name="T">ICollection of any type.</typeparam>
        /// <param name="collection">Object of type ICollection</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="predicate"> Represents the method that defines a set of criteria and determines whetherthe specified object meets those criteria.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        public static int FindIndex<T>(this ICollection<T> collection, int startIndex, Predicate<T> predicate)
        {
            return FindIndex(collection, startIndex, collection.Count, predicate);
        }

        /// <summary>
        ///  Searches for an element that matches the conditions defined by the specified
        ///     predicate, and returns the zero-based index of the first occurrence within
        ///    the entire System.Collections.Generic.List<T/>.
        /// </summary>
        /// <typeparam name="T">ICollection of any type.</typeparam>
        /// <param name="collection">Object of type ICollection</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="predicate"> Represents the method that defines a set of criteria and determines whetherthe specified object meets those criteria.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        public static int FindIndex<T>(this ICollection<T> collection, int startIndex, int count, Predicate<T> predicate)
        {
            return collection.ToList().FindIndex(predicate);
        }

        /// <summary>
        /// Performs the specified action on each element of the System.Collections.Generic.List<T/>.
        /// </summary>
        /// <typeparam name="T">ICollection of any type.</typeparam>
        /// <param name="collection">Object of type ICollection</param>
        /// <param name="action">The System.Action<T/> delegate to perform on each element of the System.Collections.Generic.List<T/>.</param>
        public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            collection.ToList().ForEach(action);
        }
    }
}
