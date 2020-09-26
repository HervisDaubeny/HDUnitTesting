﻿using HDUnit.Exceptions;
using HDUnit.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace HDUnit {

    /// <summary>
    /// Class for communication with the user and controlling test running.
    /// </summary>
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

        /// <summary>
        /// Start up ConsoleControl.
        /// </summary>
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

        /// <summary>
        /// Initial state. Greet, ask for input, offer a help.
        /// </summary>
        private static void State0() {
            Console.WriteLine("Welcome to HDUnitTesting. For list of commands type 'help' in the console.");
            Console.WriteLine("To exit input newLine.");
            state = 1;
        }

        /// <summary>
        /// Read input, decide next state.
        /// </summary>
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
                    HDTestResultSerializer.SetPath(command.Methods[0]);
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

        /// <summary>
        /// Terminating state.
        /// </summary>
        private static void StateN() {
            Console.WriteLine("If you made any change to the project you are testing, " +
                "please don't forget to rebuild the solution before running the tests again.");
        }

        /// <summary>
        /// Create new command from given string.
        /// </summary>
        /// <param name="input">String to use</param>
        /// <returns>Command given by the user</returns>
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

    /// <summary>
    /// Class representing users command.
    /// </summary>
    public class Command {
        /// <summary>
        /// Name of the command
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Methods from the input
        /// </summary>
        public string[] Methods { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Classes from the input
        /// </summary>
        public string[] Classes { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Namespaces from the input
        /// </summary>
        public string[] Namespaces { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Flag signaling usage of single or multi threaded run
        /// </summary>
        public bool MultithreadRun { get; set; }
        /// <summary>
        /// Enum describing the desired run mode
        /// </summary>
        public RunMode RunAs { get; set; }

        /// <summary>
        /// Create new Command
        /// </summary>
        /// <param name="Name">Name of the command</param>
        /// <param name="Args">Methods from the input</param>
        /// <param name="Class">Classes from the input</param>
        /// <param name="Namespace">Namespaces from the input</param>
        /// <param name="Multithreaded">Flag signaling usage of single or multi threaded run</param>
        /// <param name="RunAs">Enum describing the desired run mode</param>
        public Command(string Name, string[] Args, string[] Class, string[] Namespace, bool Multithreaded, RunMode RunAs) {
            this.Value = Name;
            this.RunAs = RunAs;
            if (Args.Length > 0) {
                this.Methods = Args;
            }
            if (Class.Length > 0) {
                this.Classes = Class;
            }
            if (Namespace.Length > 0) {
                this.Namespaces = Namespace;
            }
            if (Multithreaded) {
                this.MultithreadRun = true;
            }
        }
    }

    /// <summary>
    /// Enumerator used for deciding what to do with results from last test
    /// </summary>
    public enum RunMode {
        Default,
        Repeat,
        Failed,
        Passed,
        New
    }
}
