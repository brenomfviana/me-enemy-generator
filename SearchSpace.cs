using System;

namespace EnemyGenerator
{
    /// This class defines the search space of each attribute of enemies.
    ///
    /// The prefix `r` in the attributes' names of this class stands for `range
    /// of`, e.g., the rHealth means the range of health.
    public class SearchSpace
    {
        public (int, int) rHealth { get; }
        public (int, int) rStrength { get; }
        public (float, float) rAttackSpeed { get; }
        public MovementType[] rMovementType { get; }
        public (float, float) rMovementSpeed { get; }
        public (float, float) rActiveTime { get; }
        public (float, float) rRestTime { get; }
        public WeaponType[] rWeaponType { get; }
        public (float, float) rProjectileSpeed { get; }

        /// Search Space constructor.
        private SearchSpace(
            (int, int) _rHealth,
            (int, int) _rStrength,
            (float, float) _rAttackSpeed,
            MovementType[] _rMovementType,
            (float, float) _rMovementSpeed,
            (float, float) _rActiveTime,
            (float, float) _rRestTime,
            WeaponType[] _rWeaponType,
            (float, float) _rProjectileSpeed
        ) {
            rHealth = _rHealth;
            rStrength = _rStrength;
            rAttackSpeed = _rAttackSpeed;
            rMovementType = _rMovementType;
            rMovementSpeed = _rMovementSpeed;
            rActiveTime = _rActiveTime;
            rRestTime = _rRestTime;
            rWeaponType = _rWeaponType;
            rProjectileSpeed = _rProjectileSpeed;
        }

        /// This variable holds the single instance of the Search Space.
        private static SearchSpace instance = null;

        /// Return the single instance of the Search Space.
        public static SearchSpace Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SearchSpace(
                        (1, 5),                         // Health
                        (1, 4),                         // Strength
                        (0.75f, 4f),                    // Attack Speed
                        SearchSpace.AllMovementTypes(), // Movement Types
                        (0.8f, 2.8f),                   // Movement Speed
                        (1.5f, 10f),                    // Active Time
                        (0.3f, 1.5f),                   // Rest Time
                        SearchSpace.AllWeaponTypes(),   // Weapon Types
                        (1f, 4f)                        // Projectile Speed
                    );
                }
                return instance;
            }
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

        /// Return the list of all difficulty ranges.
        public static (float, float)[] AllDifficulties()
        {
            return new (float, float)[] {
                (8, 12), (12, 16), (16, 20), (20, 24), (24, 28)
            };
        }

        /// Return the index of the difficulty in the list of difficulty ranges.
        public static int GetDifficultyIndex(
            float difficulty
        ) {
            int index = -1;
            (float, float)[] list = AllDifficulties();
            for (int i = 0; i < list.Length; i++)
            {
                (float min, float max) d = list[i];
                if (difficulty >= d.min && difficulty < d.max)
                {
                    index = i;
                }
            }
            return index;
        }
    }
}