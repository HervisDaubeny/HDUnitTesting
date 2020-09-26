using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension methods for Type.
    /// </summary>
    public static class TypeExtensions {
        
        /// <summary>
        /// Return new instance of given Type as object.
        /// </summary>
        /// <param name="Class">Type of desired object</param>
        /// <returns></returns>
        public static object CreateInstance(this Type Class) {
            // for instance class
            if (Class.GetCustomAttribute<HDClassConstructorAttribute>(inherit: false)
                is HDClassConstructorAttribute ctor) {
                return Activator.CreateInstance(Class, ctor.ConstructorParameters);
            }

            // for static class
            return Class.GetType();
        }
    }
}
