using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        static Random myRandom = new Random();
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
                    stillAlive = "es " + JoinWithAnd(myHeroes);
                }
                Console.WriteLine("After three grueling battles, the hero" + stillAlive + " return from the dungeons to live another day.");
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void SimulateBattle(Enemy anEnemy)
        {
            if (myHeroesAreAlive)
            {
                myEnemyHP = anEnemy.myHP;
                myEnemyIsAlive = true;

                Console.WriteLine("The heroes " + JoinWithAnd(myHeroes) + " descend into the dungeon.");
                Console.WriteLine(anEnemy.myName + " with " + myEnemyHP + " HP appears!");
                while (myEnemyHP > 0 && myHeroes.Count > 0)
                {
                    for (int i = 0; i < myHeroes.Count; i++)
                    {
                        if (myEnemyHP == 0)
                        {
                            break;
                        }
                        myHeroDamage = RollDices(myHeroDamageDice);
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
                        myConstitutionRoll = RollDices(myHeroConstitutionDice) + myHeroConstitution;
                        myHeroIsSaved = myConstitutionRoll >= anEnemy.myConstitutionSaveNeeded;
                        int attackedHeroIndex = GetRandom(myHeroes.Count) - 1;
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

        string JoinWithAnd(List<string> aListOfStrings, bool aUseSerialComma = false)
        {
            int sizeOfList = aListOfStrings.Count;
            string lastItem = aListOfStrings[sizeOfList - 1];
            aListOfStrings.RemoveAt(sizeOfList - 1);
            sizeOfList--;

            string joinedItems = string.Join(", ", aListOfStrings);

            if (aUseSerialComma)
            {
                joinedItems += ", and " + lastItem;
            }
            else
            {
                joinedItems += " and " + lastItem;
            }

            aListOfStrings.Add(lastItem);

            return joinedItems;
        }

        static int GetRandom(int max)
        {
            int result;
            result = myRandom.Next(1, max + 1);
            return result;
        }

        static int RollDices(string aDiceType)
        {
            string pattern = @"(\d{0,3})d([468]|10|20)(\s|([-+])(\d{1,2}))?";
            int numberOfRolls = 1;
            int facesOnDice = 0;
            int followNumber = 0;
            int result = 0;

            MatchCollection matches = Regex.Matches(aDiceType, pattern);
            foreach (Match match in matches)
            {
                GroupCollection data = match.Groups;

                _ = int.TryParse(data[1].Value, out numberOfRolls);
                _ = int.TryParse(data[2].Value, out facesOnDice);
                _ = int.TryParse(data[5].Value, out followNumber);
                if (data[4].Value == "-")
                {
                    followNumber -= (2 * followNumber);
                }
            }
            if (numberOfRolls == 0)
            {
                numberOfRolls = 1;
            }
            for (int i = 1; i <= numberOfRolls; i++)
            {
                result += GetRandom(facesOnDice);
            }

            result += followNumber;


            return result;
        }

        struct Enemy
        {
            public string myName;
            public int myHP;
            public int myConstitutionSaveNeeded;

            public Enemy(string aName, string aDiceNotation, int aConstitutionSaveNeeded)
            {
                myName = aName;
                myHP = RollDices(aDiceNotation);
                myConstitutionSaveNeeded = aConstitutionSaveNeeded;
            }
        }
    }
}
