using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        int myDamageDealt;
        bool myEnemyIsAlive = true;
        bool myPartyAlive = true;

        public IEnumerator Simulate(GameState aGameState)
        {
            if (aGameState.myParty.Count() > 0)
            {
                myEnemyIsAlive = true;

                Console.WriteLine("The heroes " + StringHelper.JoinWithAnd(aGameState.myParty.GetNames()) + " descend into the dungeon.");
                Console.WriteLine(aGameState.myCombat.myMonster.myDisplayName + " with " + aGameState.myCombat.myMonster.myHitPoints + " HP appears!");
                while (aGameState.myCombat.myMonster.myHitPoints > 0 && aGameState.myParty.Count() > 0 && myPartyAlive)
                {
                    myPartyAlive = false;
                    for (int i = 0; i < aGameState.myParty.Count(); i++)
                    {
                        if (aGameState.myCombat.myMonster.myHitPoints == 0)
                        {
                            myEnemyIsAlive = false;
                            break;
                        }

                        if (aGameState.myParty.myCharacters[i].myLifeStatus == ELifeStatus.Dead)
                        {
                            continue;
                        }

                        if (aGameState.myParty.myCharacters[i].myLifeStatus == ELifeStatus.Conscious)
                        {
                            myDamageDealt = DiceHelper.Roll(aGameState.myParty.myCharacters[i].myWeaponType.myDamageRoll);
                            yield return aGameState.myParty.myCharacters[i].myPresenter.Attack();
                            yield return aGameState.myCombat.myMonster.ReactToDamage(myDamageDealt);
                            Console.WriteLine(aGameState.myParty.myCharacters[i].myDisplayName + " hits the " + aGameState.myCombat.myMonster.myDisplayName + " with " + aGameState.myParty.myCharacters[i].myWeaponType.myDisplayName + " for " + myDamageDealt + " damage. " + aGameState.myCombat.myMonster.myDisplayName + " has " + aGameState.myCombat.myMonster.myHitPoints + " HP left.");
                            if (aGameState.myCombat.myMonster.myHitPoints == 0)
                            {
                                myEnemyIsAlive = false;
                            }
                        }
                        else
                        {
                            int roll = DiceHelper.Roll("d20");
                            Console.WriteLine(aGameState.myParty.myCharacters[i].myDisplayName + " is unconscious and has to perform a seath saving roll: " + roll);
                            yield return aGameState.myParty.myCharacters[i].DeathSavingThrow(roll);
                     
                            if (aGameState.myParty.myCharacters[i].myLifeStatus == ELifeStatus.Conscious)
                            {
                                Console.WriteLine(aGameState.myParty.myCharacters[i].myDisplayName + " regains consciousness!");
                            }
                        }
                    }

                    if (myEnemyIsAlive && aGameState.myParty.OneAlive())
                    {
                        int attackedHeroIndex = GetAttackIndex(aGameState);
                        int randomWeaponIndex = DiceHelper.GetRandom(aGameState.myCombat.myMonster.myType.myWeaponTypes.Length) - 1;
                        myDamageDealt = DiceHelper.Roll(aGameState.myCombat.myMonster.myType.myWeaponTypes[randomWeaponIndex].myDamageRoll);
                        yield return aGameState.myCombat.myMonster.myPresenter.Attack();
                        yield return aGameState.myParty.myCharacters[attackedHeroIndex].ReactToDamage(myDamageDealt);
                        Console.WriteLine("The " + aGameState.myCombat.myMonster.myDisplayName + " attacks " + aGameState.myParty.myCharacters[attackedHeroIndex].myDisplayName + " with " + aGameState.myCombat.myMonster.myType.myWeaponTypes[randomWeaponIndex].myDisplayName + " dealing " + myDamageDealt + " damage. " + aGameState.myParty.myCharacters[attackedHeroIndex].myDisplayName + " has " + aGameState.myParty.myCharacters[attackedHeroIndex].myHitPoints + " HP left.");
                    }

                    myPartyAlive = aGameState.myParty.OneAlive();
                }
                if (!myEnemyIsAlive)
                {
                    Console.WriteLine("The " + aGameState.myCombat.myMonster.myDisplayName + " collapses and the heroes celebrate their victory!");
                }
                else
                {
                    Console.WriteLine("The party has failed and the " + aGameState.myCombat.myMonster.myDisplayName + " continues to attack unsuspecting adventurers.");
                }
            }
        }

        int GetAttackIndex(GameState aGameState)
        {
            bool isAlive = false;
            int attackedHeroIndex;
            do
            {
                attackedHeroIndex = DiceHelper.GetRandom(aGameState.myParty.Count()) - 1;
                if (aGameState.myParty.myCharacters[attackedHeroIndex].myLifeStatus != ELifeStatus.Dead)
                {
                    isAlive = true;
                }

            } while (!isAlive);
            return attackedHeroIndex;
        }
    }
}
