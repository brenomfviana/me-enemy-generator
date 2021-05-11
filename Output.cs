using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace OverlordEnemyGenerator
{
    /// .
    public static class Output
    {
        /// .
        public static void WriteMAPElites(
            Population solution,
            string[] args
        ) {
            // Get individuals
            List<Individual> data = solution.ToList();
            // Convert list to formmated JSON string
            var options = new JsonSerializerOptions(){ WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            // Calculate the JSON filename
            string filename = "";
            for (int i = 1; i < args.Length; i++)
            {
                filename += args[i] + '-';
            }
            filename += args[0];
            // Write JSON file
            File.WriteAllText("results" + filename + ".json", json);
        }
    }
}