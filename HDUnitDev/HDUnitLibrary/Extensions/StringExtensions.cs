using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Extensions {
    public static class StringExtensions {

        public static string RemoveNewLine(this string str) {
            char newLine = '\n';
            return str.Trim(newLine);
        }

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
