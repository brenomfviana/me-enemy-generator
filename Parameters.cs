namespace OverlordEnemyGenerator
{
    /// This struct defines the parameters of the evolutionary enemy generator.
    public struct Parameters
    {
        // The seed that initializes the random generator.
        public int seed;
        // The maximun number of generations of the evolutionary process.
        public int generations;
        // The number of random initial individuals.
        public int rIndividuals;
        // The number of new individuals to be created each generation.
        public int offspring;
        // The chance of mutation.
        public int mutation;
        // The chance of crossover.
        public int crossover;
        // The delimitations of the search space.
        public SpaceSearch space;
    }

    /// This struct defines search space accordingly to the enemy's attributes.
    public struct SpaceSearch
    {
        public (int, int) health;
        public (int, int) strength;
        public MovementType[] movement;
        public (float, float) movementSpeed;
        public WeaponType[] weapon;
        public (float, float) attackSpeed;
        public BehaviorType[] behavior;
        public (float, float) activeTime;
        public (float, float) restTime;
        public ProjectileType[] projectile;
        public (float, float) projectileSpeed;
    }
}