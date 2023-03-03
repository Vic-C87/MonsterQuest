using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        bool myEnemyIsAlive = true;
        bool myPartyAlive = true;

        public IEnumerator Simulate(GameState aGameState)
        {
            if (aGameState.myParty.Count() > 0)
            {
                myEnemyIsAlive = true;

                List<Creature> turnList = new List<Creature>();
                foreach (Creature character in aGameState.myParty.myCharacters)
                {
                    turnList.Add(character);
                }
                turnList.Add(aGameState.myCombat.myMonster);

                turnList.ShuffleList();

                Console.WriteLine("The heroes " + StringHelper.JoinWithAnd(aGameState.myParty.GetNames()) + " descend into the dungeon.");
                Console.WriteLine(aGameState.myCombat.myMonster.myDisplayName.ToUpperFirst() + " with " + aGameState.myCombat.myMonster.myHitPoints + " HP appears!");
                
                while (myEnemyIsAlive && myPartyAlive)
                {
                    for (int i = 0; i < turnList.Count; i++)
                    {
                        if (turnList[i].myLifeStatus == ELifeStatus.Dead) continue;

                        yield return turnList[i].Taketurn(aGameState).Execute();
                            
                        if (aGameState.myCombat.myMonster.myHitPoints <= 0)
                        {
                            myEnemyIsAlive = false;
                            break;
                        }
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
    }
}
