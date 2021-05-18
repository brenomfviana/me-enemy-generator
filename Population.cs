using System;
using System.Collections.Generic;

namespace OverlordEnemyGenerator
{
    using Coordinate = System.ValueTuple<int, int>;

    /// This struct represents a population of a MAP-Elites search approach.
    ///
    /// The MAP-Elites population is an N-dimensional array of individuals, 
    /// where each matrix's ax corresponds to a different feature.
    ///
    /// This particular population is mapped into enemy's behavior and weapons.
    /// Each Elite (or matrix cell) corresponds to a combination of different 
    /// types of behaviors and weapons.
    public struct Population
    {
        // The MAP-Elites dimension. The dimension is defined by the number of 
        // behavior types multiplied by the number of weapon Types.
        public (int behavior, int weapon) dimension { get; }
        // The MAP-Elites map (a matrix of individuals).
        public Individual[,] map { get; }

        /// Population constructor.
        public Population(
            int numberOfBehaviorTypes,
            int numbefOfWeaponTypes
        ) {
            this.dimension = (numberOfBehaviorTypes, numbefOfWeaponTypes);
            this.map = new Individual[dimension.behavior, dimension.weapon];
        }

        /// Return the number of Elites of the population.
        public int Count()
        {
            int count = 0;
            for (int b = 0; b < dimension.behavior; b++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    if (!(map[b, w] is null))
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
            int b = (int) individual.enemy.behaviorType;
            int w = (int) individual.weapon.weaponType;
            // Check if the new individual deserves to survive
            if (map[b, w] is null || individual.fitness < map[b, w].fitness)
            {
                map[b, w] = individual;
            }
        }

        /// Return a list corresponding to the Elites coordinates.
        public List<Coordinate> GetElitesCoordinates()
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int b = 0; b < dimension.behavior; b++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    if (!(map[b, w] is null))
                    {
                        coordinates.Add((b, w));
                    }
                }
            }
            return coordinates;
        }

        /// Return a list with the individuals.
        public List<Individual> ToList()
        {
            List<Individual> list = new List<Individual>();
            for (int b = 0; b < dimension.behavior; b++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    list.Add(map[b, w]);
                }
            }
            return list;
        }

        /// Print the individuals of the MAP-Elites population.
        public void Debug()
        {
            for (int b = 0; b < dimension.behavior; b++)
            {
                for (int w = 0; w < dimension.weapon; w++)
                {
                    Console.WriteLine(
                        "Elite " + ((BehaviorType) b) + "-" + ((WeaponType) w)
                    );
                    if (map[b, w] is null)
                    {
                        Console.WriteLine("   Empty");
                    }
                    else
                    {
                        Individual i = map[b, w];
                        Console.WriteLine("   " + i.fitness);
                        Console.WriteLine("   " + i.enemy.health);
                        Console.WriteLine("   " + i.enemy.strength);
                        Console.WriteLine("   " + i.enemy.attackSpeed);
                        Console.WriteLine("   " + i.enemy.movementType);
                        Console.WriteLine("   " + i.enemy.movementSpeed);
                        Console.WriteLine("   " + i.enemy.behaviorType);
                        Console.WriteLine("   " + i.enemy.activeTime);
                        Console.WriteLine("   " + i.enemy.restTime);
                        Console.WriteLine("   " + i.weapon.weaponType);
                        Console.WriteLine("   " + i.weapon.projectileType);
                        Console.WriteLine("   " + i.weapon.projectileSpeed);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}