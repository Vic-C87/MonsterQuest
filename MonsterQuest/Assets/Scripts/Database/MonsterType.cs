using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(menuName = "Monster")]
    public class MonsterType : ScriptableObject
    {
        public string myDisplayName;
        public SizeCategory mySizeCategory;
        public string myAlignment;
        public string myHitPointsRoll;
        public WeaponType[] myWeaponTypes;
        public ArmorType myArmorType;
        public int myArmorClass;
        public Sprite myBodySprite;
    }
}
