using System;
using System.Reflection;
using System.Collections.Generic;

namespace OverlordEnemyGenerator
{
    using Coordinate = System.ValueTuple<int, int>;

    /// This class holds all the evolutionary operators.
    public class Operators
    {
        /// Select individuals from the MAP-Elites population.
        ///
        /// We ensure that the same individual will not be selected for the 
        /// same selection process. To do so, we use an auxiliary list composed 
        /// of the individuals' coordinates in the MAP-Elites population. 
        /// Instead of selecting directly an individual, we select its 
        /// coordinate from the auxiliary list and remove it then it is not 
        /// available for the next selection.
        public static Individual[] Selection(
            int amount,
            Population pop,
            Random rand
        ) {
            // List of Elites' coordinates
            List<Coordinate> cs = pop.GetElitesCoordinates();
            // Select `amount` individuals
            Individual[] individuals = new Individual[amount];
            for (int i = 0; i < amount; i++)
            {
                // Perform tournament selection with 3 competitors
                (Coordinate coordinate, Individual individual) = Tournament(
                    3,   // Number of competitors
                    pop, // Population reference
                    cs,  // List of valid coordinates
                    rand // Random number generator
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
        /// We ensure that the same individual will not be selected for the 
        /// same tournament selection process. To do so, we apply the same 
        /// process explained in `Selection` function.
        static (Coordinate, Individual) Tournament(
            int amount,
            Population pop,
            List<Coordinate> coordinates,
            Random rand
        ) {
            // List of available competitors
            List<Coordinate> acds = new List<Coordinate>(coordinates);
            // Select `amount` competitors for the tournament
            Individual[] competitors = new Individual[amount];
            // Competitors' coordinates
            Coordinate[] cpcds = new Coordinate[amount];
            for (int i = 0; i < amount; i++)
            {
                // Get a random coordinate
                (int x, int y) rc = Util.RandomFromList(acds, rand);
                // Get the corresponding competitor
                competitors[i] = pop.map[rc.x, rc.y];
                cpcds[i] = rc;
                // Remove competitors from available competitors
                acds.Remove(rc);
            }
            // Find the tournament winner and its coordinate
            Individual winner = null;
            Coordinate coordinate = (-1, -1);
            for (int i = 0; i < amount; i++)
            {
                if (winner is null || competitors[i].fitness > winner.fitness)
                {
                    winner = competitors[i];
                    coordinate = cpcds[i];
                }
            }
            // Return the tournament winner and its coordinate
            return (coordinate, winner);
        }


        /// Reproduce a new individual by mutating a parent individual.
        public static Individual Mutate(
            Individual individual,
            SearchSpace ss,
            Random rand
        ) {
            // New individual
            Individual mutant = individual.Clone();
            // Apply mutation
            switch (rand.Next(11))
            {
                // Enemy attributes
                case 0:
                    mutant.enemy.health
                        = Util.RandomInt(ss.rHealth, rand);
                    break;
                case 1:
                    mutant.enemy.strength
                        = Util.RandomInt(ss.rStrength, rand);
                    break;
                case 2:
                    mutant.enemy.attackSpeed
                        = Util.RandomFloat(ss.rAttackSpeed, rand);
                    break;
                case 3:
                    mutant.enemy.movementType
                        = Util.RandomFromArray(ss.rMovementType, rand);
                    break;
                case 4:
                    mutant.enemy.movementSpeed
                        = Util.RandomFloat(ss.rMovementSpeed, rand);
                    break;
                case 5:
                    mutant.enemy.behaviorType
                        = Util.RandomFromArray(ss.rBehaviorType, rand);
                    break;
                case 6:
                    mutant.enemy.activeTime
                        = Util.RandomFloat(ss.rActiveTime, rand);
                    break;
                case 7:
                    mutant.enemy.restTime
                        = Util.RandomFloat(ss.rRestTime, rand);
                    break;
                // Weapon attributes
                case 8:
                    mutant.weapon.weaponType
                        = Util.RandomFromArray(ss.rWeaponType, rand);
                    break;
                case 9:
                    mutant.weapon.projectileType
                        = Util.RandomFromArray(ss.rProjectileType, rand);
                    break;
                case 10:
                    mutant.weapon.projectileSpeed
                        = Util.RandomFloat(ss.rProjectileSpeed, rand);
                    break;
            }
            // Calculate new individual fitness
            mutant.fitness = Fitness.Calculate(mutant);
            // Return the new mutated individual
            return mutant;
        }


        /// Reproduce two new individuals by appling BLX-alpha crossover.
        public static Individual[] Crossover(
            Individual individual1,
            Individual individual2,
            Random rand
        ) {
            return null;
        }
    }
}