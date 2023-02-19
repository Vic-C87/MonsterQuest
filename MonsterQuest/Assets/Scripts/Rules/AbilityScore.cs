using System;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScore 
    {
        [field: SerializeField]
        public int Score { get; set; }

        [field: SerializeField]
        public int Modifier { get; private set; }
    }
}
