using System;

namespace OverlordEnemyGenerator
{
    /// This struct defines search space accordingly to the enemy's attributes.
    public struct SearchSpace
    {
        public (int, int) health { get; }
        public (int, int) strength { get; }
        public (float, float) attackSpeed { get; }
        public MovementType[] movementType { get; }
        public (float, float) movementSpeed { get; }
        public BehaviorType[] behavior { get; }
        public (float, float) activeTime { get; }
        public (float, float) restTime { get; }
        public WeaponType[] weaponType { get; }
        public ProjectileType[] projectileType { get; }
        public (float, float) projectileSpeed { get; }

        /// Search space constructor.
        public SearchSpace(
            (int, int) health,
            (int, int) strength,
            (float, float) attackSpeed,
            MovementType[] movementType,
            (float, float) movementSpeed,
            BehaviorType[] behavior,
            (float, float) activeTime,
            (float, float) restTime,
            WeaponType[] weaponType,
            ProjectileType[] projectileType,
            (float, float) projectileSpeed)
        {
            this.health = health;
            this.strength = strength;
            this.attackSpeed = attackSpeed;
            this.movementType = movementType;
            this.movementSpeed = movementSpeed;
            this.behavior = behavior;
            this.activeTime = activeTime;
            this.restTime = restTime;
            this.weaponType = weaponType;
            this.projectileType = projectileType;
            this.projectileSpeed = projectileSpeed;
        }

        /// Return the list of all movement types.
        public static MovementType[] AllMovementTypes()
        {
            return (MovementType[]) Enum.GetValues(typeof(MovementType));
        }

        /// Return the list of all behavior types.
        public static BehaviorType[] AllBehaviorTypes()
        {
            return (BehaviorType[]) Enum.GetValues(typeof(BehaviorType));
        }

        /// Return the list of all weapon types.
        public static WeaponType[] AllWeaponTypes()
        {
            return (WeaponType[]) Enum.GetValues(typeof(WeaponType));
        }

        /// Return the list of all projectile types.
        public static ProjectileType[] AllProjectileTypes()
        {
            return (ProjectileType[]) Enum.GetValues(typeof(ProjectileType));
        }
    }
}