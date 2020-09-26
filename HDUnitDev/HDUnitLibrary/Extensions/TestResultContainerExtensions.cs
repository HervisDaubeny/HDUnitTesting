using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension methods of TestResultContainer.
    /// </summary>
    public static class TestResultContainerExtensions {

        /// <summary>
        /// Concatenate collection of <c>TestResultContainer[]</c> to single array of <c>TestResultContainer</c>
        /// </summary>
        /// <param name="resultContainers">The collection of <c>TestResultContainer[]</c></param>
        /// <returns>Array of <c>TestResultContainer</c></returns>
        public static TestResultContainer[] Concat(this IEnumerable<TestResultContainer[]> resultContainers) {
            List<TestResultContainer> testResults = new List<TestResultContainer>();
            foreach (var container in resultContainers) {
                foreach (var result in container) {
                    testResults.Add(result);
                }
            }

            return testResults.ToArray();
        }
    }
}
