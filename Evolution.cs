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
                //
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