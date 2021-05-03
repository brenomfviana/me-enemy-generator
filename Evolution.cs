using System;

namespace OverlordEnemyGenerator
{
    public class Evolution
    {
        public static Population Evolve(
            Parameters p
        ) {
            // Initialize the random generator
            Random rand = new Random(p.seed);
            // Initialize the MAP-Elites population
            Population population = new Population(
                SearchSpace.AllBehaviorTypes().Length,
                SearchSpace.AllWeaponTypes().Length
            );
            // Generate initial population
            for (int i = 0; i < p.rIndividuals; i++)
            {
                // Generate a new random individual
                Individual individual = Individual.GetRandom(rand, p.space);
                // TODO: Calculates the individual fitness
                // Place the new individual in the MAP-Elites
                population.PlaceIndividual(individual);
            }
            population.Debug();
            // Run the generations
            for (int i = 0; i < p.generations; i++)
            {
                // Apply the evolutionary operators
                if (p.crossover > rand.Next(100))
                {
                    // TODO: Crossover
                    if (p.mutation > rand.Next(100))
                    {
                        // TODO: Mutation
                    }
                }
                else
                {
                    // TODO: Mutation
                }
                // TODO: Place in MAP-Elites
            }
            // Return the MAP-Elites population
            return population;
        }
    }
}