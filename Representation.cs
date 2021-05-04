using System;

namespace OverlordEnemyGenerator
{
    /// This class represents an individual.
    ///
    /// Individuals are composed of an enemy, a weapon, and a fitness value.
    ///
    /// Why individuals are represented as a class?
    /// - When using MAP-Elites, some slots may be empty, then the `null` 
    ///   option makes easier to manage the MAP-Elites population.
    public class Individual
    {
        public Enemy enemy;
        public Weapon weapon;
        public float fitness;

        /// Return a random individual.
        public static Individual GetRandom(
            Random rand,
            SearchSpace ss
        ) {
            // Create a random enemy
            Enemy e = new Enemy();
            e.health = Util.RandomInt(ss.rHealth, rand);
            e.strength = Util.RandomInt(ss.rStrength, rand);
            e.attackSpeed = Util.RandomFloat(ss.rAttackSpeed, rand);
            e.movementType = Util.RandomList(ss.rMovementType, rand);
            e.movementSpeed = Util.RandomFloat(ss.rMovementSpeed, rand);
            e.behaviorType = Util.RandomList(ss.rBehaviorType, rand);
            e.activeTime = Util.RandomFloat(ss.rActiveTime, rand);
            e.restTime = Util.RandomFloat(ss.rRestTime, rand);
            // Create a random weapon
            Weapon w = new Weapon();
            w.weaponType = Util.RandomList(ss.rWeaponType, rand);
            w.projectileType = Util.RandomList(ss.rProjectileType, rand);
            w.projectileSpeed = Util.RandomFloat(ss.rProjectileSpeed, rand);
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

    // This struc represents a weapon.
    public struct Weapon
    {
        public WeaponType weaponType;
        public ProjectileType projectileType;
        public float projectileSpeed;
    }

    // This enum defines the movement types of enemies.
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

    // This enum defines the behavior types of enemies.
    public enum BehaviorType
    {
        Indifferent, // The enemy does nothing.
        LoneWolf,    // The enemy prefers to be alone.
        Swarm,       // The enemy prefers to be in a group of enemies.
    }

    /// This enum defines the types of weapons an enemy may have.
    public enum WeaponType
    {
        None,    // The enemy attacks the player with barehands (Melee).
        Sword,   // The enemy uses a short sword to damage the player (Melee).
        Shotgun, // The enemy shots projectiles towards the player (Range).
        Cannon,  // The enemy shots bombs towards the player (Range).
        Shield,  // The enemy uses shields to defend themselves (Defense).
        Cure,    // The enemy uses magic to cure themselves (Defense).
    }

    // This enum defines the projectile types of weapons.
    public enum ProjectileType
    {
        None,   // The weapon is not a projectile weapon.
        Bullet, // The weapon shots bullets.
        Arrow,  // The weapon shots arrows.
        Bomb,   // The weapon shots bombs.
    }
}