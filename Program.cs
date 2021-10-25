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
/// - the mutation chance of a single gene, and;
/// - the number of tournament competitors.
///
/// To calculate the difficulty of a single enemy, this program receives two
/// arguments:
/// - the difficulty calculation flag (`-d`);
/// - the JSON file path of the enemy.
///
/// Author: Breno M. F. Viana.

using System;
using System.Diagnostics;

namespace EnemyGenerator
{
    class Program
    {
        /// The minimum number of parameters (arguments) of the enemy generator.
        private static readonly int ENE_GEN_MIN_NUM_PARAMS = 6;
        /// The minimum number of parameters (arguments) of the enemy generator.
        private static readonly int ENE_GEN_MAX_NUM_PARAMS = 7;
        /// The number of parameters (arguments) of the difficulty calculator.
        private static readonly int DIF_CALC_NUM_PARAMS = 2;

        /// The flag of saving all enemies separately.
        private static readonly string SAVE_SEPARATELY = "-s";
        /// The flag of difficulty calculation.
        private static readonly string DIFFICULTY_CALCULATION = "-d";

        /// The error code for bad arguments.
        private static readonly int ERROR_BAD_ARGUMENTS = 0xA0;
        /// The error message of not enough population.
        public static readonly string TOO_MUCH_COMPETITORS =
            "The number of competitors is higher than the population size; " +
            "in this way, tournament selection is impossible.";
        /// The error message of not enough competitors.
        public static readonly string TOO_FEW_COMPETITORS =
            "The number of competitors is not enough for a tournament.";

        /// Run the enemy generator.
        public static void Generator(
            string[] _args,
            bool _separately
        ) {
            // If the flag was entered, then the parameters are the next
            int i = _separately ? 1 : 0;
            // Define the evolutionary parameters
            Parameters prs = new Parameters(
                int.Parse(_args[i++]), // Random seed
                int.Parse(_args[i++]), // Number of generations
                int.Parse(_args[i++]), // Initial population size
                int.Parse(_args[i++]), // Mutation chance
                int.Parse(_args[i++]), // Mutation chance of a single gene
                int.Parse(_args[i])    // Number of tournament competitors
            );
            // Ensure the population size is enough for the tournament
            Debug.Assert(
                prs.population >= prs.competitors,
                TOO_MUCH_COMPETITORS
            );
            // Ensure the number of competitors is valid
            Debug.Assert(
                prs.competitors > 1,
                TOO_FEW_COMPETITORS
            );
            // Run the generator and save the results and the collected data
            EnemyGenerator generator = new EnemyGenerator(prs);
            generator.Evolve();
            Output.WriteData(generator.Data, _separately);
        }

        /// Run the difficulty calculator.
        public static void DifficultyCalculator(
            string _path
        ) {
            Individual individual = Input.ReadIndividual(_path);
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