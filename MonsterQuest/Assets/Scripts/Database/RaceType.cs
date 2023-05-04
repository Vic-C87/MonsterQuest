using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(menuName = "Race")]
    public class RaceType : ScriptableObject
    {
        public string myDisplayName;
        public int mySpeed;
        public string myDescription;
        public SizeCategory mySize;
        public Dictionary<EAbility, int> myAbilityScoreBonuses = new Dictionary<EAbility, int>();
    }
}
