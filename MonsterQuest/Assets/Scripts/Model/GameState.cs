using System.Collections;
using System.Collections.Generic;

namespace MonsterQuest
{
    public class GameState
    {
        public Party myParty { get; private set; }
        public Combat myCombat { get; set; }

        public GameState(Party aParty)
        {
            myParty = aParty;
        }

        public void EnterCombatWithMonster(Monster aMonster)
        {
            myCombat = new Combat(aMonster);
        }
    }
}
