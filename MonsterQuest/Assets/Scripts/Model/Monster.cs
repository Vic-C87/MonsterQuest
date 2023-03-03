using System;
using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        public MonsterType myType { get; }

        public override IEnumerable myDeathSavingThrows { get { return myDeathSavingThrowArray; } }

        static readonly bool[] myDeathSavingThrowArray = new bool[0];

        public override int myArmorClass => base.myArmorClass;

        public override AbilityScores myAbilityScores => myType.myAbilityScores;

        public Monster(MonsterType aType) 
            : base(aType.myDisplayName, aType.myBodySprite, aType.mySizeCategory)
        {
            myType = aType;
            myHitPointsMaximum = DiceHelper.Roll(myType.myHitPointsRoll);
            Initialize();
        }

        public override IAction Taketurn(GameState aGameState)
        {
            bool isAlive = false;
            int attackedHeroIndex;
            if (myAbilityScores[EAbility.Intelligence] < 7)
            {
                do
                {
                    attackedHeroIndex = DiceHelper.GetRandom(aGameState.myParty.myCharacters.Count) - 1;
                    if (aGameState.myParty.myCharacters[attackedHeroIndex].myLifeStatus != ELifeStatus.Dead)
                    {
                        isAlive = true;
                    }

                } while (!isAlive);
            }
            else
            {
                int lowestIndex = 0;
                int lowestHP = int.MaxValue;

                for (int i = 0; i < aGameState.myParty.myCharacters.Count; i++)
                {
                    if(aGameState.myParty.myCharacters[i].myLifeStatus != ELifeStatus.Dead && aGameState.myParty.myCharacters[i].myHitPoints < lowestHP)
                    {
                        lowestHP = aGameState.myParty.myCharacters[i].myHitPoints;
                        lowestIndex = i;
                    }
                }
                attackedHeroIndex = lowestIndex;
            }
            WeaponType weapon = myType.myWeaponTypes.Random();
            EAbility? ability = null;

            if (weapon.myIsFinesse)
            {
                ability = myAbilityScores[EAbility.Strenght] > myAbilityScores[EAbility.Dexterity] ? EAbility.Strenght : EAbility.Dexterity;
            }

            return new AttackAction(this, aGameState.myParty.myCharacters[attackedHeroIndex], weapon, ability);
        }

        public override IEnumerator ReactToDamage(int aDamageAmount, bool aCriticalHit = false)
        {
            bool criticalDeath = false;
            if (aDamageAmount >= myHitPoints + myHitPointsMaximum)
            {
                criticalDeath = true;
            }
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

        public string GetRandomWeaponDiceNotation()
        {
            int weaponIndex = DiceHelper.GetRandom(myType.myWeaponTypes.Length);
            weaponIndex--;
            return myType.myWeaponTypes[weaponIndex].myDamageRoll;
        }

        WeaponType GetRandomWeapon()
        {
            return myType.myWeaponTypes.Random();
        }

        public override IEnumerator Death(bool aCritical)
        {
            yield return base.Death(aCritical);
        }
    }
}
