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
        /// Select individuals from the MAP-Elites population.
        ///
        /// This function ensures that the same individual will not be selected
        /// for the same selection process. To do so, we use an auxiliary list
        /// composed of the individuals' coordinates in the MAP-Elites
        /// population. Instead of selecting directly an individual, we select
        /// its coordinate from the auxiliary list and remove it then it is not
        /// available for the next selection.
        public static Individual[] Select(
            int _amount,
            int _competitors,
            Population _pop,
            ref Random _rand
        ) {
            // List of Elites' coordinates
            List<Coordinate> cs = _pop.GetElitesCoordinates();
            // Ensure the population size is enough
            Debug.Assert(
                cs.Count - _amount > _competitors, Util.NOT_ENOUGH_COMPETITORS
            );
            // Select `_amount` individuals
            Individual[] individuals = new Individual[_amount];
            for (int i = 0; i < _amount; i++)
            {
                // Perform tournament selection with `_competitors` competitors
                (Coordinate coordinate, Individual individual) = Tournament(
                    _competitors, // Number of competitors
                    _pop,         // Population
                    cs,           // List of valid coordinates
                    ref _rand     // Random number generator
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
            int _amount,
            Population _pop,
            List<Coordinate> _cs,
            ref Random _rand
        ) {
            // List of available competitors
            List<Coordinate> acds = new List<Coordinate>(_cs);
            // Initialize the list of competitors
            Individual[] competitors = new Individual[_amount];
            // Initialize competitors' coordinates
            Coordinate[] coordinates = new Coordinate[_amount];
            // Select competitors
            for (int i = 0; i < _amount; i++)
            {
                // Get a random coordinate
                (int x, int y) rc = Util.RandomElementFromList(acds, ref _rand);
                // Get the corresponding competitor
                competitors[i] = _pop.map[rc.x, rc.y];
                coordinates[i] = rc;
                // Remove competitors from available competitors
                acds.Remove(rc);
            }
            // Find the tournament winner and its coordinate
            Individual winner = null;
            Coordinate coordinate = (Util.UNKNOWN, Util.UNKNOWN);
            for (int i = 0; i < _amount; i++)
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