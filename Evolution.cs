using System;
using System.Collections.Generic;

namespace OverlordEnemyGenerator
{
    /// This class holds the evolutionary enemy generation algorithm.
    public class Evolution
    {
        /// Generate and return a set of enemies.
        public static Population Evolve(
            Parameters p,
            ref Data data
        ) {
            // Initialize the random generator
            Random rand = new Random(p.seed);
            // Initialize the MAP-Elites matrix
            Population pop = new Population(
                SearchSpace.AllDifficulties().Length,
                SearchSpace.AllWeaponTypes().Length
            );
            // Generate the initial population
            while (pop.Count() < p.initial)
            {
                // Create a new random individual
                Individual individual = Individual.GetRandom(p.space, ref rand);
                // Calculate the individual fitness
                Fitness.Calculate(ref individual);
                // Place the individual in the MAP-Elites
                pop.PlaceIndividual(individual);
            }
            // Get the initial population
            data.initial = new List<Individual>(pop.ToList());
            // Run the generations
            for (int g = 0; g < p.generations; g++)
            {
                // Initialize the offspring list
                List<Individual> offspring = new List<Individual>();
                // Generate offspring
                while (offspring.Count < p.offspring)
                {
                    // Apply the evolutionary operators
                    if (p.crossover > Util.RandomPercent(ref rand))
                    {
                        // Select two different parents
                        Individual[] parents = Operators.Select(
                            2, pop, ref rand
                        );
                        // Apply crossover and get the resulting children
                        Individual[] children = Operators.Crossover(
                            parents[0],
                            parents[1],
                            ref rand
                        );
                        // Add the new individuals in the offspring list
                        for (int i = 0; i < children.Length; i++)
                        {
                            // Calculate the new individual fitness
                            Fitness.Calculate(ref children[i]);
                            // Add the new individual in the offspring
                            offspring.Add(children[i]);
                        }
                    }
                    else
                    {
                        // Select and mutate a parent
                        Individual parent = Operators.Select(
                            1, pop, ref rand
                        )[0];
                        Individual individual = Operators.Mutate(
                            parent, p.space, p.mutation, ref rand
                        );
                        // Calculate the new individual fitness
                        Fitness.Calculate(ref individual);
                        // Add the new individual in the offspring
                        offspring.Add(individual);
                    }
                }
                // Place the offspring in MAP-Elites
                foreach (Individual individual in offspring)
                {
                    individual.generation = g;
                    pop.PlaceIndividual(individual);
                }
                // Get the intermediate population
                if (g == (int) p.generations / 2)
                {
                    data.intermediate = new List<Individual>(pop.ToList());
                }
            }
            // Get the final population (solution)
            data.solution = new List<Individual>(pop.ToList());
            // Return the MAP-Elites population
            return pop;
        }
    }
}