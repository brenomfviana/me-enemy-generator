using System;

namespace OverlordEnemyGenerator
{
    class Program
    {
       private const int ERROR_BAD_ARGUMENTS = 0xA0;

        static void Main(string[] args)
        {
            // Check if the number of parameters are valid
            if (args.Length != 4) {
                Console.WriteLine("ERROR: Invalid number of parameters!");
                System.Environment.Exit(ERROR_BAD_ARGUMENTS);
            }

            // Get the values of the parameters
            int arg0 = int.Parse(args[0]);
            int arg1 = int.Parse(args[1]);
            int arg2 = int.Parse(args[2]);
            int arg3 = int.Parse(args[3]);

            // Define the search space
            SearchSpace space = new SearchSpace(
                (1, 5),                           // Health
                (1, 4),                           // Strength
                (0.75f, 4f),                      // Attack Speed
                SearchSpace.AllMovementTypes(),   // Movement Types
                (0.8f, 3.2f),                     // Movement Speed
                SearchSpace.AllBehaviorTypes(),   // Behavior Types
                (1.5f, 10f),                      // Active Time
                (0.3f, 1.5f),                     // Rest Time
                SearchSpace.AllWeaponTypes(),     // Weapon Types
                SearchSpace.AllProjectileTypes(), // Projectile Types
                (1f, 4f)                          // Projectile Speed
            );

            // Define evolutionary parameters
            Parameters p = new Parameters(
                arg0, // Random seed
                arg1, // Number of generations
                arg2, // Number of individuals of the initial population
                arg3, // Number of individuals of offspring
                10,   // Mutation chance
                90,   // Crossover chance
                space // The problem search space
            );

            // Get starting time
            DateTime start = DateTime.Now;

            // Generate a set of enemies
            Population population = Evolution.Evolve(p);

            // Get ending time
            DateTime end = DateTime.Now;
            // Calculate duration time
            Double duration = (start - end).TotalSeconds;

            // Write the found solutions
            Output.WriteMAPElites(population, args);
        }
    }
}
