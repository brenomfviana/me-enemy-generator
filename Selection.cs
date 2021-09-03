using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace EnemyGenerator
{
    /// Alias for the coordinate of MAP-Elites matrix.
    using Coordinate = System.ValueTuple<int, int>;

    /// This class holds the selector operator.
    public class Selection
    {
        /// The number of competitors of the Tournament Selection operator.
        private const int COMPETITORS = 3;

        /// The unknown coordinate.
        private const int UNKNOWN = -1;

        /// Select individuals from the MAP-Elites population.
        ///
        /// This function ensures that the same individual will not be selected
        /// for the same selection process. To do so, we use an auxiliary list
        /// composed of the individuals' coordinates in the MAP-Elites
        /// population. Instead of selecting directly an individual, we select
        /// its coordinate from the auxiliary list and remove it then it is not
        /// available for the next selection.
        public static Individual[] Select(
            int amount,
            Population pop,
            ref Random rand
        ) {
            // List of Elites' coordinates
            List<Coordinate> cs = pop.GetElitesCoordinates();
            // Ensure the population size is enough
            Debug.Assert(
                cs.Count - amount > COMPETITORS,
                "There are no enough individuals in the input population to " +
                "perform this operation."
            );

            // Select `amount` individuals
            Individual[] individuals = new Individual[amount];
            for (int i = 0; i < amount; i++)
            {
                // Perform tournament selection with 3 competitors
                (Coordinate coordinate, Individual individual) = Tournament(
                    COMPETITORS, // Number of competitors
                    pop,         // Population
                    cs,          // List of valid coordinates
                    ref rand     // Random number generator
                );
                // Select a new individual
                individuals[i] = individual;
                // Remove selected individual from available coordinates
                cs.Remove(coordinate);
            }

            // Return all selected individuals
            return individuals;
        }

        /// Perform tournament selection of a single individual.
        ///
        /// This function ensures that the same individual will not be selected
        /// for the same tournament selection process. To do so, we apply the
        /// same process explained in `Select` function.
        static (Coordinate, Individual) Tournament(
            int amount,
            Population pop,
            List<Coordinate> cs,
            ref Random rand
        ) {
            // List of available competitors
            List<Coordinate> acds = new List<Coordinate>(cs);
            // Initialize the list of competitors
            Individual[] competitors = new Individual[amount];
            // Initialize competitors' coordinates
            Coordinate[] coordinates = new Coordinate[amount];

            // Select competitors
            for (int i = 0; i < amount; i++)
            {
                // Get a random coordinate
                (int x, int y) rc = Util.RandomElementFromList(acds, ref rand);
                // Get the corresponding competitor
                competitors[i] = pop.map[rc.x, rc.y];
                coordinates[i] = rc;
                // Remove competitors from available competitors
                acds.Remove(rc);
            }

            // Find the tournament winner and its coordinate
            Individual winner = null;
            Coordinate coordinate = (UNKNOWN, UNKNOWN);
            for (int i = 0; i < amount; i++)
            {
                if (winner is null || competitors[i].fitness > winner.fitness)
                {
                    winner = competitors[i];
                    coordinate = coordinates[i];
                }
            }

            // Return the tournament winner and its coordinate
            return (coordinate, winner);
        }
    }
}