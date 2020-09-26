using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension methods for 'ExceptionErrorDictionary' = <c>&lt;string, string[]&gt;</c>
    /// </summary>
    public static class ExceptionErrorDictionaryExtensions {

        /// <summary>
        /// Get content of Error dictionary
        /// </summary>
        /// <returns>String representing all errors from the dictionary.</returns>
        public static string GetContents(this IDictionary<string, string[]> Errors) {
            string content = "";
            int index = 0;
            foreach (var error in Errors) {
                content += $"{error.Key}: {error.Value.GetContent()}";
                if (index < Errors.Count - 1) {
                    content += "\n";
                }
                index++;
            }

            return content;
        }
    }
}
