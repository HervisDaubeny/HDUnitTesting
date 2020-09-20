using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CustomersNamespace;
using HDUnit.Attributes;
using HDUnit.Assert;

namespace CustomersProjectTests {

    [HDTestClass]
    [HDClassConstructor()]
    public class GenericGreeterTests {

        [HDTestMethod]
        [HDRunAfter("DependancyTest")]
        [HDGenericParameters(new Type[] { typeof(string), typeof(string) }, new object[] { "hello", "generic", 42})]
        [HDGenericParameters(new Type[] { typeof(string), typeof(string) }, new object[] { "generic", "hello", 47})]
        public void GenericNotnullTestingMethod<T, U> (T t, U u, int num) {
            var genGreet = new Greeter();
            genGreet.Greet<T, U>(t, u, num);

            HDAssert.IsTrue(genGreet is object);
        }

        [HDTestMethod]
        [HDRunAfter("IsNum42Test")]
        [HDGenericParameters(new Type[] { typeof(string)}, new object[] { "yo" })]
        public void DependancyTest<T>(T t) {
            HDAssert.IsTrue(true);
        }

        [HDTestMethod("ValueTest")]
        [HDGenericParameters(new Type[] { typeof(string), typeof(string) }, new object[] { "hello", "generic", 42 })]
        [HDGenericParameters(new Type[] { typeof(string), typeof(string) }, new object[] { "generic", "hello", 47 })]
        public void IsNum42Test<T, U>(T t, U u, int num) {
            var genGreet = new Greeter();
            genGreet.Greet<T, U>(t, u, num);

            //HDAssert.IsTrue(Equals(num, 42));
            num.IsTrue(x => x == 42, "It is not.");
        }
    }
}
