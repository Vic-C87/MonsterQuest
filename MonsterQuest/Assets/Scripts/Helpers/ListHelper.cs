using System;
using System.Collections.Generic;

namespace MonsterQuest
{
    public static class ListHelper<T>
    {
        public static List<T> ShuffleList(IList<T> aTurnList)
        {
            List<T> shuffledList = new List<T>(aTurnList);
            System.Random random = new System.Random();
            int count = shuffledList.Count;
            int randomItem;
            T itemPicked;

            for (int i = count; i > 1; i--)
            {
                randomItem = random.Next(count);

                itemPicked = shuffledList[randomItem];
                shuffledList.RemoveAt(randomItem);
                shuffledList.Add(itemPicked);

            }
            return shuffledList;

        }
    }
}
