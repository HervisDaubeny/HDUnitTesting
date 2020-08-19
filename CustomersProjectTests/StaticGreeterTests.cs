using System;
using HDUnit;
using CustomersNamespace;

namespace CustomersProjectTests {
    [HDTestClass]
    class StaticGreeterTests {
        public bool CheckMessage() {
            if (StaticGreeter.Message == "statitc hello") {
                return true;
            }

            return false;
        }

    }
}
