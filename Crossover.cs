using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace EnemyGenerator
{
    /// This class holds the crossover operator.
    public static class Crossover
    {
        /// Perform a custom BLX-Alpha crossover composed of two stages.
        ///
        /// The first stage applies a fixed-point crossover dividing the genes
        /// enemy and weapon. The second stage calculates the usual BLX-alpha
        /// of the numerical attributes. However, if the weapon of both
        /// individuals are different, we ignore the BLX-alpha for the
        /// projectile speed.
        public static Individual[] Apply(
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
            float a = MathF.Max(MathF.Min(c1, fb), fa);
            float b = MathF.Max(MathF.Min(c2, fb), fa);

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