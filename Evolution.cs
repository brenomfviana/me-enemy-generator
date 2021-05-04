using System;
using System.Collections.Generic;

namespace OverlordEnemyGenerator
{
    /// This class holds the evolutionary enemy generation algorithm.
    public class Evolution
    {
        /// Generate and return a set of enemies.
        public static Population Evolve(
            Parameters p
        ) {
            // Initialize the random generator
            Random rand = new Random(p.seed);
            // Initialize the MAP-Elites matrix
            Population pop = new Population(
                SearchSpace.AllBehaviorTypes().Length,
                SearchSpace.AllWeaponTypes().Length
            );
            // Generate the initial population
            for (int i = 0; i < p.initial; i++)
            {
                // Generate a new random individual and calculate its fitness
                Individual individual = Individual.GetRandom(rand, p.space);
                individual.fitness = Fitness.Calculate(individual);
                // Place the individual in the MAP-Elites
                pop.PlaceIndividual(individual);
            }
            // Run the generations
            for (int g = 0; g < p.generations; g++)
            {
                // Initialize the offspring list
                List<Individual> offspring = new List<Individual>();
                // Generate offspring
                while (offspring.Count < p.offspring)
                {
                    // Apply the evolutionary operators
                    if (p.crossover > rand.Next(100))
                    {
                        // Select two different parents
                        Individual[] parents = Operators.Select(2, pop, rand);
                        // Apply crossover and get the resulting children
                        Individual[] children = Operators.Crossover(
                            parents[0],
                            parents[1],
                            rand
                        );
                        // Check if the new individuals will suffer mutation
                        if (p.mutation > rand.Next(100))
                        {
                            for (int i = 0; i < children.Length; i++)
                            {
                                children[i] = Operators.Mutate(
                                    children[i],
                                    p.space,
                                    rand
                                );
                            }
                        }
                        // Add the new individuals in the offspring list
                        foreach (Individual individual in children)
                        {
                            offspring.Add(individual);
                        }
                    }
                    else
                    {
                        // Select and mutate a parent
                        Individual parent = Operators.Select(1, pop, rand)[0];
                        offspring.Add(Operators.Mutate(parent, p.space, rand));
                    }
                }
                // Place the offspring in MAP-Elites
                foreach (Individual individual in offspring)
                {
                    pop.PlaceIndividual(individual);
                }
            }
            // Return the MAP-Elites population
            return pop;
        }
    }
}