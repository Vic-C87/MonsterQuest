using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using UnityEngine;


namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        List<string> myHeroes = new List<string>();

        CombatManager myCombatManager;
        CombatPresenter myCombatPresenter;

        GameState myGameState;

        [SerializeField]
        Sprite[] myCharacterBodySprites;
        [SerializeField]
        MonsterType[] myMonsterTypes;

        void Awake()
        {
            myCombatManager = GetComponentInChildren<CombatManager>();
            myCombatPresenter = GetComponentInChildren<CombatPresenter>();
            
        }

        IEnumerator Start()
        {
            yield return Database.Initialize();
            NewGame();
            yield return Simulate();
        }

        void NewGame()
        {
            ArmorType armor = Database.GetItemType<ArmorType>("Studded leather");
            List<WeaponType> weapons = new List<WeaponType>();
            ItemType[] items = Database.itemTypes.ToArray<ItemType>();
            foreach (ItemType item in items)
            {
                if (item is WeaponType weapon && item.myWeight > 1)
                {
                    weapons.Add(weapon);
                }
            }
            Party party = new Party(new Character[] 
                {   new Character("Jazlyn", myCharacterBodySprites[0], 10, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor), 
                    new Character("Theron", myCharacterBodySprites[1], 10, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor), 
                    new Character("Dayana", myCharacterBodySprites[2], 10, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor), 
                    new Character("Rolando", myCharacterBodySprites[3], 10, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor) 
                });
            myGameState = new GameState(party);
            myMonsterTypes = Database.monsterTypes.ToArray<MonsterType>();
        }

        IEnumerator Simulate()
        {
            yield return myCombatPresenter.InitializeParty(myGameState);

            for (int i = 0; i < myMonsterTypes.Length; i++)
            {
                if (myGameState.myParty.OneAlive())
                {
                    myGameState.EnterCombatWithMonster(new Monster(myMonsterTypes[i]));
                    yield return myCombatPresenter.InitializeMonster(myGameState);
                    yield return myCombatManager.Simulate(myGameState);
                }
            }

            myHeroes = CheckHeroesAlive();
            if (myHeroes.Count > 0 && myGameState.myParty.OneAlive())
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
                Console.WriteLine("After " + myMonsterTypes.Length + " grueling battles, the hero" + stillAlive + " return from the dungeons to live another day.");
            }
        }

        List<string> CheckHeroesAlive()
        {
            List<string> heroesAlive = new List<string>();
            foreach (Character character in myGameState.myParty.myCharacters)
            {
                if (character.myLifeStatus != ELifeStatus.Dead)
                {
                    heroesAlive.Add(character.myDisplayName);
                }
            }

            return heroesAlive;
        }
    }
}
