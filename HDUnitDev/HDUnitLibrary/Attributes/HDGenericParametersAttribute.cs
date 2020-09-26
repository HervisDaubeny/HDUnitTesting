using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute for passing generic types and parameters to a generic TestMethod.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HDGenericParametersAttribute : HDRootAttribute {

        /// <summary>
        /// Types used for construction of a generic method
        /// </summary>
        public Type[] Types { get; set; }
        /// <summary>
        /// Parameters used for calling the resulting generic method
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// Attribute for passing generic and normal parameters to a generic TestMethod.
        /// </summary>
        /// <param name="Types">Types used for construction of a generic method</param>
        /// <param name="Parameters">Parameters used for calling the resulting generic method</param>
        public HDGenericParametersAttribute(Type[] Types, params object[] Parameters) {
            this.Types = Types;
            this.Parameters = Parameters;
        }
    }
}
