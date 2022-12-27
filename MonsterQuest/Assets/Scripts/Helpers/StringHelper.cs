using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MonsterQuest
{
    public static class StringHelper
    {

        public static string JoinWithAnd(List<string> aListOfStrings, bool aUseSerialComma = false)
        {
            int sizeOfList = aListOfStrings.Count;
            string lastItem = aListOfStrings[sizeOfList - 1];
            aListOfStrings.RemoveAt(sizeOfList - 1);
            sizeOfList--;

            string joinedItems = string.Join(", ", aListOfStrings);

            if (aUseSerialComma)
            {
                joinedItems += ", and " + lastItem;
            }
            else
            {
                joinedItems += " and " + lastItem;
            }

            aListOfStrings.Add(lastItem);

            return joinedItems;
        }
    }
}
