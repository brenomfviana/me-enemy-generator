using System;

namespace EnemyGenerator
{
    /// This class holds the mutation operator.
    public class Mutation
    {
        /// Reproduce a new individual by mutating a parent.
        public static Individual Apply(
            Individual _parent,
            int _chance,
            ref Random _rand
        ) {
            // New individual
            Individual individual = _parent.Clone();
            // Apply mutation on enemy attributes
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.enemy.health = Util.RandomInt(
                    SearchSpace.Instance.rHealth, ref _rand
                );
            }
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.enemy.strength = Util.RandomInt(
                    SearchSpace.Instance.rStrength, ref _rand
                );
            }
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.enemy.attackSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rAttackSpeed, ref _rand
                );
            }
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.enemy.movementType = Util.RandomElementFromArray(
                    SearchSpace.Instance.rMovementType, ref _rand
                );
            }
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.enemy.movementSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rMovementSpeed, ref _rand
                );
            }
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.enemy.activeTime = Util.RandomFloat(
                    SearchSpace.Instance.rActiveTime, ref _rand
                );
            }
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.enemy.restTime = Util.RandomFloat(
                    SearchSpace.Instance.rRestTime, ref _rand
                );
            }
            // Apply mutation on weapon attributes
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.weapon.weaponType = Util.RandomElementFromArray(
                    SearchSpace.Instance.rWeaponType, ref _rand
                );
            }
            if (_chance > Util.RandomPercent(ref _rand))
            {
                individual.weapon.projectileSpeed = Util.RandomFloat(
                    SearchSpace.Instance.rProjectileSpeed, ref _rand
                );
            }
            // Return the new mutated individual
            return individual;
        }
    }
}