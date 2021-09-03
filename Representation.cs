using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EnemyGenerator
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
        /// IMPORTANT: This difficulty function was designed for our prototype.
        public void CalculateDifficulty()
        {
            // Get enemy and weapon genes from the individual
            Enemy e = enemy;
            Weapon w = weapon;

            // List of ranged weapons
            List<WeaponType> rangedWeapons = new List<WeaponType>() {
                WeaponType.Bow,
                WeaponType.BombThrower,
            };
            // List of melee weapons
            List<WeaponType> meleeWeapons = new List<WeaponType>() {
                WeaponType.None,
                WeaponType.Sword,
                WeaponType.Shield,
            };
            // List of healer movements
            // The healer ideally searches for other enemies and avoids the
            // player, besides these movements in melee enemies do not present
            // a clear risk to the player
            List<MovementType> healerMovements = new List<MovementType>() {
                MovementType.Random,
                MovementType.Random1D,
                MovementType.Flee,
                MovementType.Flee1D,
            };

            // Calculate health factor
            float fH = e.health * 2f;

            // Calculate strength factor
            float fS = e.strength;
            fS *= Multiplier(w.weaponType);
            // Melee enemies attack by touching the player, therefore, the 
            // movement speed increase their strenght
            fS *= meleeWeapons.Contains(w.weaponType) ?
                e.movementSpeed : 1;
            // Shooter enemies attack by throwing projectiles, then we count
            // both attack speed (shooting frequency) and projectile speed
            fS *= rangedWeapons.Contains(w.weaponType) ?
                (e.attackSpeed * w.projectileSpeed) * 0.5f : 1;
            // The cooldown of healer enemies follows the attack speed
            fS *= w.weaponType == WeaponType.CureSpell ?
                e.attackSpeed : 1;

            // Calculate movement factor
            float fM = e.movementSpeed;
            // Both active time and rest time affect the behavior regarding
            // the enemies' movements, not the enemies' battles
            fM += e.activeTime / 3f + 1f / e.restTime;

            // Calculate the gameplay factor
            float fG = 1f;
            // Enemies with no health are useless
            fG *= e.health == 0 ? 0 : 1;
            // Enemies with no strength are useless
            fG *= e.strength == 0 ? 0 : 1;
            // Melee enemies are only risky if they follow the player
            fG *= meleeWeapons.Contains(w.weaponType) ?
                (e.movementType == MovementType.Follow ? 1 : 0) : 1;
            // Shooter enemies that stays still are the only that present some
            // risk to the player since they throw projectiles towards them
            fG *= rangedWeapons.Contains(w.weaponType) ?
                (e.movementType == MovementType.None ? 0.5f : 1) : 1;
            // Shooter enemies that fled are riskier to the player
            fG *= rangedWeapons.Contains(w.weaponType) ?
                (e.movementType == MovementType.Flee1D ? 1.25f : 1) : 1;
            fG *= rangedWeapons.Contains(w.weaponType) ?
                (e.movementType == MovementType.Flee ? 1.5f : 1) : 1;
            // Shooter enemies do not perform well when they follow the player
            // and are faster than the projectiles they shoot
            fG *= (rangedWeapons.Contains(w.weaponType) &&
                e.movementType == MovementType.Follow) ?
                0.5f / e.movementSpeed : 1;
            // Healer enemies must avoid the player and search for other enemies
            fG *= (w.weaponType == WeaponType.CureSpell &&
                healerMovements.Contains(e.movementType)) ? 1.15f : 1;
            // Healer enemies must move fast to avoid the player
            fG *= w.weaponType == WeaponType.CureSpell ?
                e.movementSpeed * 1.15f : 1;

            // Calculate the base difficulty
            difficulty = (fH + fS + fM) * fG;
        }

        /// Return the multiplier factor for the input type of movement,
        /// behavior, weapon, or projectile.
        static float Multiplier<T>(
            T value
        ) {
            if (value is WeaponType)
            {
                switch (value)
                {
                    case WeaponType.None:
                        return 1.00f;
                    case WeaponType.Sword:
                        return 1.15f;
                    case WeaponType.Bow:
                        return 1.35f;
                    case WeaponType.BombThrower:
                        return 1.25f;
                    case WeaponType.Shield:
                        return 1.15f;
                    case WeaponType.CureSpell:
                        return 1.35f;
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
            individual.difficulty = -1f;
            individual.fitness = -1f;
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
            MovementType _movementType,
            float _movementSpeed,
            float _activeTime,
            float _restTime
        ) {
            health = _health;
            strength = _strength;
            attackSpeed = _attackSpeed;
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
        Shield,      // Enemy uses a shield to defend itself (Defense).
        CureSpell,   // Enemy uses magic to cure other enemies (Defense).
    }
}