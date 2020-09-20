using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute allowing user to control flow of the testing.
    /// Requires valid name of existing TestMethod from the same TestClass.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HDRunAfterAttribute : HDRootAttribute {

        public string MethodName { get; set; }

        public HDRunAfterAttribute(string MethodName) {
            this.MethodName = MethodName;
        }
    }
}
