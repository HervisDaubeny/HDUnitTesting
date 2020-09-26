using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension methods of <IEnumerable>Type</IEnumerable>
    /// </summary>
    public static class TypeColectionsExtensions {

        public static string[] Names(this IEnumerable<Type> Classes) {
            return Classes.Select(c => c.Name).ToArray();
        }

        public static string[] Namespaces(this IEnumerable<Type> Classes) {
            return Classes.Select(c => c.Namespace).ToArray();
        }
    }
}
