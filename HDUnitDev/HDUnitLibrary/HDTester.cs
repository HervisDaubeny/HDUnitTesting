using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Dynamic;
using HDUnit.Attributes;

namespace HDUnit {
    public static class HDTester {

        private static List<Type> TestClassList;

        public static void InitTests() {
            TestClassList = Assembly.GetCallingAssembly().GetTypes()
                .Where(t => t.IsClass &&
                t.GetCustomAttribute<HDTestClassAttribute>(inherit: false) is object).ToList();
        }

        public static void RunTest(string Method, string Class = null, string Namespace = null) {
            
            foreach (var item in TestClassList) {
                Console.WriteLine(item.Name);
                /*
                var attrs = item.GetCustomAttributes(inherit: false);
                foreach (var att in attrs) {
                    Console.WriteLine($"    {att}");
                }*/
            }
        }

        public static void RunTest(RunSingleTestMode Mode = RunSingleTestMode.Repeat) {

        }

        public static void RunTests(params string[] MethodNames) {

        }

        public static void RunTests(RunMultipleTestsMode Mode) {

        }
        
    }

    public enum RunSingleTestMode {
        Repeat
    }

    public enum RunMultipleTestsMode {
        All,
        Repeat,
        Passed,
        Failed
    }
}
