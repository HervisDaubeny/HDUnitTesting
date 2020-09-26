using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension method for HDAttributes
    /// </summary>
    public static class HDAttributesExtensions {

        /// <summary>
        /// Get parameters of this attribute
        /// </summary>
        /// <returns>Parameters in string array</returns>
        public static string[] GetParams(this HDParametersAttribute attribute) {
            return attribute.Parameters.Select(p => p.ToString()).ToArray();
        }

        /// <summary>
        /// Get parameters from this generic attribute
        /// </summary>
        /// <returns>Parameters in string array</returns>
        public static string[] GetParams(this HDGenericParametersAttribute attribute) {
            return attribute.Parameters.Select(p => p.ToString()).ToArray();
        }

        /// <summary>
        /// Get types from this generic attribute
        /// </summary>
        /// <returns>Types in string array</returns>
        public static string[] GetTypes(this HDGenericParametersAttribute attribute) {
            return attribute.Types.Select(t => t.ToString()).ToArray();
        }
    }
}
