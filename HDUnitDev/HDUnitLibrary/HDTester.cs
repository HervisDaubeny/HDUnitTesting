﻿using System;
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
using System.Data;
using Pastel;
using System.Drawing;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.IO;
using System.Collections;

namespace HDUnit {

    /// <summary>
    /// Class containing the core of HDUnitTesting
    /// </summary>
    public static class HDTester {
        /// <summary>
        /// Reference to assembly with tests
        /// </summary>
        private static Assembly TestProject;
        /// <summary>
        /// File that contains output written by test methods to console.
        /// </summary>
        private static string outputPath = Directory.GetCurrentDirectory() + "\\tests_output.txt";
        private static TextWriter originalOutput;
        private static TextWriter redirectedOutput;
        private static FileStream redirectedFilestream;

        /// <summary>
        /// Initialize tests and open test controlling console.
        /// </summary>
        public static void InitTests() {
            TestProject = Assembly.GetCallingAssembly();
            HDConsoleControl.Run();
        }

        /// <summary>
        /// Gets TestClasses from calling assembly, apply filters defined by users command and run tests.
        /// </summary>
        /// <param name="command">Users command</param>
        public static void RunTests(Command command) {
            string[] invalidNamespaces = Array.Empty<string>();
            string[] invalidClasess = Array.Empty<string>();
            string[] invalidMethods = Array.Empty<string>();
            IEnumerable<Type> classes = null;
            IEnumerable<TestProcess> testProcesses = null;
            TestResultContainer[] lastRun = null;
            if (command.RunAs > 0) {
                lastRun = HDTestResultSerializer.Deserialize();
            }

            classes = GetClasses(command, lastRun, out invalidClasess, out invalidNamespaces);
            testProcesses = GetProcesses(classes, command, lastRun, out invalidMethods);
            var tasks = testProcesses.Select(x => x.GetTestsAsTask()).ToArray();
            RedirectOutput();
            if (command.MultithreadRun) {
                foreach (var task in tasks) {
                    task.Start();
                }

                Task.WaitAll(tasks);
            }
            else {
                foreach (var task in tasks) {
                    task.RunSynchronously();
                }
            }
            ResetOutput();
            HDResultPrinter.Print(testProcesses.GetResultContainersArray());
            HDResultPrinter.PrintIfNotEmpty(invalidMethods, invalidClasess, invalidNamespaces);
            HDTestResultSerializer.Serialize(testProcesses.GetResultContainersArray());
        }
        
        /// <summary>
        /// Apply conditions defined by users command and get classes from calling assembly.
        /// </summary>
        /// <param name="command">Created from users input</param>
        /// <param name="lastRun">Deserialized results of last run</param>
        /// <param name="invalidClasses">Classes that are not in the calling assembly</param>
        /// <param name="invalidNamespaces">Namespces that are not in the calling assembly</param>
        /// <returns>Collection of filtered test classes</returns>
        private static IEnumerable<Type> GetClasses(
                Command command,
                TestResultContainer[] lastRun,
                out string[] invalidClasses,
                out string[] invalidNamespaces) {

            IEnumerable<Type> classes = TestProject.GetTypes()
                .Where(t => t.IsClass && t.GetCustomAttribute<HDTestClassAttribute>(inherit: false) is object);
            switch (command.RunAs) {
                case RunMode.Default:
                    if (command.Namespaces.Length > 0) {
                        classes = classes.Where(t => command.Namespaces.Contains(t.Namespace));
                    }
                    if (command.Classes.Length > 0) {
                        classes = classes.Where(t => command.Classes.Contains(t.Name));
                    }
                    break;
                case RunMode.Repeat:
                    {
                        var lastClasses = lastRun.Select(r => r.ClassName).ToHashSet();
                        classes = classes.Where(c => lastClasses.Contains(c.Name));
                        break;
                    }
                case RunMode.Failed:
                    {
                        var lastClasses = lastRun
                            .Where(r => r.TestResult == TestResult.Failed)
                            .Select(r => r.ClassName).ToHashSet();
                        classes = classes.Where(c => lastClasses.Contains(c.Name));
                        break;
                    }
                case RunMode.Passed:
                    {
                        var lastClasses = lastRun
                            .Where(r => r.TestResult == TestResult.Passed)
                            .Select(r => r.ClassName).ToHashSet();
                        classes = classes.Where(c => lastClasses.Contains(c.Name));
                        break;
                    }
                case RunMode.New:
                    /* this Enum value will have meaning when filtering methods */
                    break;
                default:
                    throw new Exception("Impossible, but value was added to Enum and actually used.");
            }
            invalidClasses = command.Classes.Where(t => !(classes.Names().Contains(t))).ToArray();
            invalidNamespaces = command.Namespaces.Where(n => !(classes.Namespaces().Contains(n))).ToArray();

            return classes;
        }

        /// <summary>
        /// Get test methods organised to TestProcesses.
        /// </summary>
        /// <param name="Classes">Classes to search for test methods</param>
        /// <param name="command">Created from users input</param>
        /// <param name="lastRun">Deserialized results of last run</param>
        /// <param name="invalidMethods">Methods that are not in given Classes</param>
        /// <returns>Collection of TestProcesses</returns>
        private static IEnumerable<TestProcess> GetProcesses(
                IEnumerable<Type> Classes,
                Command command,
                TestResultContainer[] lastRun,
                out string[] invalidMethods) {
            
            List<TestProcess> processColection = new List<TestProcess>();
            List<String> existenceCheck = new List<string>();

            foreach (var Class in Classes) {
                object classInstance = Class.CreateInstance();
                IEnumerable<TestResultContainer> lastRunFiltered = lastRun;
                IEnumerable<MethodInfo> methodsToRun = Class.GetMethods()
                    .Where(m => m.GetCustomAttribute<HDTestMethodAttribute>(inherit: false) is object);
                existenceCheck.AddRange(methodsToRun.Select(m => m.Name));
                if (command.RunAs > 0) {
                    lastRunFiltered = lastRun.Where(run => run.ClassName == Class.Name);
                }
                switch (command.RunAs) {
                    case RunMode.Default:
                        if (command.Methods.Length > 0) {
                            methodsToRun = methodsToRun.Where(m => command.Methods.Contains(m.Name));
                        }
                        break;
                    case RunMode.Repeat:
                        var lastRunMethods = lastRunFiltered
                            .Select(m => m.MethodName)
                            .ToHashSet();
                        methodsToRun = methodsToRun.Where(m => lastRunMethods.Contains(m.Name));
                        break;
                    case RunMode.Failed:
                        var failedMethods = lastRunFiltered
                            .Where(m => m.TestResult == TestResult.Failed)
                            .Select(r => r.MethodName)
                            .ToHashSet();
                        methodsToRun = methodsToRun.Where(m => failedMethods.Contains(m.Name));
                        break;
                    case RunMode.Passed:
                        var passedMethods = lastRunFiltered
                            .Where(m => m.TestResult == TestResult.Passed)
                            .Select(r => r.MethodName)
                            .ToHashSet();
                        methodsToRun = methodsToRun.Where(m => passedMethods.Contains(m.Name));
                        break;
                    case RunMode.New:
                        var testedMethods = lastRunFiltered
                            .Select(r => r.MethodName)
                            .ToHashSet();
                        methodsToRun = methodsToRun.Where(m => !(testedMethods.Contains(m.Name)));
                        break;
                    default:
                        break;
                }

                List<MethodInfo> independentMethods = methodsToRun
                    .Where(m => !(m.GetCustomAttribute<HDRunAfterAttribute>(inherit: false) is object))
                    .ToList();
                List<MethodInfo> dependentMethods = methodsToRun
                    .Where(m => m.GetCustomAttribute<HDRunAfterAttribute>(inherit: false) is object)
                    .ToList();
                TestProcess[] processes = independentMethods
                    .Select(m => new TestProcess(m, classInstance, Class.Name))
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

            invalidMethods = command.Methods.Where(m => !(existenceCheck.Contains(m))).ToArray();
            return processColection;
        }

        /// <summary>
        /// Redirect output from console to a file.
        /// </summary>
        private static void RedirectOutput() {
            originalOutput = Console.Out;
            redirectedFilestream = File.OpenWrite(outputPath);
            redirectedOutput = new StreamWriter(redirectedFilestream);
            Console.SetOut(redirectedOutput);
        }

        /// <summary>
        /// Reset output back to console.
        /// </summary>
        private static void ResetOutput() {
            redirectedOutput.Flush();
            redirectedOutput.Close();
            redirectedOutput.Dispose();
            redirectedFilestream.Close();
            redirectedFilestream.Dispose();
            Console.SetOut(originalOutput);
        }
    }

    /// <summary>
    /// Object representing test process, ensuring desired order of test methods.
    /// </summary>
    public class TestProcess {

        /// <summary>
        /// Name of class used by methods from <paramref name="TestMethods"/>
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Test methods
        /// </summary>
        public IEnumerable<MethodInfo> TestMethods { get; set; }
        /// <summary>
        /// Instance of class named in <paramref name="ClassName"/>
        /// </summary>
        public object ClassInstance { get; set; }
        /// <summary>
        /// Array with results of all run test methods
        /// </summary>
        public TestResultContainer[] TestResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TestMethod"></param>
        /// <param name="ClassInstance"></param>
        /// <param name="ClassName"></param>
        public TestProcess(MethodInfo TestMethod, object ClassInstance, string ClassName) {
            this.TestMethods = new List<MethodInfo>() { TestMethod };
            this.ClassInstance = ClassInstance;
            this.ClassName = ClassName;
        }

        /// <summary>
        /// Object representing test process, ensuring desired order of test methods.
        /// </summary>
        /// <param name="DependantMethod"></param>
        public void AddDependentMethod(MethodInfo DependantMethod) {
            List<MethodInfo> newTestMethods = TestMethods.ToList();
            newTestMethods.Add(DependantMethod);
            TestMethods = newTestMethods;
        }

        /// <summary>
        /// Get task that will run the test methods.
        /// </summary>
        /// <returns>Runnable task</returns>
        public Task GetTestsAsTask() {
            return new Task(InternalRun);
        }

        /// <summary>
        /// Run test methods
        /// </summary>
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
                            string errorMessage = ex.Message;
                            if (ex.Errors.Any()) {
                                errorMessage += '\n';
                                errorMessage += ex.Errors.GetContents();
                            }
                            results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Failed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                new string[0],
                                atts[j].GetParams(),
                                ResultMessage: errorMessage);
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
                            string errorMessage = ex.Message;
                            if (ex.Errors.Any()) {
                                errorMessage += '\n';
                                errorMessage += ex.Errors.GetContents();
                            }
                            results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Failed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                gAtts[j].GetTypes(),
                                gAtts[j].GetParams(),
                                ResultMessage: errorMessage);
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
                    string errorMessage = ex.Message;
                    if (ex.Errors.Any()) {
                        errorMessage += '\n';
                        errorMessage += ex.Errors.GetContents();
                    }
                    results[resultIndex] = new TestResultContainer(
                                HDUnit.TestResult.Failed,
                                this.GetClassName(),
                                methods[i].Name,
                                methods[i].GetCustomName(),
                                new string[0],
                                new string[0],
                                ResultMessage: errorMessage);
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

    /// <summary>
    /// Container for storing test results
    /// </summary>
    public class TestResultContainer {
        
        /// <summary>
        /// Predefined message
        /// </summary>
        [JsonIgnore]
        private string passedMessage = "PASSED".Pastel(Color.Green);
        /// <summary>
        /// Predefined message
        /// </summary>
        [JsonIgnore]
        private string failedMessage = "FAILED".Pastel(Color.Red);

        /// <summary>
        /// Result of test method
        /// </summary>
        public TestResult TestResult { get; set; }
        /// <summary>
        /// Name of class defining the test method
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Name of test the method
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Alias of the test method
        /// </summary>
        public string MethodAlias { get; set; }
        /// <summary>
        /// Generic types of the test method
        /// </summary>
        public string[] GenericParameters { get; set; }
        /// <summary>
        /// Parameters of the test method
        /// </summary>
        public string[] Parameters { get; set; }
        /// <summary>
        /// Error message resulting from the test method
        /// </summary>
        public string ResultMessage { get; set; }
        /// <summary>
        /// Length of the test run measured in miliseconds
        /// </summary>
        public long TestTime { get; set; }

        /// <summary>
        /// Create new TestResultContainer to save the results.
        /// </summary>
        /// <param name="TestResult">Result of test method</param>
        /// <param name="ClassName">Name of class defining the test method</param>
        /// <param name="MethodName">Name of test the method</param>
        /// <param name="MethodAlias">Alias of the test method</param>
        /// <param name="GenericParameters">Generic types of the test method</param>
        /// <param name="Parameters">Parameters of the test method</param>
        /// <param name="TestTime">Length of the test run measured in miliseconds</param>
        /// <param name="ResultMessage">Error message resulting from the test method</param>
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

        /// <summary>
        /// Override the default ToString method.
        /// </summary>
        /// <returns>Valid string representation of the TestResultContainer </returns>
        public override string ToString() {
            const string separator = "========================================";
            string representation = "";

            if (MethodAlias is object) {
                representation += $"Test name: {MethodAlias}\n";
            }
            if (TestResult == TestResult.Passed) {
                representation += $"{passedMessage} -- ";
            }
            else {
                representation += $"{failedMessage} -- ";
            }
            representation += MethodName;
            if (GenericParameters.Length > 0) {
                representation += $"<{GenericParameters.GetContent()}>";
            }
            representation += $"({Parameters.GetContent()})\n";
            if (TestResult == TestResult.Failed) {
                representation += $"message: {ResultMessage}\n";
            }
            representation += $"ran: {TestTime}ms\n";
            representation += separator;

            return representation;
        }
    }

    /// <summary>
    /// Enum representing if the test passed or failed.
    /// </summary>
    public enum TestResult {
        Failed,
        Passed
    }
}
