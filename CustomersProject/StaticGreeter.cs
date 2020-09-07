using System;
using System.Data.Common;

namespace CustomersNamespace {
    public static class StaticGreeter {
        public static string Message = "static hello";
        public static void Greet() {

            Console.WriteLine(Message);
        }
    }
}
