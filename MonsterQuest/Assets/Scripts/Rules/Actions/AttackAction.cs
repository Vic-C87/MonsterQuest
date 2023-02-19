using System.Collections;
using System.Collections.Generic;

namespace MonsterQuest
{
    public class AttackAction : IAction
    {
        Creature myAttacker;
        Creature myTarget;

        WeaponType myWeaponType;

        public AttackAction(Creature anAttacker, Creature aTarget, WeaponType aWeaponType)
        {
            myAttacker = anAttacker;
            myTarget = aTarget;
            myWeaponType = aWeaponType;

        }

        public IEnumerator Execute()
        {
            yield return myAttacker.myPresenter.FaceCreature(myTarget);
            int attackRoll = DiceHelper.Roll("d20");
            int damage;
            if (myTarget.myLifeStatus == ELifeStatus.Conscious)
            {
                if (attackRoll >= myTarget.myArmorClass && attackRoll != 1)
                {
                    damage = DiceHelper.Roll(myWeaponType.myDamageRoll);
                    bool critical = attackRoll == 20;
                    yield return myAttacker.myPresenter.Attack();
                    yield return myTarget.ReactToDamage(damage, critical);
                    Console.WriteLine(myAttacker.myDisplayName + " hits " + myTarget.myDisplayName + " with " + myWeaponType.myDisplayName + " for " + damage + " damage. " + myTarget.myDisplayName + " has " + myTarget.myHitPoints + " HP left.");
                }
                else
                {
                    yield return myAttacker.myPresenter.Attack();
                    Console.WriteLine(myAttacker.myDisplayName + " misses!");
                }
            }
            else
            {
                yield return myAttacker.myPresenter.Attack();
                yield return myTarget.ReactToDamage(0, true);
                Console.WriteLine(myTarget.myDisplayName + " is unconscious and the attack is therefore a critical hit!");
            }
        }
    }
}
