using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute for providing parameters to class constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HDClassConstructorAttribute : HDRootAttribute {

        /// <summary>
        /// Collection of objects to be passed to class constructor.
        /// </summary>
        public object[] ConstructorParameters { get; set; }

        /// <summary>
        /// Attribute for providing parameters to class constructor.
        /// </summary>
        /// <param name="ConstructorParameters">Collection of objects to be passed to class constructor</param>
        public HDClassConstructorAttribute(params object[] ConstructorParameters) {
            this.ConstructorParameters = ConstructorParameters;
        }
    }
}
