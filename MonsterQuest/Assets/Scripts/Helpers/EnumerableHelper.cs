using System;
using System.Collections.Generic;
using System.Collections;

namespace MonsterQuest
{
    public static class EnumerableHelper 
    {
       
        public static T Random<T>(this IEnumerable<T> anEnumerable)
        {
            T itemToReturn = default(T);
            int count = 0;
            foreach(T item in anEnumerable)
            {
                count++;
            }

            int index = DiceHelper.GetRandom(count);
            

            count = 0;

            foreach (T item in anEnumerable)
            {
                count++;
                if (index == count)
                {
                    itemToReturn = item;
                }
            }

            return itemToReturn;
        }
    }
}
