namespace OverlordEnemyGenerator
{
    /// This struct defines the parameters of the evolutionary enemy generator.
    public struct Parameters
    {
        // The seed that initializes the random generator.
        public int seed { get; }
        // The maximun number of generations.
        public int generations { get; }
        // The initial population size.
        public int initial { get; }
        // The offspring size.
        public int offspring { get; }
        // The chance of mutation.
        public int mutation { get; }
        // The chance of crossover.
        public int crossover { get; }
        // The fitness goal.
        public float goal { get; }
        // The search space.
        public SearchSpace space { get; }

        /// Parameters constructor.
        public Parameters(
            int seed,
            int generations,
            int initial,
            int offspring,
            int mutation,
            int crossover,
            float goal,
            SearchSpace space
        ) {
            this.seed = seed;
            this.generations = generations;
            this.initial = initial;
            this.offspring = offspring;
            this.mutation = mutation;
            this.crossover = crossover;
            this.goal = goal;
            this.space = space;
        }
    }
}
