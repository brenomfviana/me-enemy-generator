using System;

namespace OverlordEnemyGenerator
{
    /// This struct defines search space accordingly to the enemy's attributes.
    ///
    /// The prefix `r` in the attributes' names stands for `range of`, e.g., 
    /// rHealth means range of health.
    public struct SearchSpace
    {
        public (int, int) rHealth { get; }
        public (int, int) rStrength { get; }
        public (float, float) rAttackSpeed { get; }
        public MovementType[] rMovementType { get; }
        public (float, float) rMovementSpeed { get; }
        public (float, float) rActiveTime { get; }
        public (float, float) rRestTime { get; }
        public WeaponType[] rWeaponType { get; }
        public ProjectileType[] rProjectileType { get; }
        public (float, float) rProjectileSpeed { get; }

        /// Search space constructor.
        public SearchSpace(
            (int, int) rHealth,
            (int, int) rStrength,
            (float, float) rAttackSpeed,
            MovementType[] rMovementType,
            (float, float) rMovementSpeed,
            (float, float) rActiveTime,
            (float, float) rRestTime,
            WeaponType[] rWeaponType,
            ProjectileType[] rProjectileType,
            (float, float) rProjectileSpeed
        ) {
            this.rHealth = rHealth;
            this.rStrength = rStrength;
            this.rAttackSpeed = rAttackSpeed;
            this.rMovementType = rMovementType;
            this.rMovementSpeed = rMovementSpeed;
            this.rActiveTime = rActiveTime;
            this.rRestTime = rRestTime;
            this.rWeaponType = rWeaponType;
            this.rProjectileType = rProjectileType;
            this.rProjectileSpeed = rProjectileSpeed;
        }

        /// Return the list of all movement types.
        public static MovementType[] AllMovementTypes()
        {
            return (MovementType[]) Enum.GetValues(typeof(MovementType));
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