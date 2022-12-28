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

        [SerializeField]
        Sprite[] myCharacterBodySprites;
        [SerializeField]
        Sprite[] myMonsterBodySprites;

        Monster myOrc;
        Monster myAzer;
        Monster myTroll;

        CombatPresenter myCombatPresenter;

        void Awake()
        {
            myCombatManager = GetComponentInChildren<CombatManager>();
            myCombatPresenter = GetComponentInChildren<CombatPresenter>();
            myOrc = new Monster("Orc", myMonsterBodySprites[0],DiceHelper.Roll("2d8+6"), SizeCategory.Medium, 10);
            myAzer = new Monster("Azer", myMonsterBodySprites[1], DiceHelper.Roll("6d8+12"), SizeCategory.Medium, 18);
            myTroll = new Monster("Troll", myMonsterBodySprites[2], DiceHelper.Roll("8d10+40"), SizeCategory.Large, 16);
        }

        IEnumerator Start()
        {
            NewGame();
            yield return Simulate();
        }
        
        void NewGame()
        {
            Party party = new Party(new Character[] 
                {   new Character("Jazlyn", myCharacterBodySprites[0], 10, SizeCategory.Medium), 
                    new Character("Theron", myCharacterBodySprites[1], 10, SizeCategory.Medium), 
                    new Character("Dayana", myCharacterBodySprites[2], 10, SizeCategory.Medium), 
                    new Character("Rolando", myCharacterBodySprites[3], 10, SizeCategory.Medium) 
                });
            myGameState = new GameState(party);
        }

        IEnumerator Simulate()
        {
            myCombatPresenter.InitializeParty(myGameState);

            myGameState.EnterCombatWithMonster(myOrc);
            myCombatPresenter.InitializeMonster(myGameState);
            yield return myCombatManager.Simulate(myGameState);
            
            myGameState.EnterCombatWithMonster(myAzer);
            myCombatPresenter.InitializeMonster(myGameState);
            yield return myCombatManager.Simulate(myGameState);

            myGameState.EnterCombatWithMonster(myTroll);
            myCombatPresenter.InitializeMonster(myGameState);
            yield return myCombatManager.Simulate(myGameState);

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
