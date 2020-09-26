using HDUnit.Assert;
using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomersProjectTests.Nested {

    [HDTestClass]
    [HDClassConstructor()]
    public class NamespaceTests {

        [HDTestMethod]
        public static void OriginalTestName() {
            HDAssert.IsTrue(true);
        }
    }
}
