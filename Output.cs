using System;
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
                System.IO.Directory.CreateDirectory(folder);
            }
            // Convert list to formmated JSON string
            var options = new JsonSerializerOptions(){ WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            // Calculate the JSON filename
            string filename = "";
            // Number of generations
            filename += "" + data.generations + '-';
            // Initial population size
            filename += "" + data.initialPopSize + '-';
            // Offspring size
            filename += "" + data.offspringSize + '-';
            // Desired difficulty
            filename += "" + data.goal;
            // Write JSON file
            File.WriteAllText("results/" + filename + ".json", json);
        }
    }
}