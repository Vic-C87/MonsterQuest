using System;
using System.Collections.Generic;

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
    }
}
