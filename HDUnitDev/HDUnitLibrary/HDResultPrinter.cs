using HDUnit.Extensions;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace HDUnit {
    public static class HDResultPrinter {
        private static string separator = "==========================================";
        private static string noTests = "NO TESTS SATISFIING REQUEST FOUND".Pastel(Color.LightBlue);
        private static string testResults = "TEST RESULTS";
        private static string invalid = "INVALID INPUT:".Pastel(Color.Red);

        public static void Print(TestResultContainer[] TestResults) {
            if (TestResults.Length < 1) {
                PrintNoTests();
                return;
            }
            PrintTestResult(TestResults); 
            foreach (var result in TestResults) {
                Console.WriteLine(result);
            }
        }

        public static void PrintIfNotEmpty(string[] InvalidMethods, string[] InvalidClasses, string[] InvalidNamespaces) {
            if (InvalidMethods.Length > 0 || InvalidClasses.Length > 0 || InvalidNamespaces.Length > 0) {
                Console.WriteLine(invalid);
                string items = "";
                if (InvalidNamespaces.Length > 0) {
                    items += $"NAMESPCES: {InvalidNamespaces.GetContent()}\n";
                }
                if (InvalidClasses.Length > 0) {
                    items += $"CLASSES: {InvalidClasses.GetContent()}\n";
                }
                if (InvalidMethods.Length > 0) {
                    items += $"METHODS: {InvalidMethods.GetContent()}\n";
                }

                Console.Write(items);
                Console.WriteLine(separator);
            }
        }

        private static void PrintNoTests() {
            Console.WriteLine(separator);
            Console.WriteLine(noTests);
            Console.WriteLine(separator);
        }

        private static void PrintTestResult(TestResultContainer[] TestResults) {
            bool failed = default(bool);
            bool passed = default(bool);
            if (TestResults.Select(r => r.TestResult).Contains(TestResult.Failed)) {
                failed = true;
            }
            if (TestResults.Select(r => r.TestResult).Contains(TestResult.Passed)) {
                passed = true;
            }

            if (passed && failed) {
                Console.WriteLine(testResults.Pastel(Color.Yellow));
            }
            else {
                if (passed) {
                    Console.WriteLine(testResults.Pastel(Color.Green));
                }
                if (failed) {
                    Console.WriteLine(testResults.Pastel(Color.Red));
                }
            }
            Console.WriteLine(separator);
            Console.WriteLine(separator);
        }
    }
}
