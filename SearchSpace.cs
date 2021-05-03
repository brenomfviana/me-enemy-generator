namespace OverlordEnemyGenerator
{
    /// This struct defines search space accordingly to the enemy's attributes.
    public struct SearchSpace
    {
        public (int, int) health;
        public (int, int) strength;
        public (float, float) attackSpeed;
        public MovementType[] movementType;
        public (float, float) movementSpeed;
        public BehaviorType[] behavior;
        public (float, float) activeTime;
        public (float, float) restTime;
        public WeaponType[] weaponType;
        public ProjectileType[] projectileType;
        public (float, float) projectileSpeed;

        /// Return the list of all movement types.
        public static MovementType[] AllMovementTypes()
        {
            return new MovementType[] {
                MovementType.None,
                MovementType.Random,
                MovementType.Follow,
                MovementType.Flee,
                MovementType.Random1D,
                MovementType.Follow1D,
                MovementType.Flee1D,
            };
        }

        /// Return the list of all behavior types.
        public static BehaviorType[] AllBehaviorTypes()
        {
            return new BehaviorType[] {
                BehaviorType.Indifferent,
                BehaviorType.LoneWolf,
                BehaviorType.Swarm,
            };
        }

        /// Return the list of all weapon types.
        public static WeaponType[] AllWeaponTypes()
        {
            return new WeaponType[] {
                WeaponType.None,
                WeaponType.Sword,
                WeaponType.Shotgun,
                WeaponType.Cannon,
                WeaponType.Shield,
                WeaponType.Cure,
            };
        }

        /// Return the list of all projectile types.
        public static ProjectileType[] AllProjectileTypes()
        {
            return new ProjectileType[] {
                ProjectileType.None,
                ProjectileType.Bullet,
                ProjectileType.Arrow,
                ProjectileType.Bomb,
            };
        }
    }
}