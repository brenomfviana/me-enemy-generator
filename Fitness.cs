using System;

namespace EnemyGenerator
{
    /// This class holds all the fitness-related functions.
    public class Fitness
    {
        /// Calculate the fitness value of the input individual.
        public static void Calculate(
            ref Individual _individual
        ) {
            // Get the index of the corresponding difficulty range
            int d = SearchSpace.GetDifficultyIndex(_individual.difficulty);
            if (d != Util.UNKNOWN)
            {
                // Calculate the mean of the difficulty range
                (float min, float max) df = SearchSpace.AllDifficulties()[d];
                float goal = (df.min + df.max) / 2;
                // Calculate the individual fitness
                _individual.fitness = Math.Abs(goal - _individual.difficulty);
            }
        }
    }
}