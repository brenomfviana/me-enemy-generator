using System;

namespace OverlordEnemyGenerator
{
    class Program
    {
        private const int ERROR_BAD_ARGUMENTS = 0xA0;

        /// The program get five parameters.
        ///
        /// arg1 - Random seed.
        /// arg2 - Number of generations.
        /// arg3 - Number of individuals of the initial population.
        /// arg4 - Number of individuals of offspring.
        static void Main(string[] args)
        {
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

            // Initialize the struct data to hold the generation process data
            Data data = new Data();

            // Define evolutionary parameters
            Parameters p;
            if (args.Length == 0)
            {
                p = new Parameters(
                    0,    // Random seed
                    200,  // Number of generations
                    10,   // Initial population soze
                    1,    // Offspring size
                    30,   // Mutation chance
                    70,   // Crossover chance
                    25,   // Desired difficulty
                    space // The problem search space
                );
            }
            else
            {
                // Check if the number of parameters are valid
                if (args.Length != 5) {
                    Console.WriteLine("ERROR: Invalid number of parameters!");
                    System.Environment.Exit(ERROR_BAD_ARGUMENTS);
                }

                // Get random seed
                data.seed = int.Parse(args[1]);
                // Get number of generations
                data.generations = int.Parse(args[2]);
                // Get number of individuals of the initial population
                data.initialPopSize = int.Parse(args[3]);
                // Get number of individuals of offspring
                data.offspringSize = int.Parse(args[4]);

                // Define evolutionary parameters
                p = new Parameters(
                    data.seed,           // Random seed
                    data.generations,    // Number of generations
                    data.initialPopSize, // Initial population soze
                    data.offspringSize,  // Offspring size
                    30,                  // Mutation chance
                    70,                  // Crossover chance
                    25,                  // Desired difficulty
                    space                // The problem search space
                );
            }

            // Get starting time
            DateTime start = DateTime.Now;

            // Generate a set of enemies
            Evolution.Evolve(p, ref data);

            // Get ending time
            DateTime end = DateTime.Now;

            // Get the duration time
            data.duration = (end - start).TotalSeconds;

            // Write the collected data
            Output.WriteData(data, args);
        }
    }
}
