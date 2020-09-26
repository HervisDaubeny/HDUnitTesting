using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Sources;

namespace HDUnit.Exceptions {

    /// <summary>
    /// Exception thrown when HDAssert method fails.
    /// </summary>
    public class HDAssertFailedException : HDRootException {

        /// <summary>
        /// Error dictionary for extended collection of wrong data.
        /// </summary>
        public IDictionary<string, string[]> Errors { get; set; }

        /// <summary>
        /// Exception thrown when HDAssert method fails.
        /// </summary>
        public HDAssertFailedException() {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Exception thrown when HDAssert method fails.
        /// </summary>
        /// <param name="Message">Exception message</param>
        public HDAssertFailedException(string Message) : base(Message) {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Exception thrown when HDAssert method fails.
        /// </summary>
        /// <param name="Message">Exception message</param>
        /// <param name="Errors">Collection of errors encountered during test</param>
        public HDAssertFailedException(string Message, IDictionary<string, string[]> Errors) : base(Message) {
            this.Errors = Errors;
        }

        /// <summary>
        /// Exception thrown when HDAssert method fails.
        /// </summary>
        /// <param name="Message">Exception message</param>
        /// <param name="Inner">The exception that occurred during test</param>
        public HDAssertFailedException(string Message, Exception Inner) : base(Message, Inner) {
            Errors = new Dictionary<string, string[]>();
        }
    }
}
