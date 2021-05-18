using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OverlordEnemyGenerator
{
    /// This struct holds the most relevant data of the evolutionary process.
    [Serializable]
    public struct Data
    {
        [JsonInclude]
        public int seed { get; set; }
        [JsonInclude]
        public int generations { get; set; }
        [JsonInclude]
        public int initialPopSize { get; set; }
        [JsonInclude]
        public int offspringSize { get; set; }
        [JsonInclude]
        public float goal { get; set; }
        [JsonInclude]
        public double duration { get; set; }
        [JsonInclude]
        public List<Individual> initial { get; set; }
        [JsonInclude]
        public List<Individual> intermediate { get; set; }
        [JsonInclude]
        public List<Individual> solution { get; set; }
    }
}