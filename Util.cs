using System;

namespace OverlordEnemyGenerator
{
    public class Util
    {
        /// Return a random integer number from the given range and the given 
        /// random number generator.
        public static int RandomInt(
            (int min, int max) range,
            Random rand
        ) {
            return rand.Next(range.min, range.max + 1);
        }

        /// Return a random float number from the given range and the given 
        /// random number generator.
        public static float RandomFloat(
            (float min, float max) range,
            Random rand
        ) {
            double n = rand.NextDouble();
            return (float) (n * (range.max - range.min) + range.min);
        }

        /// Return a random enumerate value from the given list and the given 
        /// random number generator.
        public static T RandomEnum<T>(
            T[] range,
            Random rand
        ) {
            return range[rand.Next(0, range.Length)];
        }
    }
}