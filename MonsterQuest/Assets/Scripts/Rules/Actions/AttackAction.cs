using System.Collections;
using System.Collections.Generic;
using System;

namespace MonsterQuest
{
    public class AttackAction : IAction
    {
        Creature myAttacker;
        Creature myTarget;

        WeaponType myWeaponType;

        EAbility? myAbility;

        public AttackAction(Creature anAttacker, Creature aTarget, WeaponType aWeaponType, EAbility? anAbility = null)
        {
            myAttacker = anAttacker;
            myTarget = aTarget;
            myWeaponType = aWeaponType;
            myAbility = anAbility;
        }

        public IEnumerator Execute()
        {
            yield return myAttacker.myPresenter.FaceCreature(myTarget);
            int modifier;
            if(myWeaponType.myIsFinesse && myAbility.HasValue)
            {
                modifier = myAttacker.myAbilityScores[(EAbility)myAbility].Modifier;
            }
            else if (myWeaponType.myIsRanged)
            {
                modifier = myAttacker.myAbilityScores[EAbility.Dexterity].Modifier;
            }
            else
            {
                modifier = myAttacker.myAbilityScores[EAbility.Strength].Modifier;
            }
            int attackRoll = DiceHelper.Roll("d20") + modifier;
            
            if (myAttacker.IsProficientWithWeaponType(myWeaponType)) 
            {
                attackRoll += myAttacker.myProficiencyBonus;
            }

            int damage;
            if (myTarget.myLifeStatus == ELifeStatus.Conscious)
            {
                if (attackRoll >= myTarget.myArmorClass && attackRoll != 1)
                {
                    damage = Math.Max(1, DiceHelper.Roll(myWeaponType.myDamageRoll) + modifier);

                    bool critical = attackRoll == 20;
                    yield return myAttacker.myPresenter.Attack();
                    yield return myTarget.ReactToDamage(damage, critical);
                    Console.WriteLine(myAttacker.myDisplayName.ToUpperFirst() + " hits " + myTarget.myDisplayName + " with " + myWeaponType.myDisplayName + " for " + damage + " damage. " + myTarget.myDisplayName.ToUpperFirst() + " has " + myTarget.myHitPoints + " HP left.");
                }
                else
                {
                    yield return myAttacker.myPresenter.Attack();
                    Console.WriteLine(myAttacker.myDisplayName.ToUpperFirst() + " misses!");
                }
            }
            else
            {
                yield return myAttacker.myPresenter.Attack();
                yield return myTarget.ReactToDamage(0, true);
                Console.WriteLine(myTarget.myDisplayName.ToUpperFirst() + " is unconscious and the attack is therefore a critical hit!");
            }
        }
    }
}
