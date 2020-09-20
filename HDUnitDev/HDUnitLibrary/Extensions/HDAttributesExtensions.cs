using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDUnit.Extensions {
    public static class HDAttributesExtensions {

        public static string[] GetParams(this HDParametersAttribute attribute) {
            return attribute.Parameters.Select(p => p.ToString()).ToArray();
        }

        public static string[] GetParams(this HDGenericParametersAttribute attribute) {
            return attribute.Parameters.Select(p => p.ToString()).ToArray();
        }

        public static string[] GetTypes(this HDGenericParametersAttribute attribute) {
            return attribute.Types.Select(t => t.ToString()).ToArray();
        }
    }
}
