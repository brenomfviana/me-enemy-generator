using System;
using System.Collections.Generic;

namespace EnemyGenerator
{
    /// Alias for the coordinate of MAP-Elites matrix.
    using Coordinate = System.ValueTuple<int, int>;

    /// This struct represents a MAP-Elites population.
    ///
    /// The MAP-Elites population is an N-dimensional array of individuals,
    /// where each matrix's ax corresponds to a different feature.
    ///
    /// This particular population is mapped into enemy's weapons and its
    /// difficulty factor. Each Elite (or matrix cell) corresponds to a
    /// combination of different types of weapons and difficulty factors.
    public struct Population
    {
        /// The MAP-Elites dimension. The dimension is defined by the number of
        /// difficulty factors multiplied by the number of weapon types.
        public (int difficulty, int weapon) dimension { get; }
        /// The MAP-Elites map (a matrix of individuals).
        public Individual[,] map { get; }

        /// Population constructor.
        public Population(
            int _numberOfDifficultyFactors,
            int _numbefOfWeaponTypes
        ) {
            dimension = (_numberOfDifficultyFactors, _numbefOfWeaponTypes);
            map = new Individual[dimension.difficulty, dimension.weapon];
        }

        /// Return the number of Elites of the population.
        public int Count()
        {
            int count = 0;
            for (int d = 0; d < dimension.difficulty; d++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    if (!(map[d, w] is null))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// Return a list corresponding to the Elites coordinates.
        public List<Coordinate> GetElitesCoordinates()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int d = 0; d < dimension.difficulty; d++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    if (!(map[d, w] is null))
                    {
                        coordinates.Add((d, w));
                    }
                }
            }
            return coordinates;
        }

        /// Add an individual in the MAP-Elites population.
        ///
        /// First, we identify which Elite the individual is classified in.
        /// Then, if the corresponding Elite is empty, the individual is placed
        /// there. Otherwise, we compare the both old and new individuals, and
        /// the best individual is placed in the corresponding Elite.
        public void PlaceIndividual(
            Individual _individual
        ) {
            // Calculate the individual slot (Elite)
            int d = SearchSpace.GetDifficultyIndex(_individual.difficulty);
            int w = (int) _individual.weapon.weaponType;
            // If the new individual's difficulty is known, and...
            if (d != Util.UNKNOWN) {
                // If the new individual deserves to survive
                if (Fitness.IsBest(_individual, map[d, w]))
                {
                    // Then, place the individual in the MAP-Elites population
                    map[d, w] = _individual;
                }
            }
        }

        /// Return a list with the population individuals.
        public List<Individual> ToList()
        {
            List<Individual> list = new List<Individual>();
            for (int d = 0; d < dimension.difficulty; d++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    list.Add(map[d, w]);
                }
            }
            return list;
        }

        /// Print all the individuals of the MAP-Elites population.
        public void Debug()
        {
            for (int d = 0; d < dimension.difficulty; d++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    // Print the Elite's coordinate
                    string log = "Elite ";
                    log += SearchSpace.AllDifficulties()[d] + "-";
                    log += ((WeaponType) w);
                    Console.WriteLine(log);
                    // Print "Empty" if the Elite is null
                    if (map[d, w] is null)
                    {
                        Console.WriteLine("  Empty");
                    }
                    // Print the Elite's attributes
                    else
                    {
                        map[d, w].Debug();
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}