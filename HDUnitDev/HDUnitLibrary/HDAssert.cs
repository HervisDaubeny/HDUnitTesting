using HDUnit.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Assert {
    public static class HDAssert {

        #region IsTrue
        public static void IsTrue(bool Result) {
            if (Result == false) {
                throw new HDAssertFailedException("FAILED");
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
        #endregion

        #region IsFalse
        public static void IsFalse(bool Result) {
            if (Result is true) {
                throw new HDAssertFailedException("FAILED");
            }
        }

        public static void IsFalse(bool Result, string Message) {
            if (Result is true) {
                throw new HDAssertFailedException(Message);
            }
        }

        public static void IsFalse<T>(this T t, Predicate<T> Result, string Message) {
            if (Result(t) is false) {
                throw new HDAssertFailedException(Message);
            }
        }
        #endregion

        #region IsNull
        public static void IsNull(object Value) {
            if (Value is object) {
                throw new HDAssertFailedException("FAILED");
            }
        }
        public static void IsNull(object Value, string Message) {
            if (Value is object) {
                throw new HDAssertFailedException(Message);
            }
        }
        public static void IsNull<T>(this T t, Predicate<T> Result, string Message) {
            if (Result(t) is object) {
                throw new HDAssertFailedException(Message);
            }
        }
        #endregion
    }
}
