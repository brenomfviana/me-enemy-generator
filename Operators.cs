using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace EnemyGenerator
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
            // Ensure the population size is enough
            Debug.Assert(
                cs.Count - amount > 3,
                "There are no enough individuals in the input population to " +
                "perform this operation."
            );
            // Select `amount` individuals
            Individual[] individuals = new Individual[amount];
            for (int i = 0; i < amount; i++)
            {
                // Perform tournament selection with 3 competitors
                (Coordinate coordinate, Individual individual) = Tournament(
                    3,       // Number of competitors
                    pop,     // Population
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
                (int x, int y) rc = Util.RandomElementFromList(acds, ref rand);
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
            int chance,
            ref Random rand
        ) {
            // New individual
            Individual individual = parent.Clone();
            // Apply mutation on enemy attributes
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.health = Util.RandomInt(
                    SearchSpace.Instance.rHealth, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.strength = Util.RandomInt(
                    SearchSpace.Instance.rStrength, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.attackSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rAttackSpeed, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.movementType = Util.RandomElementFromArray(
                    SearchSpace.Instance.rMovementType, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.movementSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rMovementSpeed, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.activeTime = Util.RandomFloat(
                    SearchSpace.Instance.rActiveTime, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.restTime = Util.RandomFloat(
                    SearchSpace.Instance.rRestTime, ref rand
                );
            }
            // Apply mutation on weapon attributes
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.weapon.weaponType = Util.RandomElementFromArray(
                    SearchSpace.Instance.rWeaponType, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.weapon.projectileSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rProjectileSpeed, ref rand
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
            // Calculate a random value for alpha
            float alpha = Util.RandomFloat((0f, 01), ref rand);

            // Alias for the parents' attributes
            Enemy p1e = parent1.enemy;
            Weapon p1w = parent1.weapon;
            Enemy p2e = parent2.enemy;
            Weapon p2w = parent2.weapon;

            // Initialize the new individuals performing a 1-point crossover
            Individual[] inds = new Individual[2];
            inds[0] = new Individual(p1e, p2w);
            inds[1] = new Individual(p2e, p1w);

            // Apply BLX-alpha on enemy attributes
            (inds[0].enemy.health, inds[1].enemy.health)
                = BLXAlpha(
                    inds[0].enemy.health,
                    inds[1].enemy.health,
                    SearchSpace.Instance.rHealth,
                    alpha,
                    ref rand
                );
            (inds[0].enemy.strength, inds[1].enemy.strength)
                = BLXAlpha(
                    inds[0].enemy.strength,
                    inds[1].enemy.strength,
                    SearchSpace.Instance.rStrength,
                    alpha,
                    ref rand
                );
            (inds[0].enemy.attackSpeed, inds[1].enemy.attackSpeed)
                = BLXAlpha(
                    inds[0].enemy.attackSpeed,
                    inds[1].enemy.attackSpeed,
                    SearchSpace.Instance.rAttackSpeed,
                    alpha,
                    ref rand
                );
            (inds[0].enemy.movementSpeed, inds[1].enemy.movementSpeed)
                = BLXAlpha(
                    inds[0].enemy.movementSpeed,
                    inds[1].enemy.movementSpeed,
                    SearchSpace.Instance.rMovementSpeed,
                    alpha,
                    ref rand
                );
            (inds[0].enemy.activeTime, inds[1].enemy.activeTime)
                = BLXAlpha(
                    inds[0].enemy.activeTime,
                    inds[1].enemy.activeTime,
                    SearchSpace.Instance.rActiveTime,
                    alpha,
                    ref rand
                );
            (inds[0].enemy.restTime, inds[1].enemy.restTime)
                = BLXAlpha(
                    inds[0].enemy.restTime,
                    inds[1].enemy.restTime,
                    SearchSpace.Instance.rRestTime,
                    alpha,
                    ref rand
                );

            // Apply BLX-alpha on weapon attributes
            // If both weapons are of the same type
            if (p1w.weaponType == p2w.weaponType)
            {
                (inds[0].weapon.projectileSpeed, inds[1].weapon.projectileSpeed)
                    = BLXAlpha(
                        inds[0].weapon.projectileSpeed,
                        inds[1].weapon.projectileSpeed,
                        SearchSpace.Instance.rProjectileSpeed,
                        alpha,
                        ref rand
                    );
            }

            // Return the new individuals
            return inds;
        }

        /// Return a tuple of two values calculated by the BLX-alpha.
        static (T, T) BLXAlpha<T>(
            T v1,
            T v2,
            (T min, T max) bounds,
            float alpha,
            ref Random rand
        ) {
            // Get the type of `float`
            Type ft = typeof(float);
            // Convert the given values to float
            float fv1 = (float) Convert.ChangeType(v1, ft);
            float fv2 = (float) Convert.ChangeType(v2, ft);
            float fa = (float) Convert.ChangeType(bounds.min, ft);
            float fb = (float) Convert.ChangeType(bounds.max, ft);

            // Identify the maximum and minimum values
            float max = MathF.Max(fv1, fv2);
            float min = MathF.Min(fv1, fv2);

            // Calculate the crossover values
            float max_alpha = max + alpha;
            float min_alpha = min - alpha;
            (float, float) range = (max_alpha, min_alpha);
            float c1 = Util.RandomFloat(range, ref rand);
            float c2 = Util.RandomFloat(range, ref rand);

            // If the values extrapolate the attribute's range of values, then
            // truncate the result to the closest value
            float a = MathF.Max(c1, fa);
            float b = MathF.Min(c2, fb);

            // Get the parameters' type
            Type pt = typeof(T);
            // Convert the result to type `T`
            return (
                (T) Convert.ChangeType(a, pt),
                (T) Convert.ChangeType(b, pt)
            );
        }
    }
}