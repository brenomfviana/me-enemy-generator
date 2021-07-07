namespace OverlordEnemyGenerator
{
    /// This struct holds the parameters of the evolutionary enemy generator.
    public struct Parameters
    {
        /// The seed that initializes the random generator.
        public int seed { get; }
        /// The maximun number of generations.
        public int generations { get; }
        /// The initial population size.
        public int initial { get; }
        /// The mutation chance.
        public int mutation { get; }
        /// The crossover chance.
        public int crossover { get; }
        /// The search space.
        public SearchSpace space { get; }

        /// Parameters constructor.
        public Parameters(
            int _seed,
            int _generations,
            int _initial,
            int _mutation,
            int _crossover,
            SearchSpace _space
        ) {
            seed = _seed;
            generations = _generations;
            initial = _initial;
            mutation = _mutation;
            crossover = _crossover;
            space = _space;
        }
    }
}
