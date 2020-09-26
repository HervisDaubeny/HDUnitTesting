using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute for passing parameters to a TestMethod.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HDParametersAttribute : HDRootAttribute {

        /// <summary>
        /// Parameters for calling the method
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// Attribute for passing parameters to a TestMethod.
        /// </summary>
        /// <param name="Parameters">Parameters for calling the method</param>
        public HDParametersAttribute(params object[] Parameters) {
            this.Parameters = Parameters;
        }
    }
}
