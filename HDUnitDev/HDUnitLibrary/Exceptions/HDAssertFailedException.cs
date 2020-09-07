using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Sources;

namespace HDUnit.Exceptions {

    public class HDAssertFailedException : HDRootException {

        public HDAssertFailedException() { }

        public HDAssertFailedException(string Message) :base(Message) { }

        public HDAssertFailedException(string Message, Exception Inner) :base(Message, Inner) { }
    }
}
