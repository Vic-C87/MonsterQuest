using System.Collections;
using System.Collections.Generic;

namespace MonsterQuest
{
    public class Combat
    {
        public Monster myMonster { get; private set; }

        public Combat(Monster aMonster)
        {
            myMonster = aMonster;
        }
    }
}
