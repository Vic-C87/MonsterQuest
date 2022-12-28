using System;
using UnityEngine;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        public int mySavingThrowDC { get; private set; }

        public Monster(string aDisplayName, Sprite aBodySprite, int someHitPointsMaximum, SizeCategory aSizeCategory, int aSavingThrowDC) 
            : base(aDisplayName, aBodySprite, someHitPointsMaximum, aSizeCategory)
        {
            mySavingThrowDC = aSavingThrowDC;
        }
    }
}
