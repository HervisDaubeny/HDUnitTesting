using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Dynamic;
using HDUnit.Attributes;
using System.Runtime.CompilerServices;
using HDUnit.Extensions;
using HDUnit.Exceptions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;

namespace HDUnit {
    public static class HDTester {

        private static Assembly TestProject;

        private static List<Type> TestClassList = new List<Type>();
        private static List<List<MethodInfo>> TestMethodsNestedList = new List<List<MethodInfo>>();
        private static List<List<MethodInfo>> TestMethodsCallOrder = new List<List<MethodInfo>>();

        public static void InitTests() {
            TestProject = Assembly.GetCallingAssembly();
            HDConsoleControl.Run();
        }

        public static void RunTests(Command command) {
            TestResultContainer[] lastRun = null;
            if (command.RunAs > 0) {
                lastRun = HDTestResultSerializer.Deserialize(); //TODO: think of way how to do this safely with try catch + check if contains valid data
            }
            IEnumerable<Type> classes = GetClasses(command, lastRun);
            IEnumerable<TestProcess> testProcesses = GetProcesses(classes, command, lastRun);
            var tasks = testProcesses.Select(x => x.GetTestsAsTasks()).ToArray();
            foreach (var task in tasks) {
                task.Start();
            }

            Task.WaitAll(tasks);
            HDResultPrinter.Print(testProcesses.GetResultContainersArray());
            HDTestResultSerializer.Serialize(testProcesses.GetResultContainersArray());
        }
        
        private static IEnumerable<Type> GetClasses(Command command, TestResultContainer[] lastRun) {
            IEnumerable<Type> classes = TestProject.GetTypes()
                .Where(t => t.IsClass && t.GetCustomAttribute<HDTestClassAttribute>(inherit: false) is object);
            switch (command.RunAs) {
                case RunMode.Default:
                    if (command.Namespace.Length > 0) {
                        classes = classes.Where(t => command.Namespace.Contains(t.Namespace));
                    }
                    if (command.Class.Length > 0) {
                        classes = classes.Where(t => command.Class.Contains(t.GetType().ToString()));
                    }
                    break;
                case RunMode.Repeat:
                    {
                        string[] lastClasses = lastRun.Select(r => r.ClassName).Distinct().ToArray();
                        classes = classes.Where(c => lastClasses.Contains(c.GetType().ToString()));
                        break;
                    }
                case RunMode.Failed:
                    {
                        string[] lastClasses = lastRun
                            .Where(r => r.TestResult == TestResult.Failed)
                            .Select(r => r.ClassName).Distinct().ToArray();
                        classes = classes.Where(c => lastClasses.Contains(c.GetType().ToString()));
                        break;
                    }
                case RunMode.Passed:
                    {
                        string[] lastClasses = lastRun
                            .Where(r => r.TestResult == TestResult.Passed)
                            .Select(r => r.ClassName).Distinct().ToArray();
                        classes = classes.Where(c => lastClasses.Contains(c.GetType().ToString()));
                        break;
                    }
                case RunMode.New:
                    /* this Enum value will have meaning when filtering methods */
                    break;
                default:
                    throw new Exception("Impossible, but value was added to Enum and actually used.");
            }

            return classes;
        }

        private static IEnumerable<TestProcess> GetProcesses(IEnumerable<Type> Classes, Command command, TestResultContainer[] lastRun) {
            List<TestProcess> processColection = new List<TestProcess>();

            foreach (var Class in Classes) {
                object classInstance = Class.CreateInstance();
                List<MethodInfo> methodToRun = Class.GetMethods()
                    .Where(m => m.GetCustomAttribute<HDTestMethodAttribute>(inherit: false) is object)
                    .ToList();
                switch (command.RunAs) { //TODO: filter methods to run
                    case RunMode.Default:
                        break;
                    case RunMode.Repeat:
                        break;
                    case RunMode.Failed:
                        break;
                    case RunMode.Passed:
                        break;
                    case RunMode.New:
                        break;
                    default:
                        break;
                }

                List<MethodInfo> independentMethods = methodToRun
                    .Where(m => !(m.GetCustomAttribute<HDRunAfterAttribute>(inherit: false) is object))
                    .ToList();
                List<MethodInfo> dependentMethods = methodToRun
                    .Where(m => m.GetCustomAttribute<HDRunAfterAttribute>(inherit: false) is object)
                    .ToList();
                TestProcess[] processes = independentMethods
                    .Select(m => new TestProcess(m, classInstance))
                    .ToArray();

                while (dependentMethods.Count > 0) {
                    bool messedDependency = true;

                    for (int i = 0; i < dependentMethods.Count; i++) {
                        string dependsName = dependentMethods[i].GetDependency();
                        TestProcess dependsProcess = processes
                            .SingleOrDefault(p => p.ContainsMethod(dependsName));

                        if (dependsProcess is object) {
                            dependsProcess.AddDependentMethod(dependentMethods[i]);
                            dependentMethods.RemoveAt(i);
                            messedDependency = false;
                            break;
                        }

                        continue;
                    }

                    if (messedDependency) {
                        throw new HDRunAfterMethodException("Cyclic or missing dependency.");
                    }
                }

                processColection.AddRange(processes);
            }

            return processColection;
        }
    }

    /// <summary>
    /// Object representing a process that is run in desired order, through parallel execution.
    /// </summary>
    public class TestProcess {

        public IEnumerable<MethodInfo> TestMethods { get; set; }
        public object ClassInstance { get; set; }
        public TestResultContainer[] TestResult { get; set; }

        public TestProcess(MethodInfo TestMethod, object ClassInstance) {
            this.TestMethods = new List<MethodInfo>() { TestMethod };
            this.ClassInstance = ClassInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DependantMethod"></param>
        public void AddDependentMethod(MethodInfo DependantMethod) {
            List<MethodInfo> newTestMethods = TestMethods.ToList();
            newTestMethods.Add(DependantMethod);
            TestMethods = newTestMethods;
        }

        public Task GetTestsAsTasks() {
            return new Task(InternalRun);
        }

        private void InternalRun() {
            MethodInfo[] methods = this.TestMethods.ToArray();
            int genericRuns = methods
                .Select(m => m.GetCustomAttributes<HDGenericParametersAttribute>(inherit: false)
                .Count())
                .Sum();
            int nonGenericRuns = methods
                .Select(m => m.GetCustomAttributes<HDParametersAttribute>(inherit: false)
                .Count())
                .Sum();
            int paramlessRuns = methods
                .Where(m =>
                (m.GetCustomAttributes<HDParametersAttribute>(inherit: false).Count() < 1)
                && (m.GetCustomAttributes<HDGenericParametersAttribute>(inherit: false).Count() < 1))
                .Count();
            if (genericRuns < 0)
                genericRuns = 0;
            if (nonGenericRuns < 0)
                nonGenericRuns = 0;
            TestResultContainer[] results = new TestResultContainer[paramlessRuns + genericRuns + nonGenericRuns];

            int resultIndex = 0;
            for(int i = 0; i < methods.Length; i++, resultIndex++) {
                var timer = new System.Diagnostics.Stopwatch();
                if (methods[i].GetCustomAttributes<HDParametersAttribute>(inherit: false) is HDParametersAttribute[] atts && atts.Length > 0) {
                    for (int j = 0; j < atts.Length; j++, resultIndex++) {
                        try {
                            timer.Reset();
                            timer.Start();
                            methods[i].Invoke(this.ClassInstance, atts[j].Parameters);
                            timer.Stop();
                            results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Passed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                new string[0],
                                atts[j].GetParams(),
                                timer.ElapsedMilliseconds);
                        }
                        catch (Exception e) when (e.InnerException is HDAssertFailedException ex) {
                            results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Failed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                new string[0],
                                atts[j].GetParams(),
                                ResultMessage: ex.Message);
                        }
                        catch (Exception e) when (!(e.InnerException is HDAssertFailedException)) {
                            string methodRep = "";
                            methodRep += methods[i].Name;
                            methodRep += $"({atts[j].GetParams().GetContent()})";
                            throw new Exception($"Unexpected exception in {methodRep}: {e.InnerException.Message}");
                        }
                    }
                    resultIndex--; // because it is incremented in outer for too and would cause IndexOutOfRange
                    continue;
                }
                else if (methods[i].GetCustomAttributes<HDGenericParametersAttribute>(inherit: false) is HDGenericParametersAttribute[] gAtts && gAtts.Length > 0) {
                    for (int j = 0; j < gAtts.Length; j++, resultIndex++) {
                        try {
                            var genericMethRunable = methods[i].MakeGenericMethod(gAtts[j].Types);
                            timer.Reset();
                            timer.Start();
                            genericMethRunable.Invoke(this.ClassInstance, gAtts[j].Parameters);
                            timer.Stop();
                            results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Passed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                gAtts[j].GetTypes(),
                                gAtts[j].GetParams(),
                                timer.ElapsedMilliseconds);
                        }
                        catch (Exception e) when (e.InnerException is HDAssertFailedException ex) {
                            results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Failed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                gAtts[j].GetTypes(),
                                gAtts[j].GetParams(),
                                ResultMessage: ex.Message);
                        }
                        catch (Exception e) when (!(e.InnerException is HDAssertFailedException)) {
                            string methodRep = "";
                            methodRep += methods[i].Name;
                            methodRep += $"({gAtts[j].GetParams().GetContent()})";
                            throw new Exception($"Unexpected exception in {methodRep}: {e.InnerException.Message}");
                        }
                    }
                    resultIndex--; // because it is incremented in outer for too and would cause IndexOutOfRange
                    continue;
                }
                // invoke a method without Parameters Attribute
                try {
                    timer.Reset();
                    timer.Start();
                    methods[i].Invoke(this.ClassInstance, null);
                    timer.Stop();
                    results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Passed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                new string[0],
                                new string[0],
                                timer.ElapsedMilliseconds);
                }
                catch (Exception e) when (e.InnerException is HDAssertFailedException ex) {
                    results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Failed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                new string[0],
                                new string[0],
                                ResultMessage: ex.Message);
                }
                catch (Exception e) when (!(e.InnerException is HDAssertFailedException)) {
                    string methodRep = "";
                    methodRep += methods[i].Name + "()";
                    throw new Exception($"Unexpected exception in {methodRep}: {e.InnerException.Message}");
                }
            }

            this.TestResult = results;
        }
    }

    public class TestResultContainer {

        public TestResult TestResult { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string MethodAlias { get; set; }
        public string[] GenericParameters { get; set; }
        public string[] Parameters { get; set; }
        public string ResultMessage { get; set; }
        public long TestTime { get; set; }

        /// <summary>
        /// Create new TestResultContainer to save the results.
        /// </summary>
        /// <param name="TestResult"></param>
        /// <param name="ClassName"></param>
        /// <param name="MethodName"></param>
        /// <param name="MethodAlias"></param>
        /// <param name="GenericParameters"></param>
        /// <param name="Parameters"></param>
        /// <param name="TestTime"></param>
        /// <param name="ResultMessage"></param>
        public TestResultContainer(
            TestResult TestResult,
            string ClassName,
            string MethodName,
            string MethodAlias,
            string[] GenericParameters,
            string[] Parameters,
            long TestTime = 0,
            string ResultMessage = null) {

            this.TestResult = TestResult;
            this.ClassName = ClassName;
            this.MethodName = MethodName;
            this.MethodAlias = MethodAlias;
            this.GenericParameters = GenericParameters;
            this.Parameters = Parameters;
            this.ResultMessage = ResultMessage;
            this.TestTime = TestTime;
        }

        public override string ToString() {
            const string separator = "========================================";
            string representation = "";

            if (MethodAlias is object) {
                representation += MethodAlias;
            }
            else {
                representation += MethodName;
            }
            if (GenericParameters.Length > 0) {
                representation += $"<{GenericParameters.GetContent()}>";
            }
            representation += $"({Parameters.GetContent()})";
            if (TestResult == TestResult.Passed) {
                representation += " -- PASSED\n";
            }
            else {
                representation += " -- FAILED\n";
                representation += $"message: {ResultMessage}\n";
            }
            representation += $"ran: {TestTime}ms\n";
            representation += separator;

            return representation;
        }
    }

    public enum TestResult {
        Failed,
        Passed
    }
}
