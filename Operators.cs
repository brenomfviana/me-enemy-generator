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
            // Select `amount` individuals
            Individual[] individuals = new Individual[amount];
            for (int i = 0; i < amount; i++)
            {
                // Perform tournament selection with 3 competitors
                (Coordinate coordinate, Individual individual) = Tournament(
                    3,       // Number of competitors
                    pop,     // Population reference
                    cs,      // List of valid coordinates
                    ref rand // Random number generator
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
        /// same process explained in `Selection` function.
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
                (int x, int y) rc = Util.RandomFromList(acds, ref rand);
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


        /// Reproduce a new individual by mutating a parent.
        public static Individual Mutate(
            Individual parent,
            SearchSpace ss,
            int chance,
            ref Random rand
        ) {
            // New individual
            Individual individual = parent.Clone();
            // Apply mutation on enemy attributes
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.health = Util.RandomInt(
                    ss.rHealth, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.strength = Util.RandomInt(
                    ss.rStrength, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.attackSpeed = Util.RandomFloat(
                    ss.rAttackSpeed, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.movementType = Util.RandomFromArray(
                    ss.rMovementType, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.movementSpeed = Util.RandomFloat(
                    ss.rMovementSpeed, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.activeTime = Util.RandomFloat(
                    ss.rActiveTime, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.restTime = Util.RandomFloat(
                    ss.rRestTime, ref rand
                );
            }
            // Apply mutation on weapon attributes
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.weapon.weaponType = Util.RandomFromArray(
                    ss.rWeaponType, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.weapon.projectileType = Util.RandomFromArray(
                    ss.rProjectileType, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.weapon.projectileSpeed = Util.RandomFloat(
                    ss.rProjectileSpeed, ref rand
                );
            }
            // Return the new mutated individual
            return individual;
        }


        /// Reproduce two new individuals by appling a custom crossover.
        public static Individual[] Crossover(
            Individual parent1,
            Individual parent2,
            ref Random rand
        ) {
            return BLXAlphaCrossover(parent1, parent2, ref rand);
        }

        /// Perform a custom BLX-Alpha crossover composed of two stages.
        ///
        /// The first stage applies a fixed-point crossover dividing the genes
        /// enemy and weapon. The second stage calculates the usual BLX-alpha
        /// of the numerical attributes.
        static Individual[] BLXAlphaCrossover(
            Individual parent1,
            Individual parent2,
            ref Random rand
        ) {
            // Alpha value
            float alpha = 0.5f;
            // Alias for the parents' attributes
            Enemy p1e = parent1.enemy;
            Weapon p1w = parent1.weapon;
            Enemy p2e = parent2.enemy;
            Weapon p2w = parent2.weapon;
            // Initialize the new individuals
            Individual[] id = new Individual[2];
            id[0] = new Individual(p1e, p2w);
            id[1] = new Individual(p2e, p1w);
            // Apply BLX alpha on enemy attributes
            (id[0].enemy.health, id[1].enemy.health)
                = BLXAlphaInt(
                    id[0].enemy.health,
                    id[1].enemy.health,
                    alpha,
                    ref rand
                );
            (id[0].enemy.strength, id[1].enemy.strength)
                = BLXAlphaInt(
                    id[0].enemy.strength,
                    id[1].enemy.strength,
                    alpha,
                    ref rand
                );
            (id[0].enemy.attackSpeed, id[1].enemy.attackSpeed)
                = BLXAlphaFloat(
                    id[0].enemy.attackSpeed,
                    id[1].enemy.attackSpeed,
                    alpha,
                    ref rand
                );
            (id[0].enemy.movementSpeed, id[1].enemy.movementSpeed)
                = BLXAlphaFloat(
                    id[0].enemy.movementSpeed,
                    id[1].enemy.movementSpeed,
                    alpha,
                    ref rand
                );
            (id[0].enemy.activeTime, id[1].enemy.activeTime)
                = BLXAlphaFloat(
                    id[0].enemy.activeTime,
                    id[1].enemy.activeTime,
                    alpha,
                    ref rand
                );
            (id[0].enemy.restTime, id[1].enemy.restTime)
                = BLXAlphaFloat(
                    id[0].enemy.restTime,
                    id[1].enemy.restTime,
                    alpha,
                    ref rand
                );
            // Apply BLX alpha on weapon attributes
            (id[0].weapon.projectileSpeed, id[1].weapon.projectileSpeed)
                = BLXAlphaFloat(
                    id[0].weapon.projectileSpeed,
                    id[1].weapon.projectileSpeed,
                    alpha,
                    ref rand
                );
            // Return the new individuals
            return id;
        }

        /// Return the BLX alpha for the given values.
        static (int, int) BLXAlphaInt(
            float v1,
            float v2,
            float alpha,
            ref Random rand
        ) {
            float max = Math.Max(v1, v2);
            float min = Math.Min(v1, v2);
            float range = (max + alpha) - (min - alpha);
            int a = (int) (rand.NextDouble() * range + (min - alpha));
            int b = (int) (rand.NextDouble() * range + (min - alpha));
            return (a, b);
        }

        /// Return the BLX alpha for the given values.
        static (float, float) BLXAlphaFloat(
            float v1,
            float v2,
            float alpha,
            ref Random rand
        ) {
            float max = Math.Max(v1, v2);
            float min = Math.Min(v1, v2);
            float range = (max + alpha) - (min - alpha);
            float a = (float) (rand.NextDouble() * range + (min - alpha));
            float b = (float) (rand.NextDouble() * range + (min - alpha));
            return (a, b);
        }
    }
}