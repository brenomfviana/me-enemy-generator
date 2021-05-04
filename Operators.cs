namespace OverlordEnemyGenerator
{
    /// .
    public class Operators
    {
        /// .
        public static float Fitness(
            Individual individual
        ) {
            // Get enemy and weapon components
            Enemy e = individual.enemy;
            Weapon w = individual.weapon;
            // Calculate fitness
            float fH = e.health;
            float fA = e.activeTime;
            float fR = 1 / e.restTime;
            float fD = e.strength; // * damageMultiplier;
            float fM = e.movementSpeed; // * movementMultiplier;
            float fP = (e.attackSpeed + w.projectileSpeed); // * projectileMultiplier
            // Sum all difficulty factors
            return fH + fA + fR + fD + fM + fP;
        }
    }
}