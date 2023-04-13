using System.Collections;
using System.Collections.Generic;
using System;

namespace MonsterQuest
{
    public static class Helpers 
    {
        public static SizeCategory GetSizeCategory(string aStringSize)
        {
            foreach (SizeCategory size in Enum.GetValues(typeof(SizeCategory)))
            {
                if (size.ToString().ToLower() == aStringSize.ToLower())
                    return size;
            }

            return SizeCategory.None;
        }
    }
}
