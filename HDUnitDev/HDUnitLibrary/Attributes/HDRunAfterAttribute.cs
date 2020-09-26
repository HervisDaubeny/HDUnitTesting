using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute allowing user to control flow of the testing.
    /// Requires valid name of existing TestMethod within the same TestClass as this method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HDRunAfterAttribute : HDRootAttribute {

        /// <summary>
        /// Name of TestMethod that must be run before this method
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Attribute allowing user to control flow of the testing.
        /// Requires valid name of existing TestMethod within the same TestClass as this method.
        /// </summary>
        /// <param name="MethodName">Name of TestMethod that must be run before this method</param>
        public HDRunAfterAttribute(string MethodName) {
            this.MethodName = MethodName;
        }
    }
}
