using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEditor;

namespace MonsterQuest
{
    public static class MonsterTypeImporter
    {
        public static IEnumerable<string> MonsterIndexNames 
        {
            get
            {
                if (myMonsterIndexNames == null)
                {
                    LoadMonsterNames();
                }
                return myMonsterIndexNames;
            }
        }

        private static List<string> myMonsterIndexNames = null;

        private static Dictionary<string, string> myMonsterNamesIndexDictionary = null;



        public static void ImportData(string aName, MonsterType aMonsterType)
        {
            HttpClient httpClient = new();
            JObject monsterData = JObject.Parse(httpClient.GetStringAsync("https://www.dnd5eapi.co/api/monsters/" + myMonsterNamesIndexDictionary[aName]).Result);
            aMonsterType.myDisplayName = (string)monsterData["name"];
            aMonsterType.mySizeCategory = Helpers.GetSizeCategory((string)monsterData["size"]);
            aMonsterType.myAlignment = (string)monsterData["alignment"];
            aMonsterType.myHitPointsRoll = (string)monsterData["hit_points_roll"];
            //Add weaponType
            //Add armorType

            aMonsterType.myArmorClass = (int)monsterData["armor_class"][0]["value"];
            aMonsterType.myChallengeRating = (int)monsterData["challenge_rating"];
            aMonsterType.myXPToGive = (int)monsterData["xp"];
            //Add Sprite

            aMonsterType.myAbilityScores.Strength = new ((int)monsterData["strength"]);
            aMonsterType.myAbilityScores.Dexterity = new((int)monsterData["dexterity"]);
            aMonsterType.myAbilityScores.Constitution = new((int)monsterData["constitution"]);
            aMonsterType.myAbilityScores.Intelligence = new((int)monsterData["intelligence"]);
            aMonsterType.myAbilityScores.Wisdom = new((int)monsterData["wisdom"]);
            aMonsterType.myAbilityScores.Charisma = new((int)monsterData["charisma"]);

            EditorUtility.SetDirty(aMonsterType);
        }

        static void LoadMonsterNames()
        {
            HttpClient httpClient = new();
            JObject monsterNames = JObject.Parse(httpClient.GetStringAsync("https://www.dnd5eapi.co/api/monsters").Result);
            myMonsterIndexNames = new List<string>();
            myMonsterNamesIndexDictionary = new Dictionary<string, string>();

            int count = (int)monsterNames["count"];
            for (int i = 0; i < count; i++) 
            {
                myMonsterIndexNames.Add((string)monsterNames["results"][i]["name"]);
                myMonsterNamesIndexDictionary.Add((string)monsterNames["results"][i]["name"], (string)monsterNames["results"][i]["index"]);
            }

        }
    }
}
