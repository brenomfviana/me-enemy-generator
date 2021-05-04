using System;

namespace OverlordEnemyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
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
                0,    // Random seed
                100,  // Number of generations
                20,   // Number of individuals of initial population
                5,    // Number of individuals of offspring
                10,   // Mutation chance
                90,   // Crossover chance
                space // The problem search space
            );

            // Generate a set of enemies
            Evolution.Evolve(p).Debug();
        }
    }
}
