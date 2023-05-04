using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
            yield return Database.Initialize();
            yield return NewGame();
            yield return Simulate();
        }

        IEnumerator NewGame()
        {
            myGameState = SaveGameHelper.Load();

            if (myGameState == null)
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
                {   new Character("Jazlyn",     myCharacterBodySprites[0].Asset as Sprite, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor, Database.GetClassType("Fighter")), 
                    new Character("Theron",     myCharacterBodySprites[1].Asset as Sprite, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor, Database.GetClassType("Fighter")), 
                    new Character("Dayana",     myCharacterBodySprites[2].Asset as Sprite, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor, Database.GetClassType("Fighter")), 
                    new Character("Rolando",    myCharacterBodySprites[3].Asset as Sprite, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor, Database.GetClassType("Fighter")),
                    new Character("WizBoy",     myCharacterBodySprites[4].Asset as Sprite, SizeCategory.Medium, weapons[DiceHelper.GetRandom(weapons.Count) -1], armor, Database.GetClassType("Fighter"))
                });

                yield return ValidateMonsters();
                List<Monster> monsters = new List<Monster>(myMonsterTypes.Length);                
                for (int i = 0; i < myMonsterTypes.Length; i++) 
                {
                    monsters.Add(new Monster(myMonsterTypes[i].Asset as MonsterType));
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

            if (myGameState.myParty.OneAlive())
            {
                Console.WriteLine(WinMessage());
                SaveGameHelper.Delete();
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

        string WinMessage()
        {
            myHeroes = CheckHeroesAlive();

            string stillAlive;
            if (myHeroes.Count == 1)
            {
                stillAlive = " " + myHeroes[0];
            }
            else
            {
                stillAlive = "es " + StringHelper.JoinWithAnd(myHeroes);
            }
            return "After " + myMonsterTypes.Length + " grueling battles, the hero" + stillAlive + " return from the dungeons to live another day.";
        }
    }
}
