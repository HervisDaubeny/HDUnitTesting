using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Exceptions {

    public class HDParseInputException : HDRootException {

        public HDParseInputException() { }

        public HDParseInputException(string Message) :base(Message) { }

        public HDParseInputException(string Message, Exception Inner) :base(Message, Inner) { }
    }
}
