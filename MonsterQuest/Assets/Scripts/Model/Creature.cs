using System;
using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public class Creature
    {
        public string myDisplayName { get; protected set; }
        public Sprite myBodySprite { get; protected set; }
        public int myHitPointsMaximum { get; protected set; }
        public int myHitPoints { get; protected set; }
        public SizeCategory mySizeCategory { get; protected set; }
        public float mySpaceInFeet { get; }

        public CreaturePresenter myPresenter { get; protected set; }

        public Creature(string aDisplayName, Sprite aBodySprite,int someHitPointsMaximum, SizeCategory aSizeCategory)
        {
            myDisplayName = aDisplayName;
            myBodySprite = aBodySprite;
            myHitPointsMaximum = someHitPointsMaximum;
            myHitPoints = myHitPointsMaximum;
            mySizeCategory = aSizeCategory;
            mySpaceInFeet = SizeHelper.spaceInFeetPerSizeCategory[mySizeCategory];
        }

        public void InitializePresenter(CreaturePresenter aPresenter)
        {
            myPresenter = aPresenter;
        }

        public IEnumerator ReactToDamage(int aDamageAmount)
        {
            myHitPoints = Math.Max(0, myHitPoints - aDamageAmount);
            if(myHitPoints == 0)
            {
                yield return myPresenter.Die();
            }
            else
            {
                yield return myPresenter.TakeDamage();
            }
        }
    }
}
