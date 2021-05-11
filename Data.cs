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
        public double duration { get; set; }
        [JsonInclude]
        public List<Individual> initial { get; set; }
        [JsonInclude]
        public List<Individual> intermediary { get; set; }
        [JsonInclude]
        public List<Individual> solution { get; set; }
    }
}