using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HDUnit.Exceptions;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HDUnit {
    public static class HDTestResultSerializer {

        private static string Path = Directory.GetCurrentDirectory() + "\\testResult.json";
        public static void SetPath(string Path) {
            HDTestResultSerializer.Path = Path;
        }

        public static void Serialize(TestResultContainer[] TestResults) {
            try {
                string serialization = JsonConvert.SerializeObject(TestResults, Formatting.Indented);
                File.WriteAllText(Path, serialization);
            }
            catch (JsonWriterException jwex) {
                throw new HDSerializationException("Can't serialize data.", jwex);
            }
            catch (IOException ioex) {
                throw new HDSerializationException("Writing to a file failed.", ioex);
            }
            catch (Exception ex) {
                throw new HDSerializationException("Unexpected error during serialization.", ex);
            }
        }

        public static TestResultContainer[] Deserialize() {
            TestResultContainer[] lastRunResults = null;
            try {
                string json = File.ReadAllText(Path);
                lastRunResults = JsonConvert.DeserializeObject<TestResultContainer[]>(json);
            }
            catch (IOException ioex) {
                throw new HDSerializationException("Reading file has failed.", ioex);
            }
            if (lastRunResults.Length < 1) {
                throw new HDSerializationException("Provided file could not be deserialized.");
            }

            return lastRunResults;
        }
    }
}
