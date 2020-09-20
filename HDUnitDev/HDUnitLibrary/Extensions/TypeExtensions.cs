using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HDUnit.Extensions {
    public static class TypeExtensions {
        
        public static object CreateInstance(this Type Class) {
            // for generic class
            if (Class.GetCustomAttribute<HDGenericClassConstructorAttribute>(inherit: false)
                is HDGenericClassConstructorAttribute genCtor) {
                Type[] genericTypes = genCtor.Types;
                Type genericClassType = Class.MakeGenericType(genericTypes);
                return Activator.CreateInstance(genericClassType, genCtor.Parameters);
            }
            // for default class
            if (Class.GetCustomAttribute<HDClassConstructorAttribute>(inherit: false)
                is HDClassConstructorAttribute ctor) {
                return Activator.CreateInstance(Class, ctor.ConstructorParameters);
            }

            // for static class
            return Class.GetType();
        }
    }
}
