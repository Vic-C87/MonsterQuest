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
        public List<WeaponType> myWeaponTypes = new List<WeaponType>();
        public ArmorType myArmorType;
        public int myArmorClass;
        public Sprite myBodySprite;
        public int myChallengeRating;
        public int myXPToGive;
        public AbilityScores myAbilityScores = new();
    }
}
