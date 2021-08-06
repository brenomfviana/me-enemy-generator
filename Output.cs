using System.IO;
using System.Text.Json;

namespace OverlordEnemyGenerator
{
    /// This class holds functions to write the evolutionary process' outputs.
    public static class Output
    {
        /// Write the collected data from the evolutionary process.
        public static void WriteData(
            Data data
        ) {
            // Create folder 'results'
            string folder = @"results";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            // Convert list to formmated JSON string
            var options = new JsonSerializerOptions(){ WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            // Prepare the JSON filename
            string filename = "";
            filename += "" + data.generations + '-';
            filename += "" + data.initialPopSize + '-';
            // Create folder `filename` if it does not exist
            folder = "results/" + filename;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            // Calculate the number of files in the folder
            int count = Directory.GetFiles(folder, "*.json").Length;
            // Update filename
            filename += count + ".json";
            // Write JSON file
            File.WriteAllText(folder + "/" + filename, json);
        }
    }
}