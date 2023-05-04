using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(menuName = "Armor")]
    public class ArmorType : ItemType
    {
        public ArmorCategory myArmorCategory;
        public int myArmorClass;
    }
}
