using System;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScores
    {
        
        [field: SerializeField]
        public AbilityScore Strength { get; set; }
        [field: SerializeField]
        public AbilityScore Dexterity { get; set; }
        [field: SerializeField]
        public AbilityScore Constitution { get; set; }
        [field: SerializeField]
        public AbilityScore Intelligence { get; set; }
        [field: SerializeField]
        public AbilityScore Wisdom { get; set; }
        [field: SerializeField]
        public AbilityScore Charisma { get; set; }
    }
}
