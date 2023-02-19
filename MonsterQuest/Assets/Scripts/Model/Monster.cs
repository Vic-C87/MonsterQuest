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
            do
            {
                attackedHeroIndex = DiceHelper.GetRandom(aGameState.myParty.myCharacters.Count) - 1;
                if (aGameState.myParty.myCharacters[attackedHeroIndex].myLifeStatus != ELifeStatus.Dead)
                {
                    isAlive = true;
                }

            } while (!isAlive);

            return new AttackAction(this, aGameState.myParty.myCharacters[attackedHeroIndex], GetRandomWeapon());
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
            int weaponIndex = DiceHelper.GetRandom(myType.myWeaponTypes.Length);
            weaponIndex--;
            return myType.myWeaponTypes[weaponIndex];
        }

        public override IEnumerator Death(bool aCritical)
        {
            yield return base.Death(aCritical);
        }
    }
}
