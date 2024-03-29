using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(menuName = "Weapon")]
    public class WeaponType : ItemType
    {
        public string myDamageRoll;
        public bool myIsRanged;
        public bool myIsFinesse;
        public List<string> myWeaponCategory = new List<string>();
    }
}
