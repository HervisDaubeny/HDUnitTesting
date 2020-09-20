using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HDUnit.Extensions {
    public static class MethodInfoExtensions {

        /// <summary>
        /// Get custom name of the test method or null. Used for printing test results.
        /// </summary>
        /// <param name="Method"></param>
        /// <returns></returns>
        public static string GetCustomName(this MethodInfo Method) {
            if (Method.GetCustomAttribute<HDTestMethodAttribute>(inherit: false) is HDTestMethodAttribute testMethod) {
                if (testMethod.MethodName is object) {
                    return testMethod.MethodName;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Process"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public static bool ContainsMethod(this TestProcess Process, string MethodName) {
            return Process.TestMethods.Select(m => m.Name).Contains(MethodName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Method"></param>
        /// <returns></returns>
        public static string GetDependency(this MethodInfo Method) {
            if (Method.GetCustomAttribute<HDRunAfterAttribute>(inherit: false) is HDRunAfterAttribute runAfter) {
                return runAfter.MethodName;
            }

            return null;
        }
    }
}
