using System;
using System.IO;
using System.Text.Json;

namespace EnemyGenerator
{
    /// This class holds functions to write the evolutionary process' outputs.
    public class Output
    {
        /// The JSON extension.
        private static string JSON = ".json";
        /// The operational system directory separator char.
        private static char SEPARATOR = Path.DirectorySeparatorChar;
        /// The filename separator char.
        private static char FILENAME_SEPARATOR = '-';
        /// Define the JSON options.
        private static JsonSerializerOptions JSON_OPTIONS =
            new JsonSerializerOptions(){ WriteIndented = true };
        /// Results folder name.
        /// This folder saves the collected data to evaluate the approach.
        private static string DATA_FOLDER_NAME = @"results";
        /// Enemies folder name.
        /// This folder saves the generated enemies.
        private static string ENEMY_FOLDER_NAME = @"enemies";
        /// Data filename.
        /// Created only when the enemies are saved separately.
        private static string DATA_FILENAME = @"data";

        /// Write the collected data from the evolutionary process.
        public static void WriteData(
            Data data,
            bool separately
        ) {
            // Check if the user asked to save the files separately
            if (separately)
            {
                // Save each generated enemy in different JSON files
                SaveDataSeparately(data);
            }
            else
            {
                // Save all the collected data in a single file
                SaveData(data);
            }
        }

        /// Return the folder name for the inputted parameters.
        private static string GetFolderName(
            Data data
        ) {
            string foldername = "";
            foldername += "" + data.generations + FILENAME_SEPARATOR;
            foldername += "" + data.initialPopSize;
            return foldername;
        }

        /// Return the number of files that are inside the inputted folder and
        /// have the inputted extension.
        private static int GetNumberOfFiles(
            string folder,
            string extension
        ) {
            return Directory.GetFiles(folder, "*" + extension).Length;
        }

        /// Save all the collected data in a single file.
        private static void SaveData(
            Data data
        ) {
            // Create the folder 'results'
            Directory.CreateDirectory(DATA_FOLDER_NAME);
            // Convert list to formmated JSON string
            string jsonData = JsonSerializer.Serialize(data, JSON_OPTIONS);
            // Create the base name for the inputted parameters
            string basename = GetFolderName(data);
            // Create the result folder for the inputted parameters
            string folder = DATA_FOLDER_NAME + SEPARATOR + basename;
            Directory.CreateDirectory(folder);
            // Calculate the number of JSON files in the folder
            int count = GetNumberOfFiles(folder, JSON);
            // Set the JSON filename with the base name and the number of files
            string filename = basename + FILENAME_SEPARATOR + count + JSON;
            // Write the found data in JSON format
            File.WriteAllText(folder + SEPARATOR + filename, jsonData);
        }

        /// Save a single enemy.
        private static void SaveEnemy(
            Individual individual,
            string basename
        ) {
            // Get the enemy type (weapon type) name
            string enemyType = individual.weapon.weaponType.ToString();
            // Create a folder for the enemy type
            string folder = basename + SEPARATOR + enemyType;
            Directory.CreateDirectory(folder);
            // Get the index of the corresponding difficulty range
            int df = SearchSpace.GetDifficultyIndex(individual.difficulty);
            if (df != -1)
            {
                // Convert enemy to formmated JSON string
                string jsonData = JsonSerializer.Serialize(
                    individual, JSON_OPTIONS
                );
                // Calculate the expected difficulty
                (float, float) range = SearchSpace.AllDifficulties()[df];
                int d = (int) (range.Item2 + range.Item1) / 2;
                // Prepare the JSON filename
                string filename = enemyType + FILENAME_SEPARATOR + d + JSON;
                // Write JSON file
                File.WriteAllText(folder + SEPARATOR + filename, jsonData);
            }
        }

        /// Save each generated enemy in different JSON files.
        private static void SaveDataSeparately(
            Data data
        ) {
            // Create the folder 'results'
            Directory.CreateDirectory(ENEMY_FOLDER_NAME);
            // Calculate the number of JSON files in the folder
            int count = Directory.GetDirectories(
                ENEMY_FOLDER_NAME, "*", SearchOption.TopDirectoryOnly
            ).Length;
            // Create the result folder for the inputted parameters
            string basename = ENEMY_FOLDER_NAME + SEPARATOR;
            basename += GetFolderName(data) + FILENAME_SEPARATOR + count;
            Directory.CreateDirectory(basename);
            // Write each enemy in the final solution
            foreach (Individual individual in data.final)
            {
                // Ignore empty Elites
                if (individual == null)
                {
                    continue;
                }
                SaveEnemy(individual, basename);
            }
            // Remove the populations
            data.initial = null;
            data.intermediate = null;
            data.final = null;
            // Convert list to formmated JSON string
            string jsonData = JsonSerializer.Serialize(data, JSON_OPTIONS);
            // Set the JSON filename with the base name and the number of files
            string filename = basename + SEPARATOR + DATA_FILENAME + JSON;
            // Write the found data in JSON format
            File.WriteAllText(filename, jsonData);
        }
    }
}