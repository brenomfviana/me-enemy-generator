using System;
using System.Text.Json.Serialization;

namespace OverlordEnemyGenerator
{
    /// This class represents an individual.
    ///
    /// Individuals are composed of an enemy, a weapon, its fitness value, its
    /// difficulty degree, and the generation when it was created.
    ///
    /// Why individuals are represented by a class instead of a struct? When
    /// using MAP-Elites some slots may be empty, then the `null` option makes
    /// easier to manage the MAP-Elites population.
    [Serializable]
    public class Individual
    {
        [JsonInclude]
        public Enemy enemy;
        [JsonInclude]
        public Weapon weapon;
        [JsonInclude]
        public float difficulty;
        [JsonInclude]
        public float fitness;
        [JsonInclude]
        public int generation;

        /// Individual contructor.
        public Individual(
            Enemy _enemy,
            Weapon _weapon
        ) {
            enemy = _enemy;
            weapon = _weapon;
        }

        /// Calculate the difficulty of the input individual.
        public void CalculateDifficulty(SearchSpace ss)
        {
            // Get enemy and weapon genes from the individual
            Enemy e = enemy;
            Weapon w = weapon;
            // Calculate the base difficulty
            float fH = e.health;
            float fA = e.attackSpeed;
            float fM = e.movementSpeed;
            fM *= Multiplier(e.movementType);
            fM *= Multiplier(e.behaviorType);
            float fD = e.strength;
            fD *= Multiplier(w.weaponType);
            float fP =  w.projectileSpeed;
            fP *= Multiplier(w.projectileType);
            float baseDifficulty = fH + fA + fM + fD + fP;
            // Calculate the difficulty intensity factors
            float fAT = e.activeTime / ss.rActiveTime.Item2;
            float fRT = 1 / (e.restTime / ss.rRestTime.Item2);
            float intensity = fAT * fRT;
            // Calculate the final difficulty
            difficulty = baseDifficulty * intensity;
        }

        /// Return the multiplier factor for the input type of movement,
        /// behavior, weapon, or projectile.
        static float Multiplier<T>(
            T value
        ) {
            if (value is BehaviorType)
            {
                switch(value)
                {
                    case BehaviorType.Indifferent:
                        return 1.00f;
                    case BehaviorType.LoneWolf:
                        return 0.90f;
                    case BehaviorType.Swarm:
                        return 1.10f;
                    case BehaviorType.Pincer:
                        return 1.30f;
                }
            }

            if (value is MovementType)
            {
                switch (value)
                {
                    case MovementType.None:
                        return 0.00f;
                    case MovementType.Random:
                        return 1.05f;
                    case MovementType.Flee:
                        return 1.10f;
                    case MovementType.Follow:
                        return 1.15f;
                    case MovementType.Random1D:
                        return 1.00f;
                    case MovementType.Flee1D:
                        return 1.08f;
                    case MovementType.Follow1D:
                        return 1.12f;
                }
            }

            if (value is WeaponType)
            {
                switch (value)
                {
                    case WeaponType.None:
                        return 1.00f;
                    case WeaponType.Sword:
                        return 1.50f;
                    case WeaponType.Shotgun:
                        return 1.25f;
                    case WeaponType.Bomb:
                        return 1.35f;
                    case WeaponType.Shield:
                        return 1.60f;
                    case WeaponType.Cure:
                        return 1.70f;
                }
            }

            if (value is ProjectileType)
            {
                switch (value)
                {
                    case ProjectileType.None:
                        return 1.00f;
                    case ProjectileType.Arrow:
                        return 1.10f;
                    case ProjectileType.Bullet:
                        return 1.20f;
                }
            }
            return -100f;
        }

        /// Return a clone of the individual.
        ///
        /// We create a new individual by passing `enemy` and `weapon` in the
        /// Individual constructor. Since both are structs, we can copy them by
        /// value instead of doing a deep copy.
        public Individual Clone()
        {
            Individual individual = new Individual(enemy, weapon);
            individual.difficulty = difficulty;
            individual.fitness = fitness;
            individual.generation = generation;
            return individual;
        }

        /// Print the individual attributes.
        public void Debug()
        {
            Console.WriteLine("   " + generation);
            Console.WriteLine("   " + fitness);
            Console.WriteLine("   " + difficulty);
            Console.WriteLine("   " + enemy.health);
            Console.WriteLine("   " + enemy.strength);
            Console.WriteLine("   " + enemy.attackSpeed);
            Console.WriteLine("   " + enemy.behaviorType);
            Console.WriteLine("   " + enemy.movementType);
            Console.WriteLine("   " + enemy.movementSpeed);
            Console.WriteLine("   " + enemy.activeTime);
            Console.WriteLine("   " + enemy.restTime);
            Console.WriteLine("   " + weapon.weaponType);
            Console.WriteLine("   " + weapon.projectileType);
            Console.WriteLine("   " + weapon.projectileSpeed);
            Console.WriteLine();
        }

        /// Return a random individual.
        public static Individual GetRandom(
            SearchSpace ss,
            ref Random rand
        ) {
            // Create a random enemy
            Enemy e = new Enemy(
                Util.RandomInt(ss.rHealth, ref rand),
                Util.RandomInt(ss.rStrength, ref rand),
                Util.RandomFloat(ss.rAttackSpeed, ref rand),
                Util.RandomElementFromArray(ss.rBehaviorType, ref rand),
                Util.RandomElementFromArray(ss.rMovementType, ref rand),
                Util.RandomFloat(ss.rMovementSpeed, ref rand),
                Util.RandomFloat(ss.rActiveTime, ref rand),
                Util.RandomFloat(ss.rRestTime, ref rand)
            );
            // Create a random weapon
            Weapon w = new Weapon(
                Util.RandomElementFromArray(ss.rWeaponType, ref rand),
                Util.RandomElementFromArray(ss.rProjectileType, ref rand),
                Util.RandomFloat(ss.rProjectileSpeed, ref rand)
            );
            // Combine the enemy and the weapon to create a new individual
            Individual individual = new Individual(e, w);
            individual.generation = -1;
            individual.difficulty = -1.0f;
            individual.fitness = -1.0f;
            // Return the created individual
            return individual;
        }
    }

    /// This struct represents an enemy.
    [Serializable]
    public struct Enemy
    {
        [JsonInclude]
        public int health;
        [JsonInclude]
        public int strength;
        [JsonInclude]
        public float attackSpeed;
        [JsonInclude]
        public BehaviorType behaviorType;
        [JsonInclude]
        public MovementType movementType;
        [JsonInclude]
        public float movementSpeed;
        [JsonInclude]
        public float activeTime;
        [JsonInclude]
        public float restTime;

        /// Enemy contructor.
        public Enemy(
            int _health,
            int _strength,
            float _attackSpeed,
            BehaviorType _behaviorType,
            MovementType _movementType,
            float _movementSpeed,
            float _activeTime,
            float _restTime
        ) {
            health = _health;
            strength = _strength;
            attackSpeed = _attackSpeed;
            behaviorType = _behaviorType;
            movementType = _movementType;
            movementSpeed = _movementSpeed;
            activeTime = _activeTime;
            restTime = _restTime;
        }
    }

    // This struc represents a weapon.
    [Serializable]
    public struct Weapon
    {
        [JsonInclude]
        public WeaponType weaponType;
        [JsonInclude]
        public ProjectileType projectileType;
        [JsonInclude]
        public float projectileSpeed;

        // Weapon constructor.
        public Weapon(
            WeaponType _weaponType,
            ProjectileType _projectileType,
            float _projectileSpeed
        ) {
            weaponType = _weaponType;
            projectileType = _projectileType;
            projectileSpeed = _projectileSpeed;
        }
    }

    // This enum defines the behavior types of enemies.
    public enum BehaviorType
    {
        Indifferent, // Enemy does nothing.
        LoneWolf,    // Enemy prefers to be alone.
        Swarm,       // Enemy prefers to be in a group of enemies.
        Pincer,      // Enemies attack the player on both sides.
    }

    // This enum defines the movement types of enemies.
    [Serializable]
    public enum MovementType
    {
        None,     // Enemy stays still.
        Random,   // Enemy performs random 2D movements.
        Follow,   // Enemy follows the player.
        Flee,     // Enemy flees from the player.
        Random1D, // Enemy performs random horizontal or vertical movements.
        Follow1D, // Enemy follows the player horizontally or vertically.
        Flee1D,   // Enemy flees from the player horizontally or vertically.
    }

    /// This enum defines the types of weapons an enemy may have.
    [Serializable()]
    public enum WeaponType
    {
        None,    // Enemy attacks the player with barehands (Melee).
        Sword,   // Enemy uses a short sword to damage the player (Melee).
        Shotgun, // Enemy shots projectiles towards the player (Range).
        Bomb,    // Enemy shots bombs towards the player (Range).
        Shield,  // Enemy uses shields to defend themselves (Defense).
        Cure,    // Enemy uses magic to cure enemies (Defense).
    }

    // This enum defines the projectile types of weapons.
    [Serializable]
    public enum ProjectileType
    {
        None,   // Weapon is not a projectile weapon.
        Arrow,  // Weapon shots arrows.
        Bullet, // Weapon shots bullets.
    }
}