using HDUnit.Assert;
using HDUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomersProjectTests {

    [HDTestClass]
    [HDClassConstructor("for testing Assert class, by paranoid customer")]
    public class AssertTests {

        public string Purpouse { get; set; }

        public AssertTests(string Purpouse) {
            this.Purpouse = Purpouse;
        }

        [HDTestMethod]
        [HDParameters("paranoid customer")]
        [HDParameters("nonexistent substring")]
        public void StringContainsTest(string toTest) {
            HDAssert.Contains(toTest, Purpouse);
        }

        [HDTestMethod]
        [HDParameters(new int[] { 1,2,3,4,5,6,7,8,9})]
        [HDParameters(new int[] { 1,3,5,7,9})]
        public void AllInCollectionTest(int[] collection) {
            HDAssert.All(collection, item => { var i = item / (item % 2); });
        }

        [HDTestMethod]
        [HDParameters(new int[] { 1, 2, 3, 5, 6, 8, 9, 0 })]
        [HDParameters(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
        [HDParameters(new int[] { 9, 2, 3, 4, 5, 6, 7, 8, 1 })]
        public static void CollectionsEqual(int[] testArray) {
            int[] referenceArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            HDAssert.Equal<int>(referenceArray, testArray);
        }
    }
}
