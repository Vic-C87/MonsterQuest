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

        int myCurrentXP;
        int myXPForNextLevel;

        int myHitDieCount;

        public ClassType myClassType { get; private set; }

        protected override int myProficiencyBonusBase => myLevel;

        public Character(string aDisplayName, Sprite aBodySprite, SizeCategory aSizeCategory, WeaponType aWeaponType, ArmorType anArmorType, ClassType aClassType) 
            : base(aDisplayName, aBodySprite, aSizeCategory)
        {
            myWeaponType = aWeaponType;
            myArmorType = anArmorType;
            myClassType = aClassType;
            myAbilityScores = new (true);
            myLevel = 1;
            myHitDieCount = 1;
            myXPForNextLevel = LevelUpHelper.GetXPForNextLevel(myLevel);
            myCurrentXP = 0;
            myHitPointsMaximum = DiceHelper.Roll(myClassType.myHitDie + myAbilityScores.Constitution.Modifier.ToString());
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

        public IEnumerator GainExperiencePoints(int someXP)
        {
            myCurrentXP += someXP;

            Console.WriteLine(myDisplayName + " gains " + someXP + "XP from the battle.");

            while (myCurrentXP >= myXPForNextLevel)
            {
                yield return LevelUp();
            }
        }

        IEnumerator LevelUp()
        {
            myLevel++;
            Console.WriteLine(myDisplayName + " has reached level " + myLevel + "!");
            myXPForNextLevel = LevelUpHelper.GetXPForNextLevel(myLevel);
            int roll = DiceHelper.Roll(myClassType.myHitDie + myAbilityScores.Constitution.Modifier.ToString());
            myHitPointsMaximum += roll;
            myHitDieCount = Mathf.Min(++myHitDieCount, myLevel);
            Console.WriteLine(myDisplayName + " maximum Hit Points increase to " + myHitPointsMaximum + "!");
            yield return myPresenter.LevelUp();
        }

        public IEnumerator TakeShortRest()
        {
            while (myHitDieCount > 0 && myHitPoints < myHitPointsMaximum) // maybe change to be optional to use hit die?
            {
                int roll = DiceHelper.Roll(myClassType.myHitDie);
                myHitDieCount--;

                if (myHitPoints > 0)
                {
                    myLifeStatus = ELifeStatus.Conscious;
                }
                yield return Heal(roll);

                Console.WriteLine("After a short rest " + myDisplayName + " heals up and now has a total of " + myHitPoints + " Hit Points!");
            }
        }
    }
}
