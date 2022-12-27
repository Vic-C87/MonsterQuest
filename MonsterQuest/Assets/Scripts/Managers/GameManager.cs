using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        List<string> myHeroes = new List<string>();

        CombatManager myCombatManager;

        GameState myGameState;

        Monster myOrc;
        Monster myAzer;
        Monster myTroll;

        void Awake()
        {
            myCombatManager = GetComponentInChildren<CombatManager>();
            myOrc = new Monster("Orc", DiceHelper.Roll("2d8+6"), 10);
            myAzer = new Monster("Azer", DiceHelper.Roll("6d8+12"), 18);
            myTroll = new Monster("Troll", DiceHelper.Roll("8d10+40"), 16);
        }

        void Start()
        {
            NewGame();
            Simulate();
        }
        
        void NewGame()
        {
            Party party = new Party(new Character[] { new Character("Jazlyn"), new Character("Theron"), new Character("Dayana"), new Character("Rolando") });
            myGameState = new GameState(party);
        }

        void Simulate()
        {
            myGameState.EnterCombatWithMonster(myOrc);
            myCombatManager.Simulate(myGameState);
            myGameState.EnterCombatWithMonster(myAzer);
            myCombatManager.Simulate(myGameState);
            myGameState.EnterCombatWithMonster(myTroll);
            myCombatManager.Simulate(myGameState);
            myHeroes = myGameState.myParty.GetNames();
            if (myHeroes.Count > 0)
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
}
