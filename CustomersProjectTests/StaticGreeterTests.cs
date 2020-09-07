using System;
using HDUnit.Attributes;
using CustomersNamespace;

namespace CustomersProjectTests {
    [HDTestClass]
    class StaticGreeterTests {

        [HDTestMethod]
        public bool CheckMessage() {
            if (StaticGreeter.Message == "statitc hello") {
                return true;
            }

            return false;
        }

    }
}
