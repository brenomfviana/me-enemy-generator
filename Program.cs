using System;

namespace EnemyGenerator
{
    class Program
    {
        /// The expected number of parameters (arguments).
        private const int NUMBER_OF_PARAMETERS = 5;

        /// Error code for bad arguments.
        private const int ERROR_BAD_ARGUMENTS = 0xA0;

        /// This enemy generator receives five arguments:
        /// 0 - Random seed.
        /// 1 - Number of generations.
        /// 2 - Initial population size.
        /// 3 - Mutation chance.
        /// 4 - Crossover chance.
        static void Main(string[] args)
        {
            // Check if the expected number of parameters were inputted
            if (args.Length != NUMBER_OF_PARAMETERS)
            {
                Console.WriteLine("ERROR: Invalid number of parameters!");
                System.Environment.Exit(ERROR_BAD_ARGUMENTS);
            }

            // Define the search space
            SearchSpace space = new SearchSpace(
                (1, 5),                           // Health
                (1, 4),                           // Strength
                (0.75f, 4f),                      // Attack Speed
                SearchSpace.AllMovementTypes(),   // Movement Types
                (0.8f, 2.8f),                     // Movement Speed
                (1.5f, 10f),                      // Active Time
                (0.3f, 1.5f),                     // Rest Time
                SearchSpace.AllWeaponTypes(),     // Weapon Types
                (1f, 4f)                          // Projectile Speed
            );

            // Define the evolutionary parameters
            Parameters prs = new Parameters(
                int.Parse(args[0]), // Random seed
                int.Parse(args[1]), // Number of generations
                int.Parse(args[2]), // Initial population size
                int.Parse(args[3]), // Mutation chance
                int.Parse(args[4]), // Crossover chance
                space               // Problem search space
            );

            // Prepare the evolutionary process
            EnemyGenerator generator = new EnemyGenerator(prs);

            // Generate a set of enemies
            generator.Evolve();

            // Write the collected data
            Output.WriteData(generator.GetData());
        }
    }
}