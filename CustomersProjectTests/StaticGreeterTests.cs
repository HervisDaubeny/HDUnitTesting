using System;
using HDUnit.Attributes;
using CustomersNamespace;
using HDUnit.Assert;

namespace CustomersProjectTests {
    [HDTestClass]
    public static class StaticGreeterTests {

        [HDTestMethod]
        public static void CheckMessage() {
            HDAssert.IsTrue(StaticGreeter.Message == "static hello");
        }
    }
}
