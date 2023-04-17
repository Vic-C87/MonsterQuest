using System;
using System.Collections;
using System.Collections.Generic;

namespace MonsterQuest
{
    [Serializable]
    public class GameState
    {
        public Party myParty { get; private set; }
        public Combat myCombat { get; private set; }

        List<Monster> myMonstersToFight = new List<Monster>();

        int myMonsterIndex = 0;

        public GameState(Party aParty, List<Monster> someMonsters)
        {
            myParty = aParty;
            myMonstersToFight = someMonsters;
        }

        public bool EnterCombatWithMonster()
        {
            if (myMonstersToFight[myMonsterIndex].myLifeStatus == ELifeStatus.Dead)
            {
                myMonsterIndex++;
                if (myMonsterIndex >= myMonstersToFight.Count)
                {
                    return false;
                }
                else
                {
                    myCombat = new Combat(myMonstersToFight[myMonsterIndex], this);
                    return true;
                }
            }
            else
            {
                myCombat = new Combat(myMonstersToFight[myMonsterIndex], this);
                return true;
            }
            
        }
    }
}
