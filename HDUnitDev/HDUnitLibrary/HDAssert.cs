using HDUnit.Exceptions;
using HDUnit.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDUnit.Assert {
    public static class HDAssert {

        #region All
        /// <summary>
        /// Verifies that all items in the collection pass when executed against action.
        /// </summary>
        /// <typeparam name="T">The type of the object to be verified</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="action">The action to test each item against</param>
        public static void All<T>(IEnumerable<T> collection, Action<T> action) {
            int index = 0;
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
            foreach (var item in collection) {
                try {
                    action(item);
                }
                catch (Exception ex) {
                    errors.Add($"{nameof(collection)}[{index}]", ex.Message);
                }
                index++;
            }

            if (errors.Any()) {
                throw new HDAssertFailedException("Following items failed when executed against action:", errors);
            }
        }
        #endregion

        #region Cast
        /// <summary>
        /// Verifies that an object can be casted to the given type. 
        /// </summary>
        /// <typeparam name="T">The type the object should be casted to</typeparam>
        /// <param name="object">The object to be evaluated</param>
        /// <returns>The object, casted to type T when successful</returns>
        public static T Cast<T>(object @object) {
            if (!(@object is T casted)) {
                throw new HDAssertFailedException($"'{@object}' cannot be casted to type '{typeof(T)}'");
            }
            return casted;
        }
        #endregion

        #region Contains
        /// <summary>
        /// Verifies that a string contains a given sub-string, using the current culture. 
        /// </summary>
        /// <param name="expectedSubstring">The sub-string expected to be in the string</param>
        /// <param name="actualString">The string to be inspected</param>
        public static void Contains(string expectedSubstring, string actualString) {
            if (!(actualString.Contains(expectedSubstring))) {
                throw new HDAssertFailedException($"The inspected string doesn't contain '{expectedSubstring}'.");
            }
        }

        /// <summary>
        /// Verifies that a collection contains a given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be verified</typeparam>
        /// <param name="collection">The collection to be inspected</param>
        /// <param name="filter">The filter used to find the item you're ensuring the collection contains</param>
        public static void Contains<T>(IEnumerable<T> collection, Predicate<T> filter) {
            bool contains = default(bool);
            foreach (var item in collection) {
                if (filter(item)) {
                    contains = true;
                    break;
                }
            }

            if (!contains) {
                throw new HDAssertFailedException("The collection does not contain expected object.");
            }
        }

        /// <summary>
        /// Verifies that a collection contains a given object, using an equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the object to be verified</typeparam>
        /// <param name="expected">The object expected to be in the collection</param>
        /// <param name="collection">The collection to be inspected</param>
        /// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
        public static void Contains<T>(T expected, IEnumerable<T> collection, IEqualityComparer<T> comparer) {
            bool equals = default(bool);
            foreach (var item in collection) {
                if (comparer.Equals(item, expected)) {
                    equals = true;
                    break;
                }
            }

            if (!equals) {
                throw new HDAssertFailedException($"The object '{expected}' is not a part of the collection.");
            }
        }

        /// <summary>
        /// Verifies that a collection contains a given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be verified</typeparam>
        /// <param name="expected">The object expected to be in the collection</param>
        /// <param name="collection">The collection to be inspected</param>
        public static void Contains<T>(T expected, IEnumerable<T> collection) {
            if (!collection.Contains(expected)) {
                throw new HDAssertFailedException($"The collection does not contain the item '{expected}'.");
            }
        }

        /// <summary>
        /// Verifies that a string contains a given sub-string, using the given comparison type.
        /// </summary>
        /// <param name="expectedSubstring">The sub-string expected to be in the string</param>
        /// <param name="actualString">The string to be inspected</param>
        /// <param name="comparisonType">The type of string comparison to perform</param>
        public static void Contains(string expectedSubstring, string actualString, StringComparison comparisonType) {
            if(!(actualString.Contains(expectedSubstring, comparisonType))) {
                throw new HDAssertFailedException($"'{actualString}' does not contain '{expectedSubstring}'.");
            }
        }
        #endregion

        #region DoesNotContain
        /// <summary>
        /// Verifies that a collection does not contain a given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be compared</typeparam>
        /// <param name="collection">The collection to be inspected</param>
        /// <param name="filter">The filter used to find the item you're ensuring the collection does not contain</param>
        public static void DoesNotContain<T>(IEnumerable<T> collection, Predicate<T> filter) {
            foreach (var item in collection) {
                if (filter(item)) {
                    throw new HDAssertFailedException($"The collection does contain item {item}");
                }
            }
        }

        /// <summary>
        /// Verifies that a dictionary does not contain a given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
        /// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
        /// <param name="expected">The object expected not to be in the collection.</param>
        /// <param name="collection">The collection to be inspected.</param>
        public static void DoesNotContain<TKey, TValue>(TKey expected, IReadOnlyDictionary<TKey, TValue> collection) {
            if (collection.Keys.Contains(expected)) {
                throw new HDAssertFailedException($"The collection does contain key {expected}");
            }
        }

        /// <summary>
        /// Verifies that a dictionary does not contain a given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
        /// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
        /// <param name="expected">The object expected not to be in the collection.</param>
        /// <param name="collection">The collection to be inspected.</param>
        public static void DoesNotContain<TKey, TValue>(TKey expected, IDictionary<TKey, TValue> collection) {
            if (collection.Keys.Contains(expected)) {
                throw new HDAssertFailedException($"The collection does contain key {expected}");
            }
        }

        /// <summary>
        /// Verifies that a string does not contain a given sub-string, using the current culture.
        /// </summary>
        /// <param name="expectedSubstring">The sub-string which is expected not to be in the string</param>
        /// <param name="actualString">The string to be inspected</param>
        public static void DoesNotContain(string expectedSubstring, string actualString) {
            if (actualString.Contains(expectedSubstring)) {
                throw new HDAssertFailedException($"'{actualString}' does contain '{expectedSubstring}'.");
            }
        }

        /// <summary>
        /// Verifies that a string does not contain a given sub-string, using the given comparison type.
        /// </summary>
        /// <param name="expectedSubstring">The sub-string which is expected not to be in the string</param>
        /// <param name="actualString">The string to be inspected</param>
        /// <param name="comparisonType">The type of string comparison to perform</param>
        public static void DoesNotContain(string expectedSubstring, string actualString, StringComparison comparisonType) {
            if (actualString.Contains(expectedSubstring, comparisonType)) {
                throw new HDAssertFailedException($"'{actualString}' contains '{expectedSubstring}'.");
            }
        }

        /// <summary>
        /// Verifies that a collection does not contain a given object, using an equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the object to be compared</typeparam>
        /// <param name="expected">The object that is expected not to be in the collection</param>
        /// <param name="collection">The collection to be inspected</param>
        /// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
        public static void DoesNotContain<T>(T expected, IEnumerable<T> collection, IEqualityComparer<T> comparer) {
            foreach (var item in collection) {
                if (comparer.Equals(expected, item)) {
                    throw new HDAssertFailedException($"The collection contains item '{expected}'.");
                }
            }
        }

        /// <summary>
        /// Verifies that a collection does not contain a given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be compared</typeparam>
        /// <param name="expected">The object that is expected not to be in the collection</param>
        /// <param name="collection">The collection to be inspected</param>
        public static void DoesNotContain<T>(T expected, IEnumerable<T> collection) {
            if (collection.Contains(expected)) {
                throw new HDAssertFailedException($"The collection contains item '{expected}'.");
            }
        }
        #endregion

        #region Empty
        /// <summary>
        /// Verifies that a collection is empty. 
        /// </summary>
        /// <param name="collection">The collection to be inspected</param>
        public static void Empty(IEnumerable collection) {
            var enumerator = collection.GetEnumerator();
            try {
                if (enumerator.MoveNext()) {
                    throw new HDAssertFailedException("The collection is not empty.");
                }
            }
            finally {
                (enumerator as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Verifies that a collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of the collection to be examined</typeparam>
        /// <param name="collection">The collection to be inspected</param>
        public static void Empty<T>(IEnumerable<T> collection) {
            if (collection.Any()) {
                throw new HDAssertFailedException("The collection is not empty.");
            }
        }
        #endregion

        #region Equal
        /// <summary>
        /// Verifies that two objects are equal, using a default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared with</param>
        public static void Equal<T>(T expected, T actual) =>
            HDAssert.Equal<T>(expected, actual, $"'{actual}' is not equal to '{expected}'");

        /// <summary>
        /// Verifies that two objects are equal, using a default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="message">String used for custom exception message</param>
        public static void Equal<T>(T expected, T actual, string message) {
            if (!expected.Equals(actual)) {
                throw new HDAssertFailedException(message);
            }
        }

        /// <summary>
        /// Verifies that two objects are equal, using a custom equatable comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="comparer">The comparer used to compare the two objects</param>
        public static void Equal<T>(T expected, T actual, IEqualityComparer<T> comparer) =>
            HDAssert.Equal(expected, actual, comparer, $"'{actual}' is not equal to '{expected}'");

        /// <summary>
        /// Verifies that two objects are equal, using a custom equatable comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="comparer">The comparer used to compare the two objects</param>
        /// <param name="message">String used for custom exception message</param>
        public static void Equal<T>(T expected, T actual, IEqualityComparer<T> comparer, string message) {
            if (!comparer.Equals(expected, actual)) {
                throw new HDAssertFailedException(message);
            }
        }

        /// <summary>
        /// Verifies that two System.DateTime values are equal, within the precision given by precision.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="precision">The allowed difference in time where the two dates are considered equal</param>
        public static void Equal(DateTime expected, DateTime actual, TimeSpan precision) =>
            HDAssert.Equal(expected, actual, precision, $"'{actual}' is not equal to '{expected}'");

        /// <summary>
        /// Verifies that two System.DateTime values are equal, within the precision given by precision.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="precision">The allowed difference in time where the two dates are considered equal</param>
        /// <param name="message">String used for custom exception message</param>
        public static void Equal(DateTime expected, DateTime actual, TimeSpan precision, string message) {
            if (expected > actual && expected - actual > precision) {
                throw new HDAssertFailedException(message);
            }
            if (expected < actual && actual - expected > precision) {
                throw new HDAssertFailedException(message);
            }
            // equality is automatically passed
        }

        /// <summary>
        /// Verifies that two sequences are equivalent, using a custom equatable comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="comparer">The comparer used to compare the two objects</param>
        /// <exception cref="ArgumentNullException">Argument is null</exception>
        public static void Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer) {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            var enum1 = expected.GetEnumerator();
            var enum2 = actual.GetEnumerator();

            var moving1 = enum1.MoveNext();
            var moving2 = enum2.MoveNext();

            int index = 0;
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

            while (moving1 && moving2) {
                if (!comparer.Equals(enum1.Current, enum2.Current)) {
                    errors.Add($"Equality assertion [{index}]",
                        $"Object '{enum1.Current}' was expected but '{enum2.Current}' was found");
                }
                index++;
                moving1 = enum1.MoveNext();
                moving2 = enum2.MoveNext();
            }

            if (moving1 != moving2) {
                throw new HDAssertFailedException(
                    $"Length '{expected.Count()}' was expected but length '{actual.Count()}' was found", errors);
            }
            if (errors.Any()) {
                throw new HDAssertFailedException(
                    $"Some inequalities found during evaluation", errors);
            }
        }

        /// <summary>
        /// Verifies that two sequences are equivalent, using a default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="comparer">The comparer used to compare the two objects</param>
        /// <exception cref="ArgumentNullException">Argument is null</exception>
        public static void Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual) {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));

            var enum1 = expected.GetEnumerator();
            var enum2 = actual.GetEnumerator();

            var moving1 = enum1.MoveNext();
            var moving2 = enum2.MoveNext();

            int index = 0;
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

            while (moving1 && moving2) {
                if (!object.Equals(enum1.Current, enum2.Current)) {
                    errors.Add($"Equality assertion [{index}]",
                        $"Object '{enum1.Current}' was expected but '{enum2.Current}'");
                }
                index++;
                moving1 = enum1.MoveNext();
                moving2 = enum2.MoveNext();
            }

            if (moving1 != moving2) {
                throw new HDAssertFailedException(
                    $"Length '{expected.Count()}' was expected but length '{actual.Count()}' was found", errors);
            }
            if (errors.Any()) {
                throw new HDAssertFailedException(
                    $"Some inequalities found during evaluation", errors);
            }
        }

        /// <summary>
        /// Verifies that two strings are equivalent.
        /// </summary>
        /// <param name="expected">The expected string value</param>
        /// <param name="actual">The actual string value</param>
        /// <param name="ignoreCase">If set to true, ignores case differences. The invariant culture is used</param>
        /// <param name="ignoreLineEndingDifferences">If set to true, treats \r\n, \r, and \n as equivalent</param>
        /// <param name="ignoreWhiteSpaceDifferences">If set to true, treats spaces, tabs, \r\n, \r, and \n (in any non-zero quantity) as equivalent</param>
        public static void Equal(
            string expected,
            string actual,
            bool ignoreCase = false,
            bool ignoreLineEndingDifferences = false,
            bool ignoreWhiteSpaceDifferences = false) {
            string expCopy = (ignoreCase) ? expected.ToLower() : expected;
            string actCopy = (ignoreCase) ? actual.ToLower() : actual;

            IEnumerable<string> expBlocks = null;
            IEnumerable<string> actBlocks = null;

            if (ignoreLineEndingDifferences && !ignoreWhiteSpaceDifferences) {
                expBlocks = expCopy.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                actBlocks = actCopy.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            }
            else if (ignoreWhiteSpaceDifferences) {
                expBlocks = expCopy.Split(new[] { "\r\n", "\r", "\n", " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                actBlocks = actCopy.Split(new[] { "\r\n", "\r", "\n", " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            }

            HDAssert.Equal(expBlocks, actBlocks);
        }

        /// <summary>
        /// Verifies that two strings are equivalent
        /// </summary>
        /// <param name="expected">The expected string value</param>
        /// <param name="actual">The actual string value</param>
        public static void Equal(string expected, string actual) =>
            HDAssert.Equal(expected, actual, false, false, false);
        #endregion

        #region IsFalse
        /// <summary>
        /// Verifies that given boolean is false.
        /// </summary>
        /// <param name="expected">The boolean expected to be false</param>
        public static void IsFalse(bool expected) {
            if (expected is true) {
                throw new HDAssertFailedException("Given boolean is true.");
            }
        }

        /// <summary>
        /// Verifies that given boolean is false.
        /// </summary>
        /// <param name="expected">Boolean expected to be false</param>
        /// <param name="message">String used for custom exception message</param>
        public static void IsFalse(bool expected, string message) {
            if (expected is true) {
                throw new HDAssertFailedException(message);
            }
        }

        /// <summary>
        /// Verifies that running a predicate on an instance results in false.
        /// </summary>
        /// <typeparam name="T">Type the predicate expects</typeparam>
        /// <param name="t">Instance of type t</param>
        /// <param name="expected">The predicate to evaluate the instance with</param>
        /// <param name="message">String used for custom exception message</param>
        public static void IsFalse<T>(this T t, Predicate<T> expected, string message) {
            if (expected(t) is false) {
                throw new HDAssertFailedException(message);
            }
        }
        #endregion

        #region IsNotNull
        /// <summary>
        /// Verifies that the given object is not null.
        /// </summary>
        /// <param name="object">Object to be verified</param>
        public static void IsNotNull(object @object) {
            if (!(@object is object)) {
                throw new HDAssertFailedException("Given object is null.");
            }
        }

        /// <summary>
        /// Verifies that the given object is not null.
        /// </summary>
        /// <param name="object">Object to be verified</param>
        /// <param name="message">String used for custom exception message</param>
        public static void IsNotNull(object @object, string message) {
            if (!(@object is object)) {
                throw new HDAssertFailedException(message);
            }
        }

        /// <summary>
        /// Verifies that running a predicate on an instance does not result in null.
        /// </summary>
        /// <typeparam name="T">Type the predicate expects</typeparam>
        /// <param name="t">Instance of type t</param>
        /// <param name="expected">The predicate to evaluate the instance with</param>
        /// <param name="message">String used for custom exception message</param>
        public static void IsNotNull<T>(this T t, Predicate<T> expected, string message) {
            if (!(expected(t) is object)) {
                throw new HDAssertFailedException(message);
            }
        }
        #endregion

        #region IsNull
        /// <summary>
        /// Verifies that the given object is not null.
        /// </summary>
        /// <param name="object">Object to be verified</param>
        public static void IsNull(object @object) {
            if (@object is object) {
                throw new HDAssertFailedException("Given object is not null.");
            }
        }

        /// <summary>
        /// Verifies that the given object is not null.
        /// </summary>
        /// <param name="object">The object to be examined</param>
        /// <param name="message">String used for custom exception message</param>
        public static void IsNull(object @object, string message) {
            if (@object is object) {
                throw new HDAssertFailedException(message);
            }
        }

        /// <summary>
        /// Verifies that running a predicate on an instance does result in null.
        /// </summary>
        /// <typeparam name="T">Type the predicate expects</typeparam>
        /// <param name="t">Instance of type t</param>
        /// <param name="expected">The predicate to evaluate the instance with</param>
        /// <param name="message">String used for custom exception message</param>
        public static void IsNull<T>(this T t, Predicate<T> expected, string message) {
            if (expected(t) is object) {
                throw new HDAssertFailedException(message);
            }
        }
        #endregion

        #region IsTrue
        /// <summary>
        /// Verifies that given boolean is true.
        /// </summary>
        /// <param name="expected">Boolean expected to be true</param>
        public static void IsTrue(bool expected) {
            if (expected == false) {
                throw new HDAssertFailedException("Given boolean id false.");
            }
        }

        /// <summary>
        /// Verifies that given boolean is true.
        /// </summary>
        /// <param name="expected">Boolean expected to be true</param>
        /// <param name="message">String used for custom exception message</param>
        public static void IsTrue(bool expected, string message) {
            if (expected is false) {
                throw new HDAssertFailedException(message);
            }
        }

        /// <summary>
        /// Verifies that running a predicate on an instance does result in true.
        /// </summary>
        /// <typeparam name="T">Type the the predicate expects</typeparam>
        /// <param name="t">Instance of type t</param>
        /// <param name="expected">The predicate to evaluate the instance with</param>
        /// <param name="message">String used for custom exception message</param>
        public static void IsTrue<T>(this T t, Predicate<T> expected, string message) {
            if (expected(t) is false) {
                throw new HDAssertFailedException(message);
            }
        }
        #endregion

        #region InRange
        /// <summary>
        /// Verifies that a value is within a given range.
        /// </summary>
        /// <typeparam name="T">The type of the value to be compared</typeparam>
        /// <param name="actual">The actual value to be evaluated</param>
        /// <param name="low">The (inclusive) low value of the range</param>
        /// <param name="high">The (inclusive) high value of the range</param>
        public static void InRange<T>(T actual, T low, T high) where T : IComparable {
            if (actual.CompareTo(low) < 0) {
                throw new HDAssertFailedException($"'{actual}' comes before '{low}'");
            }
            if (actual.CompareTo(high) > 0) {
                throw new HDAssertFailedException($"'{actual}' comes after '{high}'");
            }
        }

        /// <summary>
        /// Verifies that a value is within a given range, using a comparer.
        /// </summary>
        /// <typeparam name="T">The type of the value to be compared</typeparam>
        /// <param name="actual">The actual value to be evaluated</param>
        /// <param name="low">The (inclusive) low value of the range</param>
        /// <param name="high">The (inclusive) high value of the range</param>
        /// <param name="comparer">The comparer used to evaluate the value's range</param>
        public static void InRange<T>(T actual, T low, T high, IComparer<T> comparer) {
            if (comparer.Compare(actual, low) < 0) {
                throw new HDAssertFailedException($"'{actual}' comes before '{low}'");
            }
            if (comparer.Compare(actual, high) > 0) {
                throw new HDAssertFailedException($"'{actual}' comes after '{high}'");
            }
        }
        #endregion

        #region IsNotType
        /// <summary>
        /// Verifies that an object is not exactly the given type. 
        /// </summary>
        /// <param name="expectedType">The type the object should not be</param>
        /// <param name="object">The object to be evaluated</param>
        public static void IsNotType(Type expectedType, object @object) {
            if (@object.GetType() == expectedType) {
                throw new HDAssertFailedException($"'{@object}' is of type '{expectedType}'");
            }
        }

        /// <summary>
        /// Verifies that an object is not exactly the given type.
        /// </summary>
        /// <typeparam name="T">The type the object should not be</typeparam>
        /// <param name="object">The object to be evaluated</param>
        public static void IsNotType<T>(object @object) {
            if (@object.GetType() == typeof(T)) {
                throw new HDAssertFailedException($"'{@object}' is of type '{typeof(T)}'");
            }
        }
        #endregion

        #region IsType
        /// <summary>
        /// Verifies that an object is exactly the given type (and not a derived type). 
        /// </summary>
        /// <typeparam name="T">The type the object should be</typeparam>
        /// <param name="object">The object to be evaluated</param>
        /// <returns>The object, casted to type T when successful</returns>
        public static T IsType<T>(object @object) {
            if (@object.GetType() == typeof(T)) {
                throw new HDAssertFailedException($"'{@object}' is not of type '{typeof(T)}'");
            }
            return (T)@object;
        }

        /// <summary>
        /// Verifies that an object is exactly the given type (and not a derived type). 
        /// </summary>
        /// <param name="expectedType">The type the object should be</param>
        /// <param name="object">The object to be evaluated</param>
        public static void IsType(Type expectedType, object @object) {
            if (@object.GetType() != expectedType) {
                throw new HDAssertFailedException($"'{@object}' is not of type '{expectedType}'");
            }
        }
        #endregion

        #region NotEmpty
        /// <summary>
        /// Verifies that a collection is not empty. 
        /// </summary>
        /// <param name="collection">The collection to be inspected</param>
        public static void NotEmpty(IEnumerable collection) {
            var enumerator = collection.GetEnumerator();
            if (!(enumerator.MoveNext())) {
                throw new HDAssertFailedException("The collection is empty.");
            }
        }
        #endregion

        #region NotEqual
        /// <summary>
        /// Verifies that two objects are not equal, using a default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="message">String used for custom exception message</param>
        public static void NotEqual<T>(T expected, T actual, string message) {
            if (object.Equals(expected, actual)) {
                throw new HDAssertFailedException(message);
            }
        }

        /// <summary>
        /// Verifies that two objects are not equal, using a default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The value to be compared with</param>
        public static void NotEqual<T>(T expected, T actual) =>
            HDAssert.NotEqual<T>(expected, actual, $"'{expected}' is equal to '{actual}'");

        /// <summary>
        /// Verifies that two objects are not equal, using a custom equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="comparer">The comparer used to examine the objects</param>
        public static void NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer)
            => NotEqual(expected, actual, comparer, $"'{expected}' is equal to '{actual}'");

        /// <summary>
        /// Verifies that two objects are not equal, using a custom equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The value to be compared with</param>
        /// <param name="comparer">The comparer used to examine the objects</param>
        /// <param name="message">String used for custom exception message</param>
        public static void NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer, string message) {
            if (comparer.Equals(expected, actual)) {
                throw new HDAssertFailedException(message);
            }
        }


        /// <summary>
        /// Verifies that all items of collections <paramref name="expected"/> and <paramref name="actual"/> are not equal
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected collection</param>
        /// <param name="actual">The collection to compare with</param>
        public static void NotEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual) {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));

            var enum1 = expected.GetEnumerator();
            var enum2 = actual.GetEnumerator();

            var moving1 = enum1.MoveNext();
            var moving2 = enum2.MoveNext();

            int index = 0;
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

            while (moving1 && moving2) {
                if (object.Equals(enum1.Current, enum2.Current)) {
                    errors.Add($"Equality assertion [{index}]",
                        $"Object '{enum1.Current}' and '{enum2.Current}' should not be equal.");
                }
                index++;
                moving1 = enum1.MoveNext();
                moving2 = enum2.MoveNext();
            }

            if (moving1 != moving2) {
                throw new HDAssertFailedException(
                    $"Length '{expected.Count()}' was expected but length '{actual.Count()}' was found", errors);
            }
            if (errors.Any()) {
                throw new HDAssertFailedException(
                    $"Some inequalities found during evaluation", errors);
            }
        }

        /// <summary>
        /// Verifies that all items of collections <paramref name="expected"/> nad <paramref name="actual"/> are not equal
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared</typeparam>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        public static void NotEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer) {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            var enum1 = expected.GetEnumerator();
            var enum2 = actual.GetEnumerator();

            var moving1 = enum1.MoveNext();
            var moving2 = enum2.MoveNext();

            int index = 0;
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

            while (moving1 && moving2) {
                if (comparer.Equals(enum1.Current, enum2.Current)) {
                    errors.Add($"Equality assertion [{index}]",
                        $"Object '{enum1.Current}' and '{enum2.Current}' should not be equal.");
                }
                index++;
                moving1 = enum1.MoveNext();
                moving2 = enum2.MoveNext();
            }

            if (moving1 != moving2) {
                throw new HDAssertFailedException(
                    $"Length '{expected.Count()}' was expected but length '{actual.Count()}' was found", errors);
            }
            if (errors.Any()) {
                throw new HDAssertFailedException(
                    $"Some inequalities found during evaluation", errors);
            }
        }


        #endregion

        #region Throws
        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type),
        /// where the exception derives from System.ArgumentException and has the given parameter name.
        /// </summary>
        /// <typeparam name="T">Type of the expected exception</typeparam>
        /// <param name="paramName">The parameter name that is expected to be in the exception</param>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        public static T Throws<T>(string paramName, Func<object> testCode) where T : ArgumentException {
            T exception = null;
            try {
                testCode();
            }
            catch (T expected) {
                if (expected.ParamName != paramName) {
                    throw new HDAssertFailedException($"The exception was not caused by'{paramName}' but '{expected.ParamName}'.", expected);
                }
                exception = expected;
            }
            catch (Exception un) {
                throw new HDAssertFailedException("Unexpected exception has occurred.", un);
            }

            return exception;
        }

        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type),
        /// where the exception derives from System.ArgumentException and has the given parameter name.
        /// </summary>
        /// <typeparam name="T">Type of the expected exception</typeparam>
        /// <param name="paramName">The parameter name that is expected to be in the exception</param>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns></returns>
        public static T Throws<T>(string paramName, Action testCode) where T : ArgumentException {
            T exception = null;
            try {
                testCode();
            }
            catch (T expected) {
                if (expected.ParamName != paramName) {
                    throw new HDAssertFailedException($"The exception was not caused by'{paramName}' but '{expected.ParamName}'.", expected);
                }
                exception = expected;
            }
            catch (Exception un) {
                throw new HDAssertFailedException("Unexpected exception has occurred.", un);
            }

            return exception;
        }

        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type). 
        /// </summary>
        /// <param name="exceptionType">The type of the exception expected to be thrown</param>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        public static Exception Throws(Type exceptionType, Action testCode) {
            Exception exception = null;
            try {
                testCode();
            }
            catch (Exception expected) {
                if (expected.GetType() == exceptionType) {
                    exception = expected;
                }
                else {
                    throw new HDAssertFailedException($"The exception was of type'{expected.GetType()}'.", expected);
                }
            }

            return exception;
        }

        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type).
        /// </summary>
        /// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        public static T Throws<T>(Action testCode) where T : Exception {
            T exception = null;
            try {
                testCode();
            }
            catch (T expected) {
                exception = expected;
            }
            catch (Exception un) {
                throw new HDAssertFailedException("Unexpected exception has occurred.", un);
            }

            return exception;
        }
        #endregion

        #region ThrowsAny
        /// <summary>
        /// Verifies that the exact exception or a derived exception type is thrown. 
        /// </summary>
        /// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        public static T ThrowsAny<T>(Action testCode) where T : Exception {
            T exception = null;
            try {
                testCode();
            }
            catch (Exception expected) {
                if (expected is T) {
                    exception = (T)expected;
                }
                else {
                    throw new HDAssertFailedException($"Exception is of type '{expected.GetType()}'.", expected);
                }
            }

            return exception;
        }

        /// <summary>
        /// Verifies that the exact exception or a derived exception type is thrown. Generally
        /// used to test property accessors. 
        /// </summary>
        /// <typeparam name="T">The type of the exception expected to be thrown</typeparam>
        /// <param name="testCode">A delegate to the code to be tested</param>
        /// <returns>The exception that was thrown, when successful</returns>
        public static T ThrowsAny<T>(Func<object> testCode) where T : Exception {
            T exception = null;
            try {
                testCode();
            }
            catch (Exception expected) {
                if (expected is T) {
                    exception = (T) expected;
                }
                else {
                    throw new HDAssertFailedException($"Exception is of type '{expected.GetType()}'.", expected);
                }
            }

            return exception;
        }
        #endregion
    }
}
