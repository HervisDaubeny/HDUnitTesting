using System;
using System.Collections.Generic;
using System.Text;

namespace CustomersNamespace {
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
