using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        int myEnemyHP;
        [SerializeField]
        string myHeroDamageDice = "2d6";
        int myHeroDamage;
        [SerializeField]
        string myHeroConstitutionDice = "d20";
        [SerializeField]
        int myHeroConstitution = 3;
        int myConstitutionRoll;
        bool myHeroIsSaved;

        bool myEnemyIsAlive = true;


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Simulate(List<string> someHeroes, Enemy anEnemy, ref bool aHeroesAlive)
        {
            if (aHeroesAlive)
            {
                myEnemyHP = anEnemy.myHP;
                myEnemyIsAlive = true;

                Console.WriteLine("The heroes " + StringHelper.JoinWithAnd(someHeroes) + " descend into the dungeon.");
                Console.WriteLine(anEnemy.myName + " with " + myEnemyHP + " HP appears!");
                while (myEnemyHP > 0 && someHeroes.Count > 0)
                {
                    for (int i = 0; i < someHeroes.Count; i++)
                    {
                        if (myEnemyHP == 0)
                        {
                            break;
                        }
                        myHeroDamage = DiceHelper.Roll(myHeroDamageDice);
                        myEnemyHP -= myHeroDamage;
                        if (myEnemyHP <= 0)
                        {
                            myEnemyHP = 0;
                            myEnemyIsAlive = false;
                        }
                        Console.WriteLine(someHeroes[i] + " hits the " + anEnemy.myName + " for " + myHeroDamage + " damage. " + anEnemy.myName + " has " + myEnemyHP + " HP left.");
                    }
                    if (myEnemyIsAlive)
                    {
                        myConstitutionRoll = DiceHelper.Roll(myHeroConstitutionDice) + myHeroConstitution;
                        myHeroIsSaved = myConstitutionRoll >= anEnemy.myConstitutionSaveNeeded;
                        int attackedHeroIndex = DiceHelper.GetRandom(someHeroes.Count) - 1;
                        Console.WriteLine("The " + anEnemy.myName + " attacks " + someHeroes[attackedHeroIndex] + "!");
                        Console.Write(someHeroes[attackedHeroIndex] + " rolls a " + myConstitutionRoll);

                        if (myHeroIsSaved)
                        {
                            Console.WriteLine(" and is saved from the attack.");
                        }
                        else
                        {
                            Console.WriteLine(" and fails to be saved...");
                            someHeroes.RemoveAt(attackedHeroIndex);
                        }
                    }
                }
                if (!myEnemyIsAlive)
                {
                    Console.WriteLine("The " + anEnemy.myName + " collapses and the heroes celebrate their victory!");
                }
                else
                {
                    Console.WriteLine("The party has failed and the " + anEnemy.myName + " continues to attack unsuspecting adventurers.");
                    aHeroesAlive = false;
                }
            }
        }
    }
}
