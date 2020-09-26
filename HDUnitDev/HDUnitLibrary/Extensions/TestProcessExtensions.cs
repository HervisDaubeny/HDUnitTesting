using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension methods for TestProcess
    /// </summary>
    public static class TestProcessExtensions {

        /// <summary>
        /// Get name of the class from this TestProcess.
        /// </summary>
        /// <returns>Name of the class</returns>
        public static string GetClassName(this TestProcess testProcess) {
            return testProcess.ClassName;
        }
        
        /// <summary>
        /// Get array of <c>TestResultContainer</c> from this TestProcess.
        /// </summary>
        /// <returns>Array of test results</returns>
        public static TestResultContainer[] GetResultContainersArray(this IEnumerable<TestProcess> testProcesses) {
            return testProcesses.Select(tp => tp.TestResult).Concat();
        }
    }
}
