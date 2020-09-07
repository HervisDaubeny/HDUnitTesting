using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HDTestMethodAttribute : HDRootAttribute {

        public string MethodName { get; set; }

        public HDTestMethodAttribute() { }

        public HDTestMethodAttribute(string MethodName) {
            this.MethodName = MethodName;
        }
    }
}
