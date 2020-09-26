using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute designating this method as TestMethod for HDTester
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HDTestMethodAttribute : HDRootAttribute {

        /// <summary>
        /// User given name of the test, used for listing results
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Attribute designating this method as TestMethod for HDUnitTests
        /// </summary>
        public HDTestMethodAttribute() { }

        /// <summary>
        /// Attribute designating this method as TestMethod for HDUnitTests using custom name
        /// </summary>
        /// <param name="MethodName">User given name of the test, used for listing results</param>
        public HDTestMethodAttribute(string MethodName) {
            this.MethodName = MethodName;
        }
    }
}
