using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute for providing types and parameters to a generic class constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HDGenericClassConstructorAttribute : HDRootAttribute {

        public Type[] Types { get; set; }
        public object[] Parameters { get; set; }

        public HDGenericClassConstructorAttribute(Type[] Types, params object[] Parameters) {
            this.Types = Types;
            this.Parameters = Parameters;
        }
    }
}
