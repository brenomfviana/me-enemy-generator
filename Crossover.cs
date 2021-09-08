using System;

namespace EnemyGenerator
{
    /// This class holds the crossover operator.
    public class Crossover
    {
        /// Perform a custom BLX-Alpha crossover composed of two stages.
        ///
        /// The first stage applies a fixed-point crossover dividing the genes
        /// enemy and weapon. The second stage calculates the usual BLX-alpha
        /// of the numerical attributes. However, if the weapon of both
        /// individuals are different, we ignore the BLX-alpha for the
        /// projectile speed.
        public static Individual[] Apply(
            Individual _parent1,
            Individual _parent2,
            ref Random _rand
        ) {
            // Calculate a random value for alpha
            float alpha = Util.RandomFloat((0f, 01), ref _rand);
            // Alias for the parents' attributes
            Enemy p1e = _parent1.enemy;
            Weapon p1w = _parent1.weapon;
            Enemy p2e = _parent2.enemy;
            Weapon p2w = _parent2.weapon;
            // Initialize the two new individuals performing a 1-point crossover
            Individual[] inds = new Individual[2];
            inds[0] = new Individual(p1e, p2w);
            inds[1] = new Individual(p2e, p1w);
            // Apply BLX-alpha on enemy attributes
            (inds[0].enemy.health,
             inds[1].enemy.health) =
                BLXAlpha(
                    inds[0].enemy.health,
                    inds[1].enemy.health,
                    SearchSpace.Instance.rHealth,
                    alpha,
                    ref _rand
                );
            (inds[0].enemy.strength,
             inds[1].enemy.strength) =
                BLXAlpha(
                    inds[0].enemy.strength,
                    inds[1].enemy.strength,
                    SearchSpace.Instance.rStrength,
                    alpha,
                    ref _rand
                );
            (inds[0].enemy.attackSpeed,
             inds[1].enemy.attackSpeed) =
                BLXAlpha(
                    inds[0].enemy.attackSpeed,
                    inds[1].enemy.attackSpeed,
                    SearchSpace.Instance.rAttackSpeed,
                    alpha,
                    ref _rand
                );
            (inds[0].enemy.movementSpeed,
             inds[1].enemy.movementSpeed) =
                BLXAlpha(
                    inds[0].enemy.movementSpeed,
                    inds[1].enemy.movementSpeed,
                    SearchSpace.Instance.rMovementSpeed,
                    alpha,
                    ref _rand
                );
            (inds[0].enemy.activeTime,
             inds[1].enemy.activeTime) =
                BLXAlpha(
                    inds[0].enemy.activeTime,
                    inds[1].enemy.activeTime,
                    SearchSpace.Instance.rActiveTime,
                    alpha,
                    ref _rand
                );
            (inds[0].enemy.restTime,
             inds[1].enemy.restTime) =
                BLXAlpha(
                    inds[0].enemy.restTime,
                    inds[1].enemy.restTime,
                    SearchSpace.Instance.rRestTime,
                    alpha,
                    ref _rand
                );
            // Apply BLX-alpha on weapon attributes
            // If both weapons are of the same type
            if (p1w.weaponType == p2w.weaponType)
            {
                (inds[0].weapon.projectileSpeed,
                 inds[1].weapon.projectileSpeed) =
                    BLXAlpha(
                        inds[0].weapon.projectileSpeed,
                        inds[1].weapon.projectileSpeed,
                        SearchSpace.Instance.rProjectileSpeed,
                        alpha,
                        ref _rand
                    );
            }
            // Return the new individuals
            return inds;
        }

        /// Return a tuple of two values calculated by the BLX-alpha.
        static (T, T) BLXAlpha<T>(
            T _v1,
            T _v2,
            (T min, T max) _bounds,
            float _alpha,
            ref Random _rand
        ) {
            // Get the type of `float`
            Type ft = typeof(float);
            // Convert the entered values to float
            float fv1 = (float) Convert.ChangeType(_v1, ft);
            float fv2 = (float) Convert.ChangeType(_v2, ft);
            float fa = (float) Convert.ChangeType(_bounds.min, ft);
            float fb = (float) Convert.ChangeType(_bounds.max, ft);
            // Identify the maximum and minimum values
            float max = MathF.Max(fv1, fv2);
            float min = MathF.Min(fv1, fv2);
            // Calculate the crossover values
            float max_alpha = max + _alpha;
            float min_alpha = min - _alpha;
            (float, float) range = (max_alpha, min_alpha);
            float c1 = Util.RandomFloat(range, ref _rand);
            float c2 = Util.RandomFloat(range, ref _rand);
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