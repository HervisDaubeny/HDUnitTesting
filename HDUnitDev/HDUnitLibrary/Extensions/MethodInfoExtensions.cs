using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HDUnit.Extensions {
    public static class MethodInfoExtensions {

        /// <summary>
        /// enables custom naming of a method -> will display in test results
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetName(this MethodInfo info) {
            if (info.GetCustomAttribute<HDTestMethodAttribute>() is HDTestMethodAttribute attribute) {
                if (attribute.MethodName is object) {
                    return attribute.MethodName;
                }
            }

            return info.Name;
        }
    }
}
