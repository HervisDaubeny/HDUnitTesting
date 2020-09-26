using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Extensions {
    public static class ExceptionErrorDictionaryExtensions {

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
