using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CustomersNamespace;
using HDUnit.Attributes;
using HDUnit.Assert;

namespace CustomersProjectTests {

    [HDTestClass]
    public class GenericGreeterTests {

        [HDTestMethod]
        [HDGenericParameters(new Type[] { typeof(string), typeof(string) }, new object[] { "hello", "generic", 42})]
        [HDGenericParameters(new Type[] { typeof(string), typeof(string) }, new object[] { "generic", "helo", 47})]
        public void GenericNotnullTestingMethod<T, U> (T t, U u, int num) {
            var genGreet = new GenericGreeter<T, U>(t, u);
            genGreet.Greet<T, U>(genGreet.TProp, genGreet.UProp, num);

            HDAssert.IsTrue(genGreet.TProp is object && genGreet.UProp is object);
        }

        [HDTestMethod("ValueTest")]
        [HDGenericParameters(new Type[] { typeof(string), typeof(string) }, new object[] { "hello", "generic", 42 })]
        [HDGenericParameters(new Type[] { typeof(string), typeof(string) }, new object[] { "generic", "helo", 47 })]
        public void IsNum42Test<T, U>(T t, U u, int num) {
            var genGreet = new GenericGreeter<T, U>(t, u);
            genGreet.Greet<T, U>(genGreet.TProp, genGreet.UProp, num);

            //HDAssert.IsTrue(Equals(num, 42));
            num.IsTrue(x => x == 42, "It is not.");
        }
    }
}
