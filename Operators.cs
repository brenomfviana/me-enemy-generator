using System;
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
        public static Individual[] Select(
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
            List<Coordinate> cs,
            Random rand
        ) {
            // List of available competitors
            List<Coordinate> acds = new List<Coordinate>(cs);
            // Initialize `amount` competitors for the tournament
            Individual[] competitors = new Individual[amount];
            // Initialize competitors' coordinates
            Coordinate[] coordinates = new Coordinate[amount];
            for (int i = 0; i < amount; i++)
            {
                // Get a random coordinate
                (int x, int y) rc = Util.RandomFromList(acds, rand);
                // Get the corresponding competitor
                competitors[i] = pop.map[rc.x, rc.y];
                coordinates[i] = rc;
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
                    coordinate = coordinates[i];
                }
            }
            // Return the tournament winner and its coordinate
            return (coordinate, winner);
        }


        /// Reproduce a new individual by mutating a parent individual.
        public static Individual Mutate(
            Individual parent,
            SearchSpace ss,
            Random rand
        ) {
            // New individual
            Individual individual = parent.Clone();
            // Apply mutation
            switch (rand.Next(11))
            {
                // Enemy attributes
                case 0:
                    individual.enemy.health
                        = Util.RandomInt(ss.rHealth, rand);
                    break;
                case 1:
                    individual.enemy.strength
                        = Util.RandomInt(ss.rStrength, rand);
                    break;
                case 2:
                    individual.enemy.attackSpeed
                        = Util.RandomFloat(ss.rAttackSpeed, rand);
                    break;
                case 3:
                    individual.enemy.movementType
                        = Util.RandomFromArray(ss.rMovementType, rand);
                    break;
                case 4:
                    individual.enemy.movementSpeed
                        = Util.RandomFloat(ss.rMovementSpeed, rand);
                    break;
                case 5:
                    individual.enemy.behaviorType
                        = Util.RandomFromArray(ss.rBehaviorType, rand);
                    break;
                case 6:
                    individual.enemy.activeTime
                        = Util.RandomFloat(ss.rActiveTime, rand);
                    break;
                case 7:
                    individual.enemy.restTime
                        = Util.RandomFloat(ss.rRestTime, rand);
                    break;
                // Weapon attributes
                case 8:
                    individual.weapon.weaponType
                        = Util.RandomFromArray(ss.rWeaponType, rand);
                    break;
                case 9:
                    individual.weapon.projectileType
                        = Util.RandomFromArray(ss.rProjectileType, rand);
                    break;
                case 10:
                    individual.weapon.projectileSpeed
                        = Util.RandomFloat(ss.rProjectileSpeed, rand);
                    break;
            }
            // Calculate new individual fitness
            individual.fitness = Fitness.Calculate(individual);
            // Return the new mutated individual
            return individual;
        }


        /// Reproduce two new individuals by appling BLX-alpha crossover.
        public static Individual[] Crossover(
            Individual parent1,
            Individual parent2,
            Random rand
        ) {
            return new Individual[1] {
                MeanRandomCrossover(parent1, parent2, rand)
            };
        }

        /// Perform a custom crossover composed of two stages.
        ///
        /// The first stage calculates the means of the numerical attributes. 
        /// The second stage randomly selects two options of crossover: or the 
        /// enemy nominal attributes from the first parent and the weapon 
        /// nominal attributtes from the second parent or the inverse are set 
        /// to the new individual.
        static Individual MeanRandomCrossover(
            Individual parent1,
            Individual parent2,
            Random rand
        ) {
            // Get from parents
            Enemy p1e = parent1.enemy;
            Weapon p1w = parent1.weapon;
            Enemy p2e = parent2.enemy;
            Weapon p2w = parent2.weapon;
            // New individual
            Enemy e = new Enemy();
            Weapon w = new Weapon();
            // Apply mean crossover
            e.health = (p1e.health + p2e.health) / 2;
            e.strength = (p1e.strength + p2e.strength) / 2;
            e.attackSpeed = (p1e.attackSpeed + p2e.attackSpeed) / 2;
            e.movementSpeed = (p1e.movementSpeed + p2e.movementSpeed) / 2;
            e.activeTime = (p1e.activeTime + p2e.activeTime) / 2;
            e.restTime = (p1e.restTime + p2e.restTime) / 2;
            w.projectileSpeed = (p1w.projectileSpeed + p2w.projectileSpeed) / 2;
            // Apply random crossover
            if (rand.Next(2) == 1) {
                e.movementType = p1e.movementType;
                e.behaviorType = p1e.behaviorType;
                w.weaponType = p2w.weaponType;
                w.projectileType = p2w.projectileType;
            } else {
                e.movementType = p2e.movementType;
                e.behaviorType = p2e.behaviorType;
                w.weaponType = p1w.weaponType;
                w.projectileType = p1w.projectileType;
            }
            // Return the new individual
            Individual individual = new Individual(e, w);
            individual.fitness = Fitness.Calculate(individual);
            return individual;
        }
    }
}