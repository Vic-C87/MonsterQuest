using System.Collections;
using System.Collections.Generic;

namespace MonsterQuest
{
    public static class StringHelper
    {

        public static string JoinWithAnd(List<string> aListOfItems, bool aUseSerialComma = false)
        {
            int sizeOfList = aListOfItems.Count;
            string lastItem = aListOfItems[sizeOfList - 1];
            aListOfItems.RemoveAt(sizeOfList - 1);
            
            string joinedItems = string.Join(", ", aListOfItems);

            if (aUseSerialComma)
            {
                joinedItems += ", and " + lastItem;
            }
            else
            {
                joinedItems += " and " + lastItem;
            }

            aListOfItems.Add(lastItem);

            return joinedItems;
        }
    }
}
