using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit.Extensions {

    public static class ErrorDictionaryExtension {
        public static void Add(this IDictionary<string, string[]> dir, string key, string singleError) {
            dir.Add(key, new string[] { singleError });
        }
    }
}
