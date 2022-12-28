using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField]
        string myHeroDamageDice = "2d6";
        [SerializeField]
        string myHeroConstitutionDice = "d20";
        [SerializeField]
        int myHeroConstitution = 3;
        
        int myHeroDamage;
        int myConstitutionRoll;
        bool myHeroIsSaved;

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
                        myHeroDamage = DiceHelper.Roll(myHeroDamageDice);
                        yield return aGameState.myParty.myCharacters[i].myPresenter.Attack();
                        yield return aGameState.myCombat.myMonster.ReactToDamage(myHeroDamage);
                        if (aGameState.myCombat.myMonster.myHitPoints == 0)
                        {
                            myEnemyIsAlive = false;
                            
                        }
                        Console.WriteLine(aGameState.myParty.myCharacters[i].myDisplayName + " hits the " + aGameState.myCombat.myMonster.myDisplayName + " for " + myHeroDamage + " damage. " + aGameState.myCombat.myMonster.myDisplayName + " has " + aGameState.myCombat.myMonster.myHitPoints + " HP left.");
                    }
                    if (myEnemyIsAlive)
                    {
                        myConstitutionRoll = DiceHelper.Roll(myHeroConstitutionDice) + myHeroConstitution;
                        myHeroIsSaved = myConstitutionRoll >= aGameState.myCombat.myMonster.mySavingThrowDC;
                        int attackedHeroIndex = DiceHelper.GetRandom(aGameState.myParty.Count()) - 1;
                        yield return aGameState.myCombat.myMonster.myPresenter.Attack();
                        Console.WriteLine("The " + aGameState.myCombat.myMonster.myDisplayName + " attacks " + aGameState.myParty.myCharacters[attackedHeroIndex].myDisplayName + "!");
                        Console.Write(aGameState.myParty.myCharacters[attackedHeroIndex].myDisplayName + " rolls a " + myConstitutionRoll);

                        if (myHeroIsSaved)
                        {
                            Console.WriteLine(" and is saved from the attack.");
                        }
                        else
                        {
                            Console.WriteLine(" and fails to be saved...");
                            yield return aGameState.myParty.myCharacters[attackedHeroIndex].ReactToDamage(10);
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
