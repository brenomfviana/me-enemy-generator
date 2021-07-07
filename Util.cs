using System;
using System.Collections.Generic;

namespace OverlordEnemyGenerator
{
    /// This class holds only utility functions.
    public static class Util
    {
        /// Return a random integer percentage (i.e., from 0 to 100).
        public static int RandomPercent(ref Random rand)
        {
            return rand.Next(100);
        }

        /// Return a random integer number from the input inclusive range.
        public static int RandomInt(
            (int min, int max) range,
            ref Random rand
        ) {
            return rand.Next(range.min, range.max + 1);
        }

        /// Return a random float number from the input inclusive range.
        public static float RandomFloat(
            (float min, float max) range,
            ref Random rand
        ) {
            double n = rand.NextDouble();
            return (float) (n * (range.max - range.min) + range.min);
        }

        /// Return a random element from the input array.
        public static T RandomElementFromArray<T>(
            T[] range,
            ref Random rand
        ) {
            return range[rand.Next(0, range.Length)];
        }

        /// Return a random element from the input list.
        public static T RandomElementFromList<T>(
            List<T> range,
            ref Random rand
        ) {
            return range[rand.Next(0, range.Count)];
        }
    }
}