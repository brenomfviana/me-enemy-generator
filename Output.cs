using System.IO;
using System.Text.Json;

namespace EnemyGenerator
{
    /// This class holds functions to write the evolutionary process' outputs.
    public class Output
    {
        /// The JSON extension.
        private static readonly string JSON = ".json";
        /// The operational system directory separator char.
        private static readonly char SEPARATOR = Path.DirectorySeparatorChar;
        /// The filename separator char.
        private static readonly char FILENAME_SEPARATOR = '-';
        /// The general target for the search for files.
        private static readonly string SEARCH_FOR = "*";
        /// String to initialize empty strings or convert values of other types
        /// during concatenation.
        private static readonly string EMPTY_STR = "";
        /// The default JSON options.
        private static readonly JsonSerializerOptions JSON_OPTIONS =
            new JsonSerializerOptions(){ WriteIndented = true };
        /// The results folder name.
        /// This folder stores the collected data to evaluate the approach.
        private static readonly string RESULTS_FOLDER_NAME = @"results";
        /// The enemies folder name.
        /// This folder stores the generated enemies.
        private static readonly string ENEMY_FOLDER_NAME = @"enemies";
        /// The filename of the evolutionary process data.
        /// This file is created only when the enemies are saved separately.
        private static readonly string DATA_FILENAME = @"data";

        /// Write the collected data from the evolutionary process.
        public static void WriteData(
            Data _data,
            bool _separately
        ) {
            if (_separately)
            {
                SaveDataSeparately(_data);
            }
            else
            {
                SaveData(_data);
            }
        }

        /// Create and return a folder name from the entered parameters.
        private static string GetFolderName(
            Data _data
        ) {
            Parameters prs = _data.parameters;
            string foldername = EMPTY_STR;
            foldername += EMPTY_STR + prs.generations + FILENAME_SEPARATOR;
            foldername += EMPTY_STR + prs.population + FILENAME_SEPARATOR;
            foldername += EMPTY_STR + prs.mutation + FILENAME_SEPARATOR;
            foldername += EMPTY_STR + prs.geneMutation + FILENAME_SEPARATOR;
            foldername += EMPTY_STR + prs.competitors;
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
            // Create folder to store the results
            Directory.CreateDirectory(RESULTS_FOLDER_NAME);
            // Convert the collected data to a formmated JSON string
            string jsonData = JsonSerializer.Serialize(_data, JSON_OPTIONS);
            // Create the base name according to the entered parameters
            string basename = GetFolderName(_data);
            // Create the result folder for the entered parameters
            string folder = RESULTS_FOLDER_NAME + SEPARATOR + basename;
            Directory.CreateDirectory(folder);
            // Calculate the number of JSON files in the folder
            // This operation aims to avoid overwrite created files
            int count = GetNumberOfFiles(folder, JSON);
            // Set the JSON filename with the base name and the number of files
            string filename = basename + FILENAME_SEPARATOR + count + JSON;
            // Write the found data in JSON format
            File.WriteAllText(folder + SEPARATOR + filename, jsonData);
        }

        /// Save each generated enemy in different JSON files.
        private static void SaveDataSeparately(
            Data _data
        ) {
            // Create the folder to store the enemies
            Directory.CreateDirectory(ENEMY_FOLDER_NAME);
            // Calculate the number of directories in the folder
            // This operation aims to avoid overwrite created folders
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
                if (_individual is null) { continue; }
                SaveEnemy(_individual, basename);
            }
            // Remove the populations from the collected data
            _data.initial = null;
            _data.intermediate = null;
            _data.final = null;
            // Convert the collected data to a formmated JSON string
            string jsonData = JsonSerializer.Serialize(_data, JSON_OPTIONS);
            // Write the found data in JSON format
            string filename = basename + SEPARATOR + DATA_FILENAME + JSON;
            File.WriteAllText(filename, jsonData);
        }

        /// Save a single enemy.
        private static void SaveEnemy(
            Individual _individual,
            string _basename
        ) {
            // Create a folder for the enemy type
            string type = _individual.weapon.weaponType.ToString();
            string folder = _basename + SEPARATOR + type;
            Directory.CreateDirectory(folder);
            // Check if the enemy has a valid difficulty
            int df = SearchSpace.GetDifficultyIndex(_individual.difficulty);
            if (df != Common.UNKNOWN)
            {
                // Convert enemy to formmated JSON string
                string jsonData = JsonSerializer.Serialize(
                    _individual, JSON_OPTIONS
                );
                // Calculate the expected difficulty
                (float, float) range = SearchSpace.AllDifficulties()[df];
                int d = (int) (range.Item2 + range.Item1) / 2;
                // Write the JSON file
                string filename = type + FILENAME_SEPARATOR + d + JSON;
                File.WriteAllText(folder + SEPARATOR + filename, jsonData);
            }
        }
    }
}