using System;

namespace OverlordEnemyGenerator
{
    public class Evolution
    {
        public static void Evolve(Parameters p)
        {
            // Initialize the random generator
            Random rand = new Random(p.seed);
            // Generate initial population
            for (int i = 0; i < p.rIndividuals; i++)
            {
                Individual ind = Individual.GetRandomIndividual(rand, p.space);
                Console.WriteLine(ind.enemy.health);
                Console.WriteLine(ind.enemy.strength);
                Console.WriteLine(ind.enemy.attackSpeed);
                Console.WriteLine(ind.enemy.movementType);
                Console.WriteLine(ind.enemy.movementSpeed);
                Console.WriteLine(ind.enemy.behaviorType);
                Console.WriteLine(ind.enemy.activeTime);
                Console.WriteLine(ind.enemy.restTime);
                Console.WriteLine(ind.weapon.weaponType);
                Console.WriteLine(ind.weapon.projectileType);
                Console.WriteLine(ind.weapon.projectileSpeed);
                Console.WriteLine();
            }
            // Run the generations
            for (int i = 0; i < p.generations; i++)
            {
                Console.WriteLine("Generation: " + i);
                // Apply the evolutionary operators
                if (p.crossover > rand.Next(100))
                {
                    // TODO: Crossover
                    Console.WriteLine("Crossover");
                    if (p.mutation > rand.Next(100))
                    {
                        // TODO: Mutation
                        Console.WriteLine("Mutation");
                    }
                } else {
                    // TODO: Mutation
                    Console.WriteLine("Mutation");
                }
                // TODO: Place in MAP-Elites
            }
        }
    }
}