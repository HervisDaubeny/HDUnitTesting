using System;
using System.Collections.Generic;
using System.Text;

namespace CustomersNamespace {

    public class Greeter {

        public Greeter() {

        }

        public void Greet <V, W> (V Vparam, W Wparam, int num) {
            Console.WriteLine("{0} {1} {2}", Vparam, Wparam, num);
        }
    }
}
