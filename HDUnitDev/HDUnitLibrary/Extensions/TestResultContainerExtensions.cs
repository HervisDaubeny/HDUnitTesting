using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Extensions {
    public static class TestResultContainerExtensions {

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
