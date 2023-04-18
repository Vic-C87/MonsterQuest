using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class Character : Creature
    {
        public WeaponType myWeaponType { get; private set; }
        public ArmorType myArmorType { get; private set; }

        public override IEnumerable myDeathSavingThrows { get { return myDeathSavingThrowsList; } }

        [field: SerializeField]List<bool> myDeathSavingThrowsList = new List<bool>();

        public override int myArmorClass => base.myArmorClass;

        public override AbilityScores myAbilityScores { get; }

        public int myLevel { get; private set; }

        public ClassType myClassType { get; private set; }

        protected override int myProficiencyBonusBase => myLevel;

        public Character(string aDisplayName, Sprite aBodySprite, int someHitPointsMaximum, SizeCategory aSizeCategory, WeaponType aWeaponType, ArmorType anArmorType, ClassType aClassType) 
            : base(aDisplayName, aBodySprite, aSizeCategory)
        {
            myHitPointsMaximum = someHitPointsMaximum;
            myWeaponType = aWeaponType;
            myArmorType = anArmorType;
            myClassType = aClassType;
            myAbilityScores = new (true);
            myLevel = 1;
            Initialize();
        }

        public override IAction Taketurn(GameState aGameState)
        {
            if (myLifeStatus == ELifeStatus.Conscious)
            {
                EAbility? ability = null;
                if (myWeaponType.myIsFinesse)
                {
                    ability = myAbilityScores[EAbility.Strength] > myAbilityScores[EAbility.Dexterity] ? EAbility.Strength : EAbility.Dexterity;
                }
                return new AttackAction(this, aGameState.myCombat.Monster, myWeaponType, ability);
            }
            else
            {
                return new BeUnconsciousAction(this);
            }
        }

        public override IEnumerator ReactToDamage(int aDamageAmount, bool aCriticalHit = false)
        {
            bool criticalDeath = false;
            if (aDamageAmount >= myHitPoints + myHitPointsMaximum)
            {
                criticalDeath = true;
            }
            if (myLifeStatus == ELifeStatus.Conscious)
            {
                myHitPoints = Math.Max(0, myHitPoints - aDamageAmount);
                if (myHitPoints == 0)
                {
                    yield return Death(criticalDeath);
                }
                else
                {
                    yield return myPresenter.TakeDamage();
                }
            }
            else
            {
                yield return myPresenter.TakeDamage();
                yield return AddFailure();
                if (criticalDeath)
                {
                    yield return AddFailure();
                }
            }
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

        protected IEnumerator AddFailure(int? aRoll = null)
        {
            if (myLifeStatus != ELifeStatus.Dead)
            {
                myDeathSavingThrowsList.Add(false);
                myDeathSavingThrowFailures++;
                yield return myPresenter.PerformDeathSavingThrow(false, aRoll);
                if (myDeathSavingThrowFailures >= 3)
                {
                    myLifeStatus = ELifeStatus.Dead;
                    myPresenter.UpdateStableStatus();
                    yield return myPresenter.Die();
                }
                if (aRoll == 1)
                {
                    yield return AddFailure();
                }
            }
        }

        protected IEnumerator AddSuccess(int aRoll)
        {
            myDeathSavingThrowsList.Add(true);
            myDeathSavingThrowSucceeses++;
            yield return myPresenter.PerformDeathSavingThrow(true, aRoll);
            if (aRoll == 20)
            {
                int amountToHeal = 1;  
                yield return Heal(amountToHeal);
            }

            if (myDeathSavingThrowSucceeses >= 3)
            {
                myDeathSavingThrowSucceeses = 0;
                myDeathSavingThrowFailures = 0;
                myPresenter.ResetDeathSavingThrows();
                myDeathSavingThrowsList.Clear();
                myLifeStatus = ELifeStatus.Conscious;
                myHitPoints++;
                myPresenter.UpdateStableStatus();
                yield return myPresenter.RegainConsciousness();
            }
        }

        public override bool IsProficientWithWeaponType(WeaponType aWeaponType)
        {
            for (int i = 0; i < aWeaponType.myWeaponCategory.Length; i++) 
            {
                if (myClassType.myWeaponProficiencies.Contains(aWeaponType.myWeaponCategory[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
