using System;
using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        public MonsterType myType { get; }
        public Monster(MonsterType aType) 
            : base(aType.myDisplayName, aType.myBodySprite, aType.mySizeCategory)
        {
            myType = aType;
            myHitPointsMaximum = DiceHelper.Roll(myType.myHitPointsRoll);
            Initialize();
        }

        public string GetRandomWeaponDiceNotation()
        {
            int weaponIndex = DiceHelper.GetRandom(myType.myWeaponTypes.Length);
            weaponIndex--;
            return myType.myWeaponTypes[weaponIndex].myDamageRoll;
        }

        public override IEnumerator Death(bool aCritical)
        {
            yield return base.Death(aCritical);
        }
    }
}
