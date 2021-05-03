using System;

namespace OverlordEnemyGenerator
{
    /// This struct represents an enemy.
    public struct Enemy
    {
        public int health;
        public int strength;
        public float attackSpeed;
        public MovementType movementType;
        public float movementSpeed;
        public BehaviorType behaviorType;
        public float activeTime;
        public float restTime;
    }

    // This enum defines the movement types of enemies.
    public enum MovementType
    {
        // The enemy stays still.
        None,
        // The enemy performs random 2D movements.
        Random,
        // The enemy follows the player.
        Follow,
        // The enemy flees from the player.
        Flee,
        // The enemy performs random horizontal or vertical movements.
        Random1D,
        // The enemy follows the player horizontally or vertically.
        Follow1D,
        // The enemy flees from the player horizontally or vertically.
        Flee1D,
    }

    // This enum defines the behavior types of enemies.
    public enum BehaviorType
    {
        // The enemy does nothing.
        Indifferent,
        // The enemy prefers to be alone.
        LoneWolf,
        // The enemy prefers to be in a group of enemies.
        Swarm,
    }

    // This struc represents a weapon.
    public struct Weapon
    {
        public WeaponType weaponType;
        public ProjectileType projectileType;
        public float projectileSpeed;
    }

    /// This enum defines the types of weapons an enemy may have.
    public enum WeaponType
    {
        // The enemy attacks the player with barehands (Melee).
        None,
        // The enemy uses a short sword to damage the player (Melee).
        Sword,
        // The enemy shots projectiles towards the player (Range).
        Shotgun,
        // The enemy shots bombs towards the player (Range).
        Cannon,
        // The enemy uses shields to defend themselves (Defense).
        Shield,
        // The enemy uses magic to cure themselves (Defense).
        Cure,
    }

    // This enum defines the projectile types of weapons.
    public enum ProjectileType
    {
        // The weapon is not a projectile weapon.
        None,
        // The weapon shots bullets.
        Bullet,
        // The weapon shots arrows.
        Arrow,
        // The weapon shots bombs.
        Bomb,
    }



    /// This struct represents an individual.
    ///
    /// Individuals are composed of an enemy, a weapon, and a fitness value.
    public struct Individual
    {
        public Enemy enemy;
        public Weapon weapon;
        public float fitness;

        /// Get a random individual.
        public static Individual GetRandomIndividual(Random rand,
            SearchSpace ss)
        {
            // Create a random enemy
            Enemy e = new Enemy();
            e.health = SearchSpace.RandomInt(ss.health, rand);
            e.strength = SearchSpace.RandomInt(ss.strength, rand);
            e.attackSpeed = SearchSpace.RandomFloat(ss.attackSpeed, rand);
            e.movementSpeed = SearchSpace.RandomFloat(ss.movementSpeed, rand);
            e.activeTime = SearchSpace.RandomFloat(ss.activeTime, rand);
            e.restTime = SearchSpace.RandomFloat(ss.restTime, rand);
            e.movementType = SearchSpace.RandomEnum(
                SearchSpace.AllMovementTypes(), rand);
            e.behaviorType = SearchSpace.RandomEnum(
                SearchSpace.AllBehaviorTypes(), rand);
            // Create a random weapon
            Weapon w = new Weapon();
            w.weaponType = SearchSpace.RandomEnum(
                SearchSpace.AllWeaponTypes(), rand);
            w.projectileType = SearchSpace.RandomEnum(
                SearchSpace.AllProjectileTypes(), rand);
            w.projectileSpeed = SearchSpace.RandomFloat(
                ss.projectileSpeed, rand);
            // Create individual
            Individual individual = new Individual();
            individual.enemy = e;
            individual.weapon = w;
            // Return the created individual
            return individual;
        }
    }



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

        /// Return a random integer number from the given range and the given 
        /// random number generator.
        public static int RandomInt((int min, int max) range, Random rand)
        {
            return rand.Next(range.min, range.max + 1);
        }

        /// Return a random float number from the given range and the given 
        /// random number generator.
        public static float RandomFloat((float min, float max) range,
            Random rand)
        {
            double n = rand.NextDouble();
            return (float) (n * (range.max - range.min) + range.min);
        }

        /// Return a random enumerate value from the given list and the given 
        /// random number generator.
        public static T RandomEnum<T>(T[] range, Random rand)
        {
            return range[rand.Next(0, range.Length)];
        }
    }
}