using System.IO;
using System.Text.Json;

namespace EnemyGenerator
{
    /// This class holds functions handle the program inputs.
    public class Input
    {
        /// Return the individual corresponding to the entered JSON file.
        public static Individual ReadIndividual(
            string path
        ) {
            // Read and parse JSON file
            string content = File.ReadAllText(path);
            JsonDocument doc = JsonDocument.Parse(content);
            JsonElement root = doc.RootElement;
            // Build the enemy component
            JsonElement je = root.GetProperty("enemy");
            Enemy enemy = new Enemy(
                je.GetProperty("health").GetInt32(),
                je.GetProperty("strength").GetInt32(),
                (float) je.GetProperty("attackSpeed").GetDouble(),
                (MovementType) je.GetProperty("movementType").GetInt32(),
                (float) je.GetProperty("movementSpeed").GetDouble(),
                (float) je.GetProperty("activeTime").GetDouble(),
                (float) je.GetProperty("restTime").GetDouble()
            );
            // Build the weapon component
            JsonElement jw = root.GetProperty("weapon");
            Weapon weapon = new Weapon(
                (WeaponType) jw.GetProperty("weaponType").GetInt32(),
                (float) jw.GetProperty("projectileSpeed").GetDouble()
            );
            // Build and return the individual
            return new Individual(enemy, weapon);
        }
    }
}