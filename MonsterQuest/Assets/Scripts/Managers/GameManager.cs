using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        List<string> myHeroes = new List<string>();
        int myEnemyHP;
        const string myHeroDamageDice = "2d6";
        int myHeroDamage;
        const string myHeroConstitutionDice = "d20";
        const int myHeroConstitution = 3;
        int myConstitutionRoll;
        bool myHeroIsSaved;

        bool myEnemyIsAlive = true;
        bool myHeroesAreAlive = true;

        Enemy myOrc;
        Enemy myAzer;
        Enemy myTroll;

        void Start()
        {
            SetHeroList("Jazlyn", "Theron", "Dayana", "Rolando");
            myOrc = new Enemy("Orc", "2d8+6", 10);
            myAzer = new Enemy("Azer", "6d8+12", 18);
            myTroll = new Enemy("Troll", "8d10+40", 16);
            SimulateBattle(myOrc);
            SimulateBattle(myAzer);
            SimulateBattle(myTroll);
            if (myHeroesAreAlive)
            {
                string stillAlive;
                if (myHeroes.Count == 1)
                {
                    stillAlive = " " + myHeroes[0];
                }
                else
                {
                    stillAlive = "es " + StringHelper.JoinWithAnd(myHeroes);
                }
                Console.WriteLine("After three grueling battles, the hero" + stillAlive + " return from the dungeons to live another day.");
            }
        }

        void SimulateBattle(Enemy anEnemy)
        {
            if (myHeroesAreAlive)
            {
                myEnemyHP = anEnemy.myHP;
                myEnemyIsAlive = true;

                Console.WriteLine("The heroes " + StringHelper.JoinWithAnd(myHeroes) + " descend into the dungeon.");
                Console.WriteLine(anEnemy.myName + " with " + myEnemyHP + " HP appears!");
                while (myEnemyHP > 0 && myHeroes.Count > 0)
                {
                    for (int i = 0; i < myHeroes.Count; i++)
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
                        Console.WriteLine(myHeroes[i] + " hits the " + anEnemy.myName + " for " + myHeroDamage + " damage. " + anEnemy.myName + " has " + myEnemyHP + " HP left.");
                    }
                    if (myEnemyIsAlive)
                    {
                        myConstitutionRoll = DiceHelper.Roll(myHeroConstitutionDice) + myHeroConstitution;
                        myHeroIsSaved = myConstitutionRoll >= anEnemy.myConstitutionSaveNeeded;
                        int attackedHeroIndex = DiceHelper.GetRandom(myHeroes.Count) - 1;
                        Console.WriteLine("The " + anEnemy.myName + " attacks " + myHeroes[attackedHeroIndex] + "!");
                        Console.Write(myHeroes[attackedHeroIndex] + " rolls a " + myConstitutionRoll);

                        if (myHeroIsSaved)
                        {
                            Console.WriteLine(" and is saved from the attack.");
                        }
                        else
                        {
                            Console.WriteLine(" and fails to be saved...");
                            myHeroes.RemoveAt(attackedHeroIndex);
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
                    myHeroesAreAlive = false;
                }
            }
        }

        void SetHeroList(string aFirstName, string aSeconName, string aThridName, string aFourthName)
        {
            myHeroes.Add(aFirstName);
            myHeroes.Add(aSeconName);
            myHeroes.Add(aThridName);
            myHeroes.Add(aFourthName);
        }

        

        struct Enemy
        {
            public string myName;
            public int myHP;
            public int myConstitutionSaveNeeded;

            public Enemy(string aName, string aDiceNotation, int aConstitutionSaveNeeded)
            {
                myName = aName;
                myHP = DiceHelper.Roll(aDiceNotation);
                myConstitutionSaveNeeded = aConstitutionSaveNeeded;
            }
        }
    }
}
