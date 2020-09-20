using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit {
    public static class HDResultPrinter {
        private static string separator = "==========================================";

        public static void Print(TestResultContainer[] TestResults) {
            Console.WriteLine(separator);
            Console.WriteLine("TEST RESULTS");
            Console.WriteLine(separator);
            foreach (var result in TestResults) {
                Console.WriteLine(result);
            }
        }
    }
}
