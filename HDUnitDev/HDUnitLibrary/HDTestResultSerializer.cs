using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HDUnit.Exceptions;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HDUnit {

    /// <summary>
    /// Class for serialization of data from test results using json.
    /// </summary>
    public static class HDTestResultSerializer {

        /// <summary>
        /// Predefined path for the serialization file
        /// </summary>
        private static string Path = Directory.GetCurrentDirectory() + "\\testResult.json";


        /// <summary>
        /// Set custom path for the serialization file
        /// </summary>
        /// <param name="Path"></param>
        public static void SetPath(string Path) {
            HDTestResultSerializer.Path = Path;
        }

        /// <summary>
        /// Serialize test result array to the json file
        /// </summary>
        /// <param name="TestResults">array to be serialized</param>
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

        /// <summary>
        /// Deserialize test results from the json file
        /// </summary>
        /// <returns>Test results from last run</returns>
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
