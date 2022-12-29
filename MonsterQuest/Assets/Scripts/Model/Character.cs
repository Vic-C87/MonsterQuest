using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        public WeaponType myWeaponType { get; private set; }
        public ArmorType myArmorType { get; private set; }
        public Character(string aDisplayName, Sprite aBodySprite, int someHitPointsMaximum, SizeCategory aSizeCategory, WeaponType aWeaponType, ArmorType anArmorType) 
            : base(aDisplayName, aBodySprite, aSizeCategory)
        {
            myHitPointsMaximum = someHitPointsMaximum;
            myWeaponType = aWeaponType;
            myArmorType = anArmorType;
            Initialize();
        }
    }
}
