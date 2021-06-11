using System;

namespace OverlordEnemyGenerator
{
    class Program
    {
        private const int ERROR_BAD_ARGUMENTS = 0xA0;

        /// This program gets five arguments.
        ///
        /// `0` - Random seed.
        /// `1` - Number of generations.
        /// `2` - Initial population size.
        /// `3` - Offspring size.
        /// `4` - Desired difficulty.
        static void Main(string[] args)
        {
            // Check if the number of parameters are valid
            if (args.Length != 5) {
                Console.WriteLine("ERROR: Invalid number of parameters!");
                System.Environment.Exit(ERROR_BAD_ARGUMENTS);
            }

            // Initialize the struct data to hold the generation process data
            Data data = new Data();

            // Get random seed
            data.seed = int.Parse(args[0]);
            // Get number of generations
            data.generations = int.Parse(args[1]);
            // Get initial population size
            data.initialPopSize = int.Parse(args[2]);
            // Get offspring size
            data.offspringSize = int.Parse(args[3]);
            // Get desired difficulty
            data.goal = float.Parse(args[4]);

            // Define the chances of each recombination operator
            int mutation = 20;
            int crossover = 80;

            // Define the search space
            SearchSpace space = new SearchSpace(
                (1, 5),                           // Health
                (1, 4),                           // Strength
                (0.75f, 4f),                      // Attack Speed
                SearchSpace.AllMovementTypes(),   // Movement Types
                (0.8f, 3.2f),                     // Movement Speed
                (1.5f, 10f),                      // Active Time
                (0.3f, 1.5f),                     // Rest Time
                SearchSpace.AllWeaponTypes(),     // Weapon Types
                SearchSpace.AllProjectileTypes(), // Projectile Types
                (1f, 4f)                          // Projectile Speed
            );

            // Define evolutionary parameters
            Parameters p = new Parameters(
                data.seed,           // Random seed
                data.generations,    // Number of generations
                data.initialPopSize, // Initial population size
                data.offspringSize,  // Offspring size
                mutation,            // Mutation chance
                crossover,           // Crossover chance
                data.goal,           // Desired difficulty
                space                // The problem search space
            );

            // Get starting time
            DateTime start = DateTime.Now;

            // Generate a set of enemies
            Evolution.Evolve(p, ref data);

            // Get ending time
            DateTime end = DateTime.Now;

            // Get the duration time
            data.duration = (end - start).TotalSeconds;

            // Write the collected data
            Output.WriteData(data);
        }
    }
}