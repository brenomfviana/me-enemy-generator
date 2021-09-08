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
        /// This string defines that all files will be searched.
        private static string SEARCH_FOR = "*";
        /// This string must be used to initialize empty strings or convert
        /// values of other types during concatenation.
        private static string EMPTY_STRING = "";
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
            Data _data,
            bool _separately
        ) {
            // Check if the user asked to save the files separately
            if (_separately)
            {
                // Save each generated enemy in different JSON files
                SaveDataSeparately(_data);
            }
            else
            {
                // Save all the collected data in a single file
                SaveData(_data);
            }
        }

        /// Return the folder name for the entered parameters.
        private static string GetFolderName(
            Data _data
        ) {
            string foldername = EMPTY_STRING;
            foldername += EMPTY_STRING + _data.generations + FILENAME_SEPARATOR;
            foldername += EMPTY_STRING + _data.initialPopSize;
            return foldername;
        }

        /// Return the number of files that are inside the entered folder and
        /// have the entered extension.
        private static int GetNumberOfFiles(
            string _folder,
            string _extension
        ) {
            return Directory.GetFiles(_folder, SEARCH_FOR + _extension).Length;
        }

        /// Save all the collected data in a single file.
        private static void SaveData(
            Data _data
        ) {
            // Create the folder 'results'
            Directory.CreateDirectory(DATA_FOLDER_NAME);
            // Convert list to formmated JSON string
            string jsonData = JsonSerializer.Serialize(_data, JSON_OPTIONS);
            // Create the base name for the entered parameters
            string basename = GetFolderName(_data);
            // Create the result folder for the entered parameters
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
            Individual _individual,
            string _basename
        ) {
            // Get the enemy type (weapon type) name
            string enemyType = _individual.weapon.weaponType.ToString();
            // Create a folder for the enemy type
            string folder = _basename + SEPARATOR + enemyType;
            Directory.CreateDirectory(folder);
            // Get the index of the corresponding difficulty range
            int df = SearchSpace.GetDifficultyIndex(_individual.difficulty);
            if (df != Util.UNKNOWN)
            {
                // Convert enemy to formmated JSON string
                string jsonData = JsonSerializer.Serialize(
                    _individual, JSON_OPTIONS
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
            Data _data
        ) {
            // Create the folder 'results'
            Directory.CreateDirectory(ENEMY_FOLDER_NAME);
            // Calculate the number of JSON files in the folder
            int count = Directory.GetDirectories(
                ENEMY_FOLDER_NAME, SEARCH_FOR, SearchOption.TopDirectoryOnly
            ).Length;
            // Create the result folder for the entered parameters
            string basename = ENEMY_FOLDER_NAME + SEPARATOR;
            basename += GetFolderName(_data) + FILENAME_SEPARATOR + count;
            Directory.CreateDirectory(basename);
            // Write each enemy in the final solution
            foreach (Individual _individual in _data.final)
            {
                // Ignore empty Elites
                if (_individual == null)
                {
                    continue;
                }
                SaveEnemy(_individual, basename);
            }
            // Remove the populations
            _data.initial = null;
            _data.intermediate = null;
            _data.final = null;
            // Convert list to formmated JSON string
            string jsonData = JsonSerializer.Serialize(_data, JSON_OPTIONS);
            // Set the JSON filename with the base name and the number of files
            string filename = basename + SEPARATOR + DATA_FILENAME + JSON;
            // Write the found data in JSON format
            File.WriteAllText(filename, jsonData);
        }
    }
}