using System;
using System.Collections;
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

        public override IEnumerator Death(bool aCritical)
        {
            if (aCritical)
            {
                yield return base.Death(aCritical);
            }
            else
            {
                yield return myPresenter.TakeDamage();
                myLifeStatus = ELifeStatus.UnconsciousUnstable;
                myPresenter.UpdateStableStatus();
            }
        }

        public IEnumerator DeathSavingThrow(int aRoll)
        {
            int scoreToBeat = 10;
            if (aRoll >= scoreToBeat)
            {
                yield return AddSuccess(aRoll);
                Console.WriteLine("Success!");
                if (myLifeStatus != ELifeStatus.Conscious)
                {
                    Console.WriteLine(myDisplayName + " has a total of " + myDeathSavingThrowSucceeses + " successfull rolls.");

                }
            }
            else
            {
                yield return AddFailure(aRoll);
                Console.WriteLine("Failure!" + myDisplayName + " has a total of " + myDeathSavingThrowFailures + " failed rolls.");
            }
        }
    }
}
