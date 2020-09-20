using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDUnit.Extensions {
    public static class TestProcessExtensions {

        public static string GetClassName(this TestProcess testProcess) {
            return testProcess.GetType().ToString();
        }
        
        public static TestResultContainer[] GetResultContainersArray(this IEnumerable<TestProcess> testProcesses) {
            return testProcesses.Select(tp => tp.TestResult).Concat();
        }
    }
}
