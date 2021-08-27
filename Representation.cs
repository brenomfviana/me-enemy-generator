using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OverlordEnemyGenerator
{
    /// This class represents an individual.
    ///
    /// Individuals are composed of an enemy, a weapon, their fitness value, 
    /// their difficulty degree, and the generation when they were created.
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

            // List of range weapons
            List<WeaponType> rangeWeapons = new List<WeaponType>() {
                WeaponType.Bow,
                WeaponType.BombThrower,
            };
            // List of bad movements (i.e., the movements that do not present 
            // a clear risk to the player)
            List<MovementType> badMovements = new List<MovementType>() {
                MovementType.Random,
                MovementType.Random1D,
                MovementType.Flee,
                MovementType.Flee1D,
            };

            // Calculate health factor
            float fH = e.health * 10;

            // Calculate movement factor
            float fM = e.movementSpeed;
            fM *= Multiplier(e.movementType);
            fM *= Multiplier(e.behaviorType);

            // Calculate strength factor
            float fS = e.strength;
            fS *= e.attackSpeed;
            fS *= Multiplier(w.weaponType);
            // If the enemy stays still, it is only a risk to the player if it
            // throws projectiles towards the player
            fS *= (e.movementType == MovementType.None &&
                !rangeWeapons.Contains(w.weaponType)) ? 0 : 1;
            // If the enemy has a bad movement, it is less risky to the player
            // if it did not throw projectiles towards the player
            fS *= (badMovements.Contains(e.movementType) &&
                !rangeWeapons.Contains(w.weaponType)) ? 0.5f : 1;

            // Calculate the base difficulty
            float baseDifficulty = fH + fM + fS;

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
                        return 0.85f;
                    case BehaviorType.LoneWolf:
                        return 1.00f;
                    case BehaviorType.Swarm:
                        return 1.15f;
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
                        return 1.00f;
                    case MovementType.Flee:
                        return 1.10f;
                    case MovementType.Follow:
                        return 1.20f;
                    case MovementType.Random1D:
                        return 0.90f;
                    case MovementType.Flee1D:
                        return 1.00f;
                    case MovementType.Follow1D:
                        return 1.10f;
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
                    case WeaponType.Bow:
                        return 1.35f;
                    case WeaponType.BombThrower:
                        return 1.25f;
                    case WeaponType.Shield:
                        return 1.60f;
                    case WeaponType.Cure:
                        return 1.70f;
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
            Console.WriteLine("  G=" + generation);
            Console.WriteLine("  F=" + fitness);
            Console.WriteLine("  D=" + difficulty);
            Console.WriteLine("  He=" + enemy.health);
            Console.WriteLine("  St=" + enemy.strength);
            Console.WriteLine("  AS=" + enemy.attackSpeed);
            Console.WriteLine("  Be=" + enemy.behaviorType);
            Console.WriteLine("  MT=" + enemy.movementType);
            Console.WriteLine("  MS=" + enemy.movementSpeed);
            Console.WriteLine("  AT=" + enemy.activeTime);
            Console.WriteLine("  RT=" + enemy.restTime);
            Console.WriteLine("  WT=" + weapon.weaponType);
            Console.WriteLine("  PS=" + weapon.projectileSpeed);
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
        public float projectileSpeed;

        // Weapon constructor.
        public Weapon(
            WeaponType _weaponType,
            float _projectileSpeed
        ) {
            weaponType = _weaponType;
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
        None,        // Enemy attacks the player with barehands (Melee).
        Sword,       // Enemy uses a short sword to damage the player (Melee).
        Bow,         // Enemy shots projectiles towards the player (Range).
        BombThrower, // Enemy shots bombs towards the player (Range).
        Shield,      // Enemy uses shields to defend themselves (Defense).
        Cure,        // Enemy uses magic to cure enemies (Defense).
    }
}