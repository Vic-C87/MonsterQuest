using System;
using System.Collections.Generic;
using System.Linq;

namespace MonsterQuest
{
    public static class ListHelper
    {
        public static void ShuffleList<T>(this IList<T> aTurnList) 
        {
            List<T> shuffledList = new List<T>(aTurnList);
            System.Random random = new System.Random();
            int count = aTurnList.Count;
            int randomItem;
            T itemPicked;

            for (int i = count; i > 1; i--)
            {
                randomItem = random.Next(count);

                itemPicked = aTurnList[randomItem];
                aTurnList.RemoveAt(randomItem);
                aTurnList.Add(itemPicked);

            }
        }

        public static void SortByRoll(List<Creature> aList)
        {
            Dictionary<Creature, int> rolls = new Dictionary<Creature, int>();
            List<int> tempList = new List<int>();

            foreach (Creature creature in aList) 
            {
                int roll = DiceHelper.Roll("d20+" + creature.myAbilityScores.Dexterity);
                rolls.Add(creature, roll);
            }

            tempList = rolls.Values.ToList();
            tempList.Sort();

            aList = new List<Creature>();

            foreach (int rollValue in tempList)
            {
                foreach (KeyValuePair<Creature, int> kvp in rolls)
                {
                    if (rollValue == kvp.Value && !aList.Contains(kvp.Key))
                    {
                        aList.Add(kvp.Key);
                    }
                }
            }
        }
    }
}
