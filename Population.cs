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
    public class Population
    {
        /// The MAP-Elites dimension. The dimension is defined by the number of
        /// difficulty factors multiplied by the number of weapon Types.
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

        /// Add an individual in the MAP-Elites population.
        ///
        /// First, we identify which Elite the individual is classified in.
        /// Then, if the corresponding Elite is empty, the individual is placed
        /// there. Otherwise, we compare the both old and new individuals, and
        /// the best individual is placed in the corresponding Elite.
        public void PlaceIndividual(
            Individual individual
        ) {
            // Calculate the individual slot (Elite)
            int d = SearchSpace.GetDifficultyIndex(individual.difficulty);
            int w = (int) individual.weapon.weaponType;
            // Place individual in the MAP-Elites population if...
            if (d != -1 && // The new individual's difficulty is valid, and...
                // The new individual deserves to survive
                (map[d, w] is null || individual.fitness < map[d, w].fitness)
            ) {
                map[d, w] = individual;
            }
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

        /// Return a list with the individuals.
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

        /// Print the individuals of the MAP-Elites population.
        public void Debug()
        {
            for (int d = 0; d < dimension.difficulty; d++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    // Print the Elite's features
                    string log = "Elite ";
                    log += SearchSpace.AllDifficulties()[d] + "-";
                    log += ((WeaponType) w);
                    Console.WriteLine(log);
                    // Print empty if the Elite is null
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