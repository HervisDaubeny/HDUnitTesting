using HDUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HDTestClassAttribute : HDRootAttribute {

    }
}
