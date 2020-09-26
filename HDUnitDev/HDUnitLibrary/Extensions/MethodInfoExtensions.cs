using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HDUnit.Extensions {


    /// <summary>
    /// Extension method for MethodInfo
    /// </summary>
    public static class MethodInfoExtensions {

        /// <summary>
        /// Get custom name of the test method or null.
        /// </summary>
        /// <returns>Null or user given name of method</returns>
        public static string GetCustomName(this MethodInfo Method) {
            if (Method.GetCustomAttribute<HDTestMethodAttribute>(inherit: false) is HDTestMethodAttribute testMethod) {
                if (testMethod.MethodName is object) {
                    return testMethod.MethodName;
                }
            }

            return null;
        }

        /// <summary>
        /// Verify if this TestProcess contains given method.
        /// </summary>
        /// <param name="MethodName">Name of method to check</param>
        /// <returns>True if method is contained, False otherwise.</returns>
        public static bool ContainsMethod(this TestProcess Process, string MethodName) {
            return Process.TestMethods.Select(m => m.Name).Contains(MethodName);
        }

        /// <summary>
        /// Get name of the method this method has to be run after.
        /// </summary>
        /// <returns>Null or name of the method</returns>
        public static string GetDependency(this MethodInfo Method) {
            if (Method.GetCustomAttribute<HDRunAfterAttribute>(inherit: false) is HDRunAfterAttribute runAfter) {
                return runAfter.MethodName;
            }

            return null;
        }
    }
}
