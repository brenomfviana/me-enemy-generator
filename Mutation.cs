using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace EnemyGenerator
{
    /// This class holds the mutation operator.
    public class Mutation
    {
        /// Reproduce a new individual by mutating a parent.
        public static Individual Apply(
            Individual parent,
            int chance,
            ref Random rand
        ) {
            // New individual
            Individual individual = parent.Clone();
            // Apply mutation on enemy attributes
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.health = Util.RandomInt(
                    SearchSpace.Instance.rHealth, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.strength = Util.RandomInt(
                    SearchSpace.Instance.rStrength, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.attackSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rAttackSpeed, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.movementType = Util.RandomElementFromArray(
                    SearchSpace.Instance.rMovementType, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.movementSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rMovementSpeed, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.activeTime = Util.RandomFloat(
                    SearchSpace.Instance.rActiveTime, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.enemy.restTime = Util.RandomFloat(
                    SearchSpace.Instance.rRestTime, ref rand
                );
            }
            // Apply mutation on weapon attributes
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.weapon.weaponType = Util.RandomElementFromArray(
                    SearchSpace.Instance.rWeaponType, ref rand
                );
            }
            if (chance > Util.RandomPercent(ref rand))
            {
                individual.weapon.projectileSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rProjectileSpeed, ref rand
                );
            }
            // Return the new mutated individual
            return individual;
        }
    }
}