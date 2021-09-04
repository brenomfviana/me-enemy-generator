/// This program is an Enemy Generator that evolves enemies via a MAP-Elites GA.
/// The result of this program is a set of enemies written in JSON files.
///
/// ATTENTION: This program was designed for the game prototype of the Game
///            Research Group of Universidade de São Paulo (USP). Therefore, the
///            designed enemy representation and difficulty function may not
///            work for other games.
///
/// This enemy generator receives five arguments:
/// - [Optional] save separately flag (-s);
///   * if the flag -s is inputted, then only the enemies of the final
///     population will be saved.
/// - a random seed;
/// - the number of generations;
/// - the initial population size;
/// - the mutation chance, and;
/// - the crossover chance.
///
/// Author: Breno M. F. Viana.

using System;

namespace EnemyGenerator
{
    class Program
    {
        /// The expected number of parameters (arguments).
        private const int NUMBER_OF_PARAMETERS = 5;

        /// Write all enemies separately flag.
        private const string SAVE_SEPARATELY = "-s";

        /// Error code for bad arguments.
        private const int ERROR_BAD_ARGUMENTS = 0xA0;

        static void Main(string[] args)
        {
            // Check if the expected number of parameters were inputted
            if (args.Length < NUMBER_OF_PARAMETERS)
            {
                Console.WriteLine("ERROR: Invalid number of parameters!");
                System.Environment.Exit(ERROR_BAD_ARGUMENTS);
            }

            // If the first argument is the flag save separately
            bool separately = args[0] == SAVE_SEPARATELY;
            int i = separately ? 1 : 0;

            // Define the evolutionary parameters
            Parameters prs = new Parameters(
                int.Parse(args[i++]),            // Random seed
                int.Parse(args[i++]),            // Number of generations
                int.Parse(args[i++]),            // Initial population size
                int.Parse(args[i++]),            // Mutation chance
                int.Parse(args[i])               // Crossover chance
            );

            // Prepare the evolutionary process
            EnemyGenerator generator = new EnemyGenerator(prs);

            // Start the generative process and generate a set of enemies
            generator.Evolve();

            // When the enemy generation process is over, then write the
            // results and the collected data
            Output.WriteData(generator.GetData(), separately);
        }
    }
}