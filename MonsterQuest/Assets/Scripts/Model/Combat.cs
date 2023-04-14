using System;
using System.Collections;
using System.Collections.Generic;

namespace MonsterQuest
{
    [Serializable]
    public class Combat
    {
        public Monster Monster { get; private set; }

        List<Creature> myCreaturesInOrderOfInitiative;
        int myCurrentCreatureIndex;

        public Combat(Monster aMonster, GameState aGameState)
        {
            Monster = aMonster;            

            myCreaturesInOrderOfInitiative= new List<Creature>();
            foreach(Creature creature in aGameState.myParty.myCharacters) 
            {
                myCreaturesInOrderOfInitiative.Add(creature);
            }

            myCreaturesInOrderOfInitiative.Add(aMonster);

            ListHelper.SortByRoll(myCreaturesInOrderOfInitiative);

            myCurrentCreatureIndex = -1;
        }

        public Creature StartNextCreatureTurn()
        {
            myCurrentCreatureIndex++;

            if (myCurrentCreatureIndex >= myCreaturesInOrderOfInitiative.Count)
            {
                myCurrentCreatureIndex = 0;
            }

            return myCreaturesInOrderOfInitiative[myCurrentCreatureIndex];

        }
    }
}
