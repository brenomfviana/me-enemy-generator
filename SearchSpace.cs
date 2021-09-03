using System;

namespace EnemyGenerator
{
    /// This struct defines search space of the enemy's attributes.
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
        public (float, float) rProjectileSpeed { get; }

        /// Search space constructor.
        public SearchSpace(
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