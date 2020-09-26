using HDUnit.Exceptions;
using HDUnit.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace HDUnit {
    static class HDConsoleControl {
        const string classKeyword = "--class:";
        const string classShortcut = "-c:";
        const string namespaceKeyword = "--namespace:";
        const string namespaceShortcut = "-n:";
        const string multithreadKeyword = "--multithread";
        const string multithreadShortcut = "-m";
        const string repeatKeyword = "--repeat";
        const string repeatShortcut = "-r";
        const string failedKeyword = "--failed";
        const string failedShortcut = "-f";
        const string passedKeyword = "--passed";
        const string passedShortcut = "-p";
        const string unrunKeyword = "--unruned";
        const string unrunShortcut = "-u";

        static char[] inputSeparators = new char[] { ' ' };
        static int state = 0;

        public static void Run() {
            while (state >= 0) {
                switch (state) {
                    case 0:
                        State0();
                        break;
                    case 1:
                        State1();
                        break;
                    default:
                        break;
                }
            }

        }

        /// Initial state. Greet, ask for input, offer a help.
        private static void State0() {
            Console.WriteLine("Welcome to HDUnitTesting. For list of commands type 'help' in the console.");
            Console.WriteLine("To exit input newLine.");
            state = 1;
        }
        /// Read input, decide next state.
        private static void State1() {
            Console.Write('>');
            var command = GetCommand(Console.ReadLine());

            switch (command.Value) {
                case "help":
                    Console.WriteLine("This is list of supported commands:");
                    Console.WriteLine("help  ..  prints this list");
                    Console.WriteLine("run_test  ..  runs single test");
                    Console.WriteLine("run_tests  ..  runs all tests");
                    //TODO: fill the list of commands
                    state = 1;
                    break;
                case "run_tests":
                case "run_test":
                    HDTester.RunTests(command);
                    state = 1;
                    break;
                case "set_path":
                    HDTestResultSerializer.SetPath(command.Args[0]);
                    state = 1;
                    break;
                case "":
                    StateN();
                    state = -1;
                    break;
                default:
                    Console.WriteLine("Unknown command. For list of command type 'help'.");
                    break;
            }
        }
        /// Terminating state.
        private static void StateN() {
            Console.WriteLine("If you made any change to the project you are testing, " +
                "please don't forget to rebuild the solution before running the tests again.");
        }

        private static Command GetCommand(string input) {
            char[] innerSep = new char[] { ':' };
            string Name = default(string);
            List<string> Args = new List<string>();
            List<string> Class = new List<string>();
            List<string> Namespace = new List<string>();
            bool Multithread = false;
            RunMode RunAs = RunMode.Default;


            string[] splited = input.RemoveNewLine().Split(inputSeparators);
            try {
                Name = splited[0];

                for (int i = 1; i < splited.Length; i++) {
                    if (splited[i].Contains(classKeyword) || splited[i].Contains(classShortcut)) {
                        Class.Add(splited[i].Split(innerSep)[1]);
                        continue;
                    }
                    if (splited[i].Contains(namespaceKeyword) || splited[i].Contains(namespaceShortcut)) {
                        Namespace.Add(splited[i].Split(innerSep)[1]);
                        continue;
                    }
                    if (splited[i] == multithreadKeyword || splited[i] == multithreadShortcut) {
                        Multithread = true;
                        continue;
                    }
                    if (splited[i] == repeatKeyword || splited[i] == repeatShortcut) {
                        RunAs = RunMode.Repeat;
                        continue;
                    }
                    if (splited[i] == failedKeyword || splited[i] == failedShortcut) {
                        RunAs = RunMode.Failed;
                        continue;
                    }
                    if (splited[i] == passedKeyword || splited[i] == passedShortcut) {
                        RunAs = RunMode.Passed;
                        continue;
                    }
                    if (splited[i] == unrunKeyword || splited[i] == unrunShortcut) {
                        RunAs = RunMode.New;
                        continue;
                    }
                    Args.Add(splited[i]);
                }
            }
            catch (Exception e) {
                throw new HDParseInputException($"unexpected exception when parsing input:\n    {e.Message}");
            }

            return new Command(Name, Args.ToArray(), Class.ToArray(), Namespace.ToArray(), Multithread, RunAs);
        }
    }   
    public class Command {
        public string Value { get; set; }
        public string[] Args { get; set; } = Array.Empty<string>();
        public string[] Class { get; set; } = Array.Empty<string>();
        public string[] Namespace { get; set; } = Array.Empty<string>();
        public bool MultithreadRun { get; set; }
        public RunMode RunAs { get; set; }

        public Command(string Name, string[] Args, string[] Class, string[] Namespace, bool Multithreaded, RunMode RunAs) {
            this.Value = Name;
            this.RunAs = RunAs;
            if (Args.Length > 0) {
                this.Args = Args;
            }
            if (Class.Length > 0) {
                this.Class = Class;
            }
            if (Namespace.Length > 0) {
                this.Namespace = Namespace;
            }
            if (Multithreaded) {
                this.MultithreadRun = true;
            }
        }
    }

    public enum RunMode {
        Default,
        Repeat,
        Failed,
        Passed,
        New
    }
}
