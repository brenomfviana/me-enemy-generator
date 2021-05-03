using System;

namespace OverlordEnemyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define evolutionary parameters
            Parameters p;
            p.seed = 0;
            p.generations = 10;
            p.rIndividuals = 10;
            p.offspring = 1;
            p.mutation = 10;
            p.crossover = 90;

            // Define the search space
            p.space.health = (1, 5);
            p.space.strength = (1, 4);
            p.space.attackSpeed = (0.75f, 4f);
            p.space.movementSpeed = (0.8f, 3.2f);
            p.space.activeTime = (1.5f, 10f);
            p.space.restTime = (0.3f, 1.5f);
            p.space.projectileSpeed = (1f, 4f);
            p.space.movementType = SearchSpace.AllMovementTypes();
            p.space.behavior = SearchSpace.AllBehaviorTypes();
            p.space.weaponType = SearchSpace.AllWeaponTypes();
            p.space.projectileType = SearchSpace.AllProjectileTypes();

            // Generate enemies
            Evolution.Evolve(p);
        }
    }
}
