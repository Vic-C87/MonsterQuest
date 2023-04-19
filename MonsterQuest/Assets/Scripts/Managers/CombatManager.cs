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
            if (aGameState.myParty.OneAlive())
            {
                myEnemyIsAlive = true;

                Console.WriteLine("The heroes " + StringHelper.JoinWithAnd(aGameState.myParty.GetNames()) + " descend into the dungeon.");
                Console.WriteLine(aGameState.myCombat.Monster.myDisplayName.ToUpperFirst() + " with " + aGameState.myCombat.Monster.myHitPoints + " HP appears!");
                
                while (myEnemyIsAlive && myPartyAlive)
                {
                    SaveGameHelper.Save(aGameState);
                    Creature currentActor = aGameState.myCombat.StartNextCreatureTurn();
                    if (currentActor.myLifeStatus == ELifeStatus.Dead) continue;

                    yield return currentActor.Taketurn(aGameState).Execute();

                    if (aGameState.myCombat.Monster.myHitPoints <= 0)
                    {
                        myEnemyIsAlive = false;
                        break;
                    }

                    myPartyAlive = aGameState.myParty.OneAlive();
                }
                if (!myEnemyIsAlive)
                {
                    Console.WriteLine("The " + aGameState.myCombat.Monster.myDisplayName + " collapses and the heroes celebrate their victory!");
                    yield return RewardHeroes(aGameState);
                    yield return RestHeroes(aGameState);
                }
                else
                {
                    Console.WriteLine("The party has failed and the " + aGameState.myCombat.Monster.myDisplayName + " continues to attack unsuspecting adventurers.");
                }
            }
        }

        IEnumerator RewardHeroes(GameState aGameState)
        {
            int xpToShare = aGameState.myCombat.Monster.myType.myXPToGive;
            int modulusXP = xpToShare % aGameState.myParty.Count();

            xpToShare = xpToShare / aGameState.myParty.Count();

            foreach (Character character in aGameState.myParty.myCharacters) 
            {
                if (character.myLifeStatus != ELifeStatus.Dead) 
                {
                    yield return character.GainExperiencePoints(xpToShare + modulusXP);
                    modulusXP = 0; 
                }
            }
        }

        IEnumerator RestHeroes(GameState aGameState) 
        {
            foreach (Character character in aGameState.myParty.myCharacters)
            {
                if (character.myLifeStatus != ELifeStatus.Dead)
                {
                    yield return character.TakeShortRest();
                }
            }
        }
    }
}
