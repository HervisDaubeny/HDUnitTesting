using System;
using System.Collections.Generic;
using System.Text;

namespace CustomersNamespace {

    public class GenericGreeter<T, U> {

        public T TProp { get; set; }
        public U UProp { get; set; }

        public GenericGreeter(T TProp, U UProp) {
            this.TProp = TProp;
            this.UProp = UProp;
        }

        public void Greet <V, W> (V Vparam, W Wparam, int num) {
            Console.WriteLine("{0} {1} {2}", Vparam, Wparam, num);
        }
    }
}
