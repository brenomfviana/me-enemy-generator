using System;
using System.Diagnostics;

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

        /// Return true if the first individual (`_i1`) is best than the second
        /// (`_i2`), and false otherwise.
        ///
        /// The best is the individual that is closest to the local goal in the
        /// MAP-Elites population. This is, the best is the one that's fitness
        /// has the lesser value.
        public static bool IsBest(
            Individual _i1,
            Individual _i2
        ) {
            // Ensure that both enemies are not null.
            Debug.Assert(
                _i1 != null || _i2 != null,
                Util.CANNOT_COMPARE_INDIVIDUALS
            );
            // If `_i1` is null, then `_i2` is the best individual
            if (_i1 == null)
            {
                return false;
            }
            // If `_i2` is null, then `_i1` is the best individual
            if (_i2 == null)
            {
                return true;
            }
            // Check which individual is the best
            return _i2.fitness > _i1.fitness;
        }
    }
}