using System;

namespace MonsterQuest
{
    public class Monster
    {
        public string myDisplayName { get; private set; }
        public int myHitPoints { get; private set; }

        public int mySavingThrowDC { get; private set; }

        public Monster(string aDisplayName, int someHitPoints, int aSavingThrowDC)
        {
            myDisplayName = aDisplayName;
            myHitPoints = someHitPoints;
            mySavingThrowDC = aSavingThrowDC;
        }

        public void ReactToDamage(int aDamageAmount)
        {
            myHitPoints = Math.Max(0, aDamageAmount);
        }

    }
}
