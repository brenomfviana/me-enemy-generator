using System;
using System.Collections.Generic;

namespace OverlordEnemyGenerator
{
    /// This class holds the evolutionary enemy generation algorithm.
    public class EnemyGenerator
    {
        /// The generator parameters.
        private Parameters prs;
        /// The found MAP-Elites population.
        private Population solution;
        /// The collected data.
        private Data data;

        /// Enemy Generator constructor.
        public EnemyGenerator(
            Parameters _prs
        ) {
            prs = _prs;
            // Initialize the data to be collected
            data = new Data();
            data.seed = prs.seed;
            data.generations = prs.generations;
            data.initialPopSize = prs.initial;
        }

        /// Return the collected data from the evolutionary process.
        public Data GetData()
        {
            return data;
        }

        /// Return the resulting MAP-Elites population.
        public Population GetSolution()
        {
            return solution;
        }

        /// Generate and return a set of enemies.
        public Population Evolve()
        {
            // Get starting time
            DateTime start = DateTime.Now;
            // Run evolutionary process
            Evolution();
            // Get ending time
            DateTime end = DateTime.Now;
            // Get the duration time
            data.duration = (end - start).TotalSeconds;
            // Return the found individuals
            return solution;
        }

        /// Perform the enemy evolution process.
        private void Evolution()
        {
            // Initialize the random generator
            Random rand = new Random(prs.seed);

            // Initialize the MAP-Elites matrix
            Population pop = new Population(
                SearchSpace.AllDifficulties().Length,
                SearchSpace.AllWeaponTypes().Length
            );
            // Generate the initial population
            while (pop.Count() < prs.initial)
            {
                // Create a new random individual
                Individual individual = Individual.GetRandom(prs.space, ref rand);
                // Calculate the individual's difficulty
                individual.CalculateDifficulty();
                // Calculate the individual fitness
                Fitness.Calculate(ref individual);
                // Place the individual in the MAP-Elites
                pop.PlaceIndividual(individual);
            }

            // Get the initial population
            data.initial = new List<Individual>(pop.ToList());

            // Run the generations
            for (int g = 0; g < prs.generations; g++)
            {
                // Initialize the offspring list
                List<Individual> offspring = new List<Individual>();

                // Apply the evolutionary operators
                if (prs.crossover > Util.RandomPercent(ref rand))
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
                        // Calculate the individual's difficulty
                        children[i].CalculateDifficulty();
                        // Calculate the new individual fitness
                        Fitness.Calculate(ref children[i]);
                        // Add the new individual in the offspring
                        offspring.Add(children[i]);
                    }
                }
                else
                {
                    // Select and mutate a parent
                    var parent = Operators.Select(1, pop, ref rand)[0];
                    Individual individual = Operators.Mutate(
                        parent, prs.space, prs.mutation, ref rand
                    );
                    // Calculate the individual's difficulty
                    individual.CalculateDifficulty();
                    // Calculate the new individual fitness
                    Fitness.Calculate(ref individual);
                    // Add the new individual in the offspring
                    offspring.Add(individual);
                }

                // Place the offspring in MAP-Elites
                foreach (Individual individual in offspring)
                {
                    individual.generation = g;
                    pop.PlaceIndividual(individual);
                }

                // Get the intermediate population
                if (g == (int) prs.generations / 2)
                {
                    data.intermediate = new List<Individual>(pop.ToList());
                }
            }

            // Get the final population (solution)
            solution = pop;

            // Get the final population (solution)
            data.solution = new List<Individual>(solution.ToList());
        }
    }
}