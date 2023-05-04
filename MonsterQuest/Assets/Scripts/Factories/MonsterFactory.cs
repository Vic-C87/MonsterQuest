using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

namespace MonsterQuest
{
    public class MonsterFactory : Factory
    {
        public MonsterFactory()
            : base()
        {
            myApiAddress = "https://www.dnd5eapi.co/api/monsters";
            LoadDictionary();
        }

        public MonsterType GetMonsterType(string aName, out JObject someApiData)
        {

            HttpClient httpClient = new();
            JObject monsterData = JObject.Parse(httpClient.GetStringAsync("https://www.dnd5eapi.co/api/monsters/" + myNameIndexDictionary[aName]).Result);
            MonsterType monster = ScriptableObject.CreateInstance<MonsterType>();

            monster.myDisplayName = (string)monsterData["name"];
            Debug.Log("Monster name: " + monster.myDisplayName);
            monster.mySizeCategory = SizeHelper.GetSizeCategory((string)monsterData["size"]);
            monster.myAlignment = (string)monsterData["alignment"];
            monster.myHitPointsRoll = (string)monsterData["hit_points_roll"];
            Debug.Log(monster.myHitPointsRoll);
            monster.myArmorClass = (int)monsterData["armor_class"][0]["value"];
            monster.myChallengeRating = (int)monsterData["challenge_rating"];
            Debug.Log("Challenge Rating:" + monster.myChallengeRating);
            monster.myXPToGive = (int)monsterData["xp"];
            Debug.Log("XP: " + monster.myXPToGive);
            //Add Sprite            
            monster.myAbilityScores.Strength = new((int)monsterData["strength"]);
            monster.myAbilityScores.Dexterity = new((int)monsterData["dexterity"]);
            monster.myAbilityScores.Constitution = new((int)monsterData["constitution"]);
            monster.myAbilityScores.Intelligence = new((int)monsterData["intelligence"]);
            monster.myAbilityScores.Wisdom = new((int)monsterData["wisdom"]);
            monster.myAbilityScores.Charisma = new((int)monsterData["charisma"]);

            monster.name = monster.myDisplayName;
            someApiData = monsterData;
            return monster;
        }
    }
}
