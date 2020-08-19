using System;
using System.Data.Common;

namespace CustomersNamespace {
    public static class StaticGreeter {
        public static string Message = "static hello";
        public static void Greet() {

            Console.WriteLine(Message);
        }
    }

    public class InstanceGreeter {
        public string Message { get; set; }

        public InstanceGreeter() {
            Message = "instance hello";
        }

        public void Greet() {
            Console.WriteLine(Message);
        }
    }
}
