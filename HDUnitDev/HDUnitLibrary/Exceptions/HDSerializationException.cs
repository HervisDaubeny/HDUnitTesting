using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Exceptions {

   public class HDSerializationException : HDRootException {

        public HDSerializationException() { }

        public HDSerializationException(string Message) : base(Message) { }

        public HDSerializationException(string Message, Exception Inner) : base(Message, Inner) { }
    }
}
