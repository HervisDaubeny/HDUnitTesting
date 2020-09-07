using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Attributes {

    /// <summary>
    /// Attribute for constructing generic methods.
    /// </summary>
    /// <example><code lang="cs">
    ///     [HDTestMethod]
    ///     [HDGenericParameters([int, string], [0, "priklad", 42])]
    ///     [HDGenericParameters([string, string], ["druhy", "priklad", 47])]    
    ///     MyTestMethod &lt;T, U&gt; (T t, U u, int i) {
    ///         ...
    ///     }
    /// </code></example>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HDGenericParametersAttribute : HDRootAttribute {

        public Type[] Types { get; set; }
        public object[] Parameters { get; set; }

        public HDGenericParametersAttribute(Type[] Types, object[] Parameters) {
            this.Types = Types;
            this.Parameters = Parameters;
        }
    }
}
