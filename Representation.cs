using System;

namespace OverlordEnemyGenerator
{
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
            e.health = Util.RandomInt(ss.health, rand);
            e.strength = Util.RandomInt(ss.strength, rand);
            e.attackSpeed = Util.RandomFloat(ss.attackSpeed, rand);
            e.movementSpeed = Util.RandomFloat(ss.movementSpeed, rand);
            e.activeTime = Util.RandomFloat(ss.activeTime, rand);
            e.restTime = Util.RandomFloat(ss.restTime, rand);
            e.movementType = Util.RandomEnum(
                SearchSpace.AllMovementTypes(), rand);
            e.behaviorType = Util.RandomEnum(
                SearchSpace.AllBehaviorTypes(), rand);
            // Create a random weapon
            Weapon w = new Weapon();
            w.weaponType = Util.RandomEnum(
                SearchSpace.AllWeaponTypes(), rand);
            w.projectileType = Util.RandomEnum(
                SearchSpace.AllProjectileTypes(), rand);
            w.projectileSpeed = Util.RandomFloat(ss.projectileSpeed, rand);
            // Create individual
            Individual individual = new Individual();
            individual.enemy = e;
            individual.weapon = w;
            // Return the created individual
            return individual;
        }
    }

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
}