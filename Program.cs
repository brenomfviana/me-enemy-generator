/// This program is an Enemy Generator that evolves enemies via a MAP-Elites
/// Genetic Algorithm. The output of this program is a set of levels written in
/// JSON files.
///
/// ATTENTION: This program was designed for the game prototype of the Game
///            Research Group of Universidade de São Paulo (USP). Therefore, the
///            designed enemy representation and difficulty function may not
///            work for other games.
///
/// This program can perform two tasks: generate a set of enemies and calculate
/// the difficulty of an entered enemy.
///
/// To generate enemies, this program receives seven arguments:
/// - [Optional] the separately save flag (`-s`);
///   * if the flag `-s` is entered, then the enemies on the final population
///     will be written separately, each one in a single JSON file.
/// - a random seed;
/// - the number of generations;
/// - the initial population size;
/// - the mutation chance;
/// - the crossover chance, and;
/// - the number of tournament competitors.
///
/// To calculate the difficulty of a single enemy, this program receives two
/// arguments:
/// - the difficulty calculation flag (`-d`);
/// - the JSON file path of the enemy.
///
/// Author: Breno M. F. Viana.

using System;

namespace EnemyGenerator
{
    class Program
    {
        /// The minimum number of parameters (arguments) of the enemy generator.
        private readonly static int ENE_GEN_MIN_NUM_PARAMS = 6;
        /// The minimum number of parameters (arguments) of the enemy generator.
        private readonly static int ENE_GEN_MAX_NUM_PARAMS = 7;
        /// The number of parameters (arguments) of the difficulty calculator.
        private readonly static int DIF_CALC_NUM_PARAMS = 2;

        /// Flag of saving all enemies separately.
        private readonly static string SAVE_SEPARATELY = "-s";
        /// Flag of difficulty calculation.
        private readonly static string DIFFICULTY_CALCULATION = "-d";

        /// Error code for bad arguments.
        private readonly static int ERROR_BAD_ARGUMENTS = 0xA0;

        /// Run the enemy generator.
        public static void Generator(
            string[] _args,
            bool separately
        ) {
            // If the flag was entered, then the parameters are the next
            int i = separately ? 1 : 0;
            // Define the evolutionary parameters
            Parameters prs = new Parameters(
                int.Parse(_args[i++]), // Random seed
                int.Parse(_args[i++]), // Number of generations
                int.Parse(_args[i++]), // Initial population size
                int.Parse(_args[i++]), // Mutation chance
                int.Parse(_args[i++]), // Crossover chance
                int.Parse(_args[i])    // Number of tournament competitors
            );
            // Prepare the evolutionary process
            EnemyGenerator generator = new EnemyGenerator(prs);
            // Start the generative process and generate a set of enemies
            generator.Evolve();
            // When the enemy generation process is over, then write the
            // results and the collected data
            Output.WriteData(generator.GetData(), separately);
        }

        /// Run the difficulty calculator.
        public static void DifficultyCalculator(
            string path
        ) {
            Individual individual = Input.ReadJSON(path);
            Difficulty.Calculate(ref individual);
            Console.WriteLine("Difficulty = " + individual.difficulty);
        }

        /// Print error message.
        public static void ErrorMessage()
        {
            Console.WriteLine("ERROR: The entered arguments are invalid!");
            System.Environment.Exit(ERROR_BAD_ARGUMENTS);
        }

        static void Main(
            string[] _args
        ) {
            if (_args.Length == ENE_GEN_MIN_NUM_PARAMS)
            {
                // Generate and print all results in a single file
                Generator(_args, false);
            }
            else if (_args.Length == ENE_GEN_MAX_NUM_PARAMS)
            {
                if (_args[0] == SAVE_SEPARATELY)
                {
                    // Generate and print the final result in different files
                    Generator(_args, true);
                }
                else
                {
                    ErrorMessage();
                }
            }
            else if (_args.Length == Program.DIF_CALC_NUM_PARAMS)
            {
                if (_args[0] == DIFFICULTY_CALCULATION)
                {
                    // Calculate the difficulty of the entered individual
                    DifficultyCalculator(_args[1]);
                }
                else
                {
                    ErrorMessage();
                }
            }
            else
            {
                Console.WriteLine("ERROR: Invalid number of parameters!");
                System.Environment.Exit(ERROR_BAD_ARGUMENTS);
            }
        }
    }
}