using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        int myDamageDealt;
        bool myEnemyIsAlive = true;

        public IEnumerator Simulate(GameState aGameState)
        {
            if (aGameState.myParty.Count() > 0)
            {
                myEnemyIsAlive = true;

                Console.WriteLine("The heroes " + StringHelper.JoinWithAnd(aGameState.myParty.GetNames()) + " descend into the dungeon.");
                Console.WriteLine(aGameState.myCombat.myMonster.myDisplayName + " with " + aGameState.myCombat.myMonster.myHitPoints + " HP appears!");
                while (aGameState.myCombat.myMonster.myHitPoints > 0 && aGameState.myParty.Count() > 0)
                {
                    for (int i = 0; i < aGameState.myParty.Count(); i++)
                    {
                        if (aGameState.myCombat.myMonster.myHitPoints == 0)
                        {
                            myEnemyIsAlive = false;
                            break;
                        }
                        myDamageDealt = DiceHelper.Roll(aGameState.myParty.myCharacters[i].myWeaponType.myDamageRoll);
                        yield return aGameState.myParty.myCharacters[i].myPresenter.Attack();
                        yield return aGameState.myCombat.myMonster.ReactToDamage(myDamageDealt);
                        Console.WriteLine(aGameState.myParty.myCharacters[i].myDisplayName + " hits the " + aGameState.myCombat.myMonster.myDisplayName + " with " + aGameState.myParty.myCharacters[i].myWeaponType.myDisplayName + " for " + myDamageDealt + " damage. " + aGameState.myCombat.myMonster.myDisplayName + " has " + aGameState.myCombat.myMonster.myHitPoints + " HP left.");
                        if (aGameState.myCombat.myMonster.myHitPoints == 0)
                        {
                            myEnemyIsAlive = false;
                            
                        }
                    }
                    if (myEnemyIsAlive)
                    {
                        int attackedHeroIndex = DiceHelper.GetRandom(aGameState.myParty.Count()) - 1;
                        int randomWeaponIndex = DiceHelper.GetRandom(aGameState.myCombat.myMonster.myType.myWeaponTypes.Length) - 1;
                        myDamageDealt = DiceHelper.Roll(aGameState.myCombat.myMonster.myType.myWeaponTypes[randomWeaponIndex].myDamageRoll);
                        yield return aGameState.myCombat.myMonster.myPresenter.Attack();
                        yield return aGameState.myParty.myCharacters[attackedHeroIndex].ReactToDamage(myDamageDealt);
                        Console.WriteLine("The " + aGameState.myCombat.myMonster.myDisplayName + " attacks " + aGameState.myParty.myCharacters[attackedHeroIndex].myDisplayName + " with " + aGameState.myCombat.myMonster.myType.myWeaponTypes[randomWeaponIndex].myDisplayName + " dealing " + myDamageDealt + " damage. " + aGameState.myParty.myCharacters[attackedHeroIndex].myDisplayName + " has " + aGameState.myParty.myCharacters[attackedHeroIndex].myHitPoints + " HP left.");
                        if (aGameState.myParty.myCharacters[attackedHeroIndex].myHitPoints == 0)
                        {
                            aGameState.myParty.myCharacters.RemoveAt(attackedHeroIndex);
                        }
                    }
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
    }
}
