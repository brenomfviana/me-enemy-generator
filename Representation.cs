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
    /// using MAP-Elites, some slots may be empty, then the `null` option makes
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
            Enemy enemy,
            Weapon weapon
        ) {
            this.enemy = enemy;
            this.weapon = weapon;
        }

        /// Return a clone of the individual.
        ///
        /// We create a new individual by passing `enemy` and `weapon` in the
        /// Individual constructor. Since both are structs, we can copy them by
        /// value instead of doing a deep copy.
        public Individual Clone()
        {
            Individual individual = new Individual(this.enemy, this.weapon);
            individual.difficulty = this.difficulty;
            individual.fitness = this.fitness;
            individual.generation = this.generation;
            return individual;
        }

        /// Print the individual attributes.
        public void Debug()
        {
            Console.WriteLine("   " + this.generation);
            Console.WriteLine("   " + this.fitness);
            Console.WriteLine("   " + this.difficulty);
            Console.WriteLine("   " + this.enemy.health);
            Console.WriteLine("   " + this.enemy.strength);
            Console.WriteLine("   " + this.enemy.attackSpeed);
            Console.WriteLine("   " + this.enemy.movementType);
            Console.WriteLine("   " + this.enemy.movementSpeed);
            Console.WriteLine("   " + this.enemy.activeTime);
            Console.WriteLine("   " + this.enemy.restTime);
            Console.WriteLine("   " + this.weapon.weaponType);
            Console.WriteLine("   " + this.weapon.projectileType);
            Console.WriteLine("   " + this.weapon.projectileSpeed);
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
                Util.RandomFromArray(ss.rMovementType, ref rand),
                Util.RandomFloat(ss.rMovementSpeed, ref rand),
                Util.RandomFloat(ss.rActiveTime, ref rand),
                Util.RandomFloat(ss.rRestTime, ref rand)
            );
            // Create a random weapon
            Weapon w = new Weapon(
                Util.RandomFromArray(ss.rWeaponType, ref rand),
                Util.RandomFromArray(ss.rProjectileType, ref rand),
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
        public MovementType movementType;
        [JsonInclude]
        public float movementSpeed;
        [JsonInclude]
        public float activeTime;
        [JsonInclude]
        public float restTime;

        /// Enemy contructor.
        public Enemy(
            int health,
            int strength,
            float attackSpeed,
            MovementType movementType,
            float movementSpeed,
            float activeTime,
            float restTime
        ) {
            this.health = health;
            this.strength = strength;
            this.attackSpeed = attackSpeed;
            this.movementType = movementType;
            this.movementSpeed = movementSpeed;
            this.activeTime = activeTime;
            this.restTime = restTime;
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
            WeaponType weaponType,
            ProjectileType projectileType,
            float projectileSpeed
        ) {
            this.weaponType = weaponType;
            this.projectileType = projectileType;
            this.projectileSpeed = projectileSpeed;
        }
    }

    // This enum defines the movement types of enemies.
    [Serializable]
    public enum MovementType
    {
        None,     // The enemy stays still.
        Random,   // The enemy performs random 2D movements.
        Follow,   // The enemy follows the player.
        Flee,     // The enemy flees from the player.
        Random1D, // The enemy performs random horizontal or vertical movements.
        Follow1D, // The enemy follows the player horizontally or vertically.
        Flee1D,   // The enemy flees from the player horizontally or vertically.
    }

    /// This enum defines the types of weapons an enemy may have.
    [Serializable()]
    public enum WeaponType
    {
        None,    // The enemy attacks the player with barehands (Melee).
        Sword,   // The enemy uses a short sword to damage the player (Melee).
        Shotgun, // The enemy shots projectiles towards the player (Range).
        Cannon,  // The enemy shots bombs towards the player (Range).
        Shield,  // The enemy uses shields to defend themselves (Defense).
    }

    // This enum defines the projectile types of weapons.
    [Serializable]
    public enum ProjectileType
    {
        None,   // The weapon is not a projectile weapon.
        Bullet, // The weapon shots bullets.
        Bomb,   // The weapon shots bombs.
    }
}