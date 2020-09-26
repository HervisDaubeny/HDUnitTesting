using HDUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute designating this class as TestClass for HDTester
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HDTestClassAttribute : HDRootAttribute {

    }
}
