using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(menuName = "Class")]
    public class ClassType : ScriptableObject
    {
        public string myDisplayName;
        public List<EWeaponCategory> myWeaponProficiencies;
    }
}
