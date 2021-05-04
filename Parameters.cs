using System;

namespace OverlordEnemyGenerator
{
    /// This struct defines the parameters of the evolutionary enemy generator.
    public struct Parameters
    {
        // The seed that initializes the random generator.
        public int seed { get; }
        // The maximun number of generations of the evolutionary process.
        public int generations { get; }
        // The number of random initial individuals.
        public int initial { get; }
        // The number of new individuals to be created each generation.
        public int offspring { get; }
        // The chance of mutation.
        public int mutation { get; }
        // The chance of crossover.
        public int crossover { get; }
        // The delimitations of the search space.
        public SearchSpace space { get; }

        /// Parameters constructor.
        public Parameters(
            int seed,
            int generations,
            int initial,
            int offspring,
            int mutation,
            int crossover,
            SearchSpace space
        ) {
            this.seed = seed;
            this.generations = generations;
            this.initial = initial;
            this.offspring = offspring;
            this.mutation = mutation;
            this.crossover = crossover;
            this.space = space;
        }
    }
}