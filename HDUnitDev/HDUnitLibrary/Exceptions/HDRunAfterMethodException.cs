using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Exceptions {
    public class HDRunAfterMethodException : HDRootException {
        public HDRunAfterMethodException() { }

        public HDRunAfterMethodException(string Message) : base(Message) { }

        public HDRunAfterMethodException(string Message, Exception Inner) : base(Message, Inner) { }
    }
}
