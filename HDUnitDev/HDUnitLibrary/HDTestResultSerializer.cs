using System;
using System.Collections.Generic;
using System.Text;

namespace HDUnit {
    public static class HDTestResultSerializer {

        private static string Path = null;

        public static void SetPath(string Path) {
            HDTestResultSerializer.Path = Path;
        }

        public static void Serialize(TestResultContainer[] TestResults) {

        }

        public static TestResultContainer[] Deserialize() {

            return null; //TODO: proper return
        }
    }
}
