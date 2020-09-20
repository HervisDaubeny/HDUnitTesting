using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute for passing parameters to a TestMethod.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HDParametersAttribute : HDRootAttribute {

        public object[] Parameters { get; set; }

        public HDParametersAttribute(params object[] Parameters) {
            this.Parameters = Parameters;
        }
    }
}
