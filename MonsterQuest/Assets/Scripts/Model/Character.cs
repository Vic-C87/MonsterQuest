using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        public Character(string aDisplayName, Sprite aBodySprite, int someHitPointsMaximum, SizeCategory aSizeCategory) 
            : base(aDisplayName, aBodySprite, someHitPointsMaximum, aSizeCategory)
        {
         
        }
    }
}
