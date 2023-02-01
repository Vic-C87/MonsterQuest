using System;
using System.Collections;
using System.Collections.Generic;
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

        public ELifeStatus myLifeStatus { get; protected set; }

        public List<bool> myDeathSavingThrows { get; } = new List<bool>();

        public int myDeathSavingThrowFailures { get; private set; }

        public int myDeathSavingThrowSucceeses { get; private set; }


        public Creature(string aDisplayName, Sprite aBodySprite, SizeCategory aSizeCategory)
        {
            myDisplayName = aDisplayName;
            myBodySprite = aBodySprite;
            myHitPoints = myHitPointsMaximum;
            mySizeCategory = aSizeCategory;
            mySpaceInFeet = SizeHelper.spaceInFeetPerSizeCategory[mySizeCategory];
        }

        protected void Initialize()
        {
            myHitPoints = myHitPointsMaximum;
            myLifeStatus = ELifeStatus.Conscious;
        }

        public void InitializePresenter(CreaturePresenter aPresenter)
        {
            myPresenter = aPresenter;
        }

        public virtual IEnumerator ReactToDamage(int aDamageAmount)
        {
            bool criticalDeath = false;
            if (aDamageAmount >= myHitPoints + myHitPointsMaximum)
            {
                criticalDeath = true;
            }
            if (myLifeStatus == ELifeStatus.Conscious)
            {
                myHitPoints = Math.Max(0, myHitPoints - aDamageAmount);
                if(myHitPoints == 0)
                {
                    yield return Death(criticalDeath);
                }
                else
                {
                    yield return myPresenter.TakeDamage();
                }
            }
            else
            {
                yield return myPresenter.TakeDamage();
                yield return AddFailure();
                if (criticalDeath)
                {
                    yield return AddFailure();
                }
            }
        }

        protected IEnumerator AddFailure(int? aRoll = null)
        {
            if (myLifeStatus != ELifeStatus.Dead)
            {
                myDeathSavingThrows.Add(false);
                myDeathSavingThrowFailures++;
                yield return myPresenter.PerformDeathSavingThrow(false, aRoll);
                if (myDeathSavingThrowFailures >= 3)
                {
                    myLifeStatus = ELifeStatus.Dead;
                    myPresenter.UpdateStableStatus();
                    yield return myPresenter.Die();
                }
                if (aRoll == 1)
                {
                    yield return AddFailure();
                }
            }
        }

        protected IEnumerator AddSuccess(int aRoll)
        {
            myDeathSavingThrows.Add(true);
            myDeathSavingThrowSucceeses++;
            yield return myPresenter.PerformDeathSavingThrow(true, aRoll);
            if (aRoll == 20)
            {
                myHitPoints++;
            }

            if (myDeathSavingThrowSucceeses >= 3)
            {
                myDeathSavingThrowSucceeses = 0;
                myDeathSavingThrowFailures = 0;
                myPresenter.ResetDeathSavingThrows();
                myLifeStatus = ELifeStatus.Conscious;
                myHitPoints++;
                myPresenter.UpdateStableStatus();
                yield return myPresenter.RegainConsciousness();
            }
        }

        public virtual IEnumerator Death(bool aCritical)
        {
            yield return myPresenter.TakeDamage(aCritical);
            myLifeStatus = ELifeStatus.Dead;
            myPresenter.UpdateStableStatus();
            yield return myPresenter.Die();
        }


    }
}
