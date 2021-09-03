using System;

namespace EnemyGenerator
{
    /// This class holds all the fitness-related functions.
    public class Fitness
    {
        /// Calculate the fitness value of the input individual.
        public static void Calculate(
            ref Individual individual
        ) {
            // Get the index of the corresponding difficulty range
            int d = SearchSpace.GetDifficultyIndex(individual.difficulty);
            if (d != -1)
            {
                // Calculate the mean of the difficulty range
                (float min, float max) df = SearchSpace.AllDifficulties()[d];
                float goal = (df.min + df.max) / 2;
                // Calculate the individual fitness
                individual.fitness = Math.Abs(goal - individual.difficulty);
            }
        }
    }
}