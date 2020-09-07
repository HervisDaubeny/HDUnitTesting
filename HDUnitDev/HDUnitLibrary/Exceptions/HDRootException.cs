using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Exceptions {

    /// <summary>
    /// Abstract exception. Allows me to group all my custom exceptions due to inheritance.
    /// </summary>
    [Serializable]
    public abstract class HDRootException : Exception {

        public HDRootException() { }

        public HDRootException(string Message) :base(Message) { }

        public HDRootException(string Message, Exception Inner) : base(Message, Inner) { }
    }
}
