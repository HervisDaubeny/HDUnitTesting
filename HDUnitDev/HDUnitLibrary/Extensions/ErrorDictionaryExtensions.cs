using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension methods for 'ErrorDictionary' = <c>&lt;string, string[]&gt;</c>
    /// </summary>
    public static class ErrorDictionaryExtension {

        /// <summary>
        /// Add new log in the error dictionary
        /// </summary>
        /// <param name="key">Key of new log</param>
        /// <param name="singleError">Value of new log</param>
        public static void Add(this IDictionary<string, string[]> dir, string key, string singleError) {
            dir.Add(key, new string[] { singleError });
        }
    }
}
