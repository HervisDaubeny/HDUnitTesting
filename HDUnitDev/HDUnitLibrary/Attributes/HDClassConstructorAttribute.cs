using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute for providing specific parameters to a constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HDClassConstructorAttribute : HDRootAttribute {

        public object[] ConstructorParameters { get; set; }

        public HDClassConstructorAttribute(params object[] ConstructorParameters) {
            this.ConstructorParameters = ConstructorParameters;
        }
    }
}
