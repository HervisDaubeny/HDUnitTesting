using HDUnit.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Assert {
    public static class HDAssert {

        public static void IsTrue(bool Result) {
            if (Result == false) {
                throw new HDAssertFailedException("Test failed.");
            }
        }

        public static void IsTrue(bool Result, string Message) {
            if (Result is false) {
                throw new HDAssertFailedException(Message);
            }
        }

        public static void IsTrue<T>(this T t, Predicate<T> Result, string Message) {
            if (Result(t) is false) {
                throw new HDAssertFailedException(Message);
            }
        }
    }
}
