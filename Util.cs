using System;
using System.Collections.Generic;

namespace EnemyGenerator
{
    /// This class holds only utility functions and constants.
    public class Util
    {
        /// This constant defines unknown references.
        public readonly static int UNKNOWN = -1;
        /// This constant defines the error message of not enough competitors.
        public readonly static string NOT_ENOUGH_COMPETITORS = "There are " +
            "not enough individuals in the input population to perform this " + 
            "operation.";
        /// This constant defines the error message of not enough competitors.
        public readonly static string CANNOT_COMPARE_INDIVIDUALS = "There is " +
            "no way of comparing two null individuals.";

        /// Return a random integer percentage (i.e., from 0 to 100).
        public static int RandomPercent(
            ref Random _rand
        ) {
            return _rand.Next(100);
        }

        /// Return a random integer number from the input inclusive range.
        public static int RandomInt(
            (int min, int max) _range,
            ref Random _rand
        ) {
            return _rand.Next(_range.min, _range.max + 1);
        }

        /// Return a random float number from the input inclusive range.
        public static float RandomFloat(
            (float min, float max) _range,
            ref Random _rand
        ) {
            double n = _rand.NextDouble();
            return (float) (n * (_range.max - _range.min) + _range.min);
        }

        /// Return a random element from the input array.
        public static T RandomElementFromArray<T>(
            T[] _range,
            ref Random _rand
        ) {
            return _range[_rand.Next(0, _range.Length)];
        }

        /// Return a random element from the input list.
        public static T RandomElementFromList<T>(
            List<T> _range,
            ref Random _rand
        ) {
            return _range[_rand.Next(0, _range.Count)];
        }
    }
}