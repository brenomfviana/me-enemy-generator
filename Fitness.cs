using System;

namespace OverlordEnemyGenerator
{
    /// This class holds all the fitness-related functions.
    public class Fitness
    {
        /// Calculate the fitness value an individual.
        public static float Calculate(
            Individual individual,
            float goal
        ) {
            // Get enemy and weapon components
            Enemy e = individual.enemy;
            Weapon w = individual.weapon;
            // Calculate fitness
            float fH = e.health;
            float fA = e.activeTime;
            float fR = 1 / e.restTime;
            float fM = e.movementSpeed * Multiplier(e.movementType);
            float fD = e.strength * Multiplier(w.weaponType);
            float fP = e.attackSpeed + w.projectileSpeed;
            fP *= Multiplier(w.projectileType);
            // Sum all difficulty factors
            return Math.Abs(goal - (fH + fA + fR + fM + fD + fP));
        }

        /// Return the multiplier factor for the given type of movement, 
        /// behavior, weapon, or projectile.
        static float Multiplier<T>(
            T value
        ) {
            // If the value is a MovementType
            if (value is MovementType) {
                switch (value)
                {
                    case MovementType.None:
                        return 0f;
                    case MovementType.Random:
                        return 1.04f;
                    case MovementType.Random1D:
                        return 1f;
                    case MovementType.Flee:
                        return 1.1f;
                    case MovementType.Flee1D:
                        return 1.08f;
                    case MovementType.Follow:
                        return 1.15f;
                    case MovementType.Follow1D:
                        return 1.12f;
                }
            }
            // If the value is a BehaviorType
            else if (value is BehaviorType)
            {
                switch (value)
                {
                    case BehaviorType.Indifferent:
                        return 1f;
                    case BehaviorType.LoneWolf:
                        return 1f;
                    case BehaviorType.Swarm:
                        return 1f;
                }
            }
            // If the value is a WeaponType
            else if (value is WeaponType)
            {
                switch (value)
                {
                    case WeaponType.None:
                        return 1f;
                    case WeaponType.Sword:
                        return 1.5f;
                    case WeaponType.Shotgun:
                        return 1f;           // Change it later
                    case WeaponType.Cannon:
                        return 1f;           // Change it later
                    case WeaponType.Shield:
                        return 1.6f;
                    case WeaponType.Cure:
                        return 1f;           // Change it later
                }
            }
            // If the value is a ProjectileType
            else if (value is ProjectileType)
            {
                switch (value)
                {
                    case ProjectileType.None:
                        return 1f;
                    case ProjectileType.Bullet:
                        return 0.3f;            // Change it later?
                    case ProjectileType.Arrow:
                        return 0.3f;            // Change it later?
                    case ProjectileType.Bomb:
                        return 0.3f;            // Change it later?
                }
            }
            return 0f;
        }
    }
}