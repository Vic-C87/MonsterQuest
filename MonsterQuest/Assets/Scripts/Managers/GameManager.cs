using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        List<string> myHeroes = new List<string>();

        CombatManager myCombatManager;
        CombatPresenter myCombatPresenter;

        GameState myGameState;

        [SerializeField]
        AssetReferenceSprite[] myCharacterBodySprites;
        [SerializeField]
        AssetReferenceT<MonsterType>[] myMonsterTypes;

        void Awake()
        {
            myCombatManager = GetComponentInChildren<CombatManager>();
            myCombatPresenter = GetComponentInChildren<CombatPresenter>();
            
        }

        IEnumerator Start()
        {
            Debug.Log(Application.streamingAssetsPath);
            yield return Database.Initialize();
            yield return NewGame();
            yield return Simulate();
        }

        IEnumerator NewGame()
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

            yield return ValidateHeroes();

            Party party = new Party(new Character[] 
                {   new Character("Jazlyn", myCharacterBodySprites[0].Asset as Sprite, 10, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor), 
                    new Character("Theron", myCharacterBodySprites[1].Asset as Sprite, 10, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor), 
                    new Character("Dayana", myCharacterBodySprites[2].Asset as Sprite, 10, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor), 
                    new Character("Rolando", myCharacterBodySprites[3].Asset as Sprite, 10, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor) 
                });

            myGameState = SaveGameHelper.Load();

            yield return ValidateMonsters();

            if (myGameState == null)
            {
                List<Monster> monsters = new List<Monster>();
                foreach (AssetReferenceT<MonsterType> type in myMonsterTypes)
                {             
                    monsters.Add(new Monster(type.Asset as MonsterType));
                }
                myGameState = new GameState(party, monsters);
            }
        }

        IEnumerator ValidateMonsters()
        {
            foreach (AssetReferenceT<MonsterType> type in myMonsterTypes)
            {
                AsyncOperationHandle<MonsterType> asyncOperationHandle = type.LoadAssetAsync();

                if (!asyncOperationHandle.IsDone)
                {
                    yield return asyncOperationHandle;
                }
                
            }
        }

        IEnumerator ValidateHeroes()
        {
            foreach(AssetReferenceSprite sprite in myCharacterBodySprites) 
            {
                AsyncOperationHandle<Sprite> asyncOperationHandle = sprite.LoadAssetAsync(); 
                
                if (!asyncOperationHandle.IsDone) 
                {
                    yield return asyncOperationHandle;
                }
            }
        }

        IEnumerator Simulate()
        {
            yield return myCombatPresenter.InitializeParty(myGameState);

            while (myGameState.EnterCombatWithMonster())
            {
                yield return myCombatPresenter.InitializeMonster(myGameState);
                yield return myCombatManager.Simulate(myGameState);
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
            SaveGameHelper.Delete();
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
