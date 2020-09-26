using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension methods of <c>IEnumerable&lt;Type&gt;</c>.
    /// </summary>
    public static class TypeColectionsExtensions {

        /// <summary>
        /// Return names of all Types in the collection
        /// </summary>
        /// <param name="Classes">The collection to be examined</param>
        /// <returns>Array with names of all Types from collection</returns>
        public static string[] Names(this IEnumerable<Type> Classes) {
            return Classes.Select(c => c.Name).ToArray();
        }

        /// <summary>
        /// Return union of all Namespaces of given collection of Types
        /// </summary>
        /// <param name="Classes">The collection to be examined</param>
        /// <returns>Array with names of all Namespaces given Types have</returns>
        public static string[] Namespaces(this IEnumerable<Type> Classes) {
            return Classes.Select(c => c.Namespace).ToArray();
        }
    }
}
