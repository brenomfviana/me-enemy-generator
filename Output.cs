using System.IO;
using System.Text.Json;

namespace OverlordEnemyGenerator
{
    /// This class holds functions to write the evolutionary process' outputs.
    public static class Output
    {
        /// Write the collected data from the evolutionary process.
        public static void WriteData(
            Data data,
            string[] args
        ) {
            // Convert list to formmated JSON string
            var options = new JsonSerializerOptions(){ WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            // Calculate the JSON filename
            string filename = "";
            filename += args[2] + '-';
            filename += args[3] + '-';
            filename += args[4] + '-';
            filename += args[1] + '-';
            filename += args[0];
            // Write JSON file
            File.WriteAllText("results/" + filename + ".json", json);
        }
    }
}