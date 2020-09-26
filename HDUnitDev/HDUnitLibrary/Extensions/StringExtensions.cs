using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Extensions {

    /// <summary>
    /// Extension methods for String.
    /// </summary>
    public static class StringExtensions {

        /// <summary>
        /// Remove leading and trailing new line characters.
        /// </summary>
        /// <returns>String without leading and trailing new line characters.</returns>
        public static string RemoveNewLine(this string str) {
            char newLine = '\n';
            return str.Trim(newLine);
        }

        /// <summary>
        /// Get string representation of content of this array.
        /// </summary>
        /// <returns>string representation of array content</returns>
        public static string GetContent(this string[] array) {
            string content = "";
            if (array.Length < 1) {
                return content;
            }

            int i = 0;
            for (; i < array.Length - 1; i++) {
                content += $"{array[i]}, ";
            }
            content += array[i];
            return content;
        }
    }
}
