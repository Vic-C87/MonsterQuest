using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public abstract class Creature
    {
        public string myDisplayName { get; protected set; }
        public Sprite myBodySprite { get; protected set; }
        public int myHitPointsMaximum { get; protected set; }
        public int myHitPoints { get; protected set; }

        public virtual int myArmorClass { get; }

        public SizeCategory mySizeCategory { get; protected set; }
        public float mySpaceInFeet { get; }

        public CreaturePresenter myPresenter { get; protected set; }

        public ELifeStatus myLifeStatus { get; protected set; }

        public abstract IEnumerable myDeathSavingThrows { get; }

        public int myDeathSavingThrowFailures { get; protected set; }

        public int myDeathSavingThrowSucceeses { get; protected set; }

        public abstract AbilityScores myAbilityScores { get; }

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

        public abstract IEnumerator ReactToDamage(int aDamageAmount, bool aCriticalHit = false);

        public abstract IAction Taketurn(GameState aGameState);

        public virtual IEnumerator Death(bool aCritical)
        {
            yield return myPresenter.TakeDamage(aCritical);
            myLifeStatus = ELifeStatus.Dead;
            myPresenter.UpdateStableStatus();
            yield return myPresenter.Die();
        }

        public IEnumerator Heal(int anAmount)
        {
            myHitPoints = Mathf.Min(myHitPoints + anAmount, myHitPointsMaximum);
            yield return myPresenter.Heal();
        }
    }
}
