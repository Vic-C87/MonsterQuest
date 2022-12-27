using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        List<string> myHeroes = new List<string>();
        
        bool myHeroesAreAlive = true;

        Enemy myOrc;
        Enemy myAzer;
        Enemy myTroll;

        CombatManager myCombatManager;

        void Awake()
        {
            myCombatManager = GetComponentInChildren<CombatManager>();
            myOrc = new Enemy("Orc", "2d8+6", 10);
            myAzer = new Enemy("Azer", "6d8+12", 18);
            myTroll = new Enemy("Troll", "8d10+40", 16);
        }

        void Start()
        {
            myCombatManager.Simulate(myHeroes, myOrc, ref myHeroesAreAlive);
            myCombatManager.Simulate(myHeroes, myAzer, ref myHeroesAreAlive);
            myCombatManager.Simulate(myHeroes, myTroll, ref myHeroesAreAlive);
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
    }

    public struct Enemy
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
