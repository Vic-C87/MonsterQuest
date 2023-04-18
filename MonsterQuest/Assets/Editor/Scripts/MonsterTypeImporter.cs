using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

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
            //Add Sprite

            aMonsterType.myAbilityScores.Strength = new ((int)monsterData["strength"]);
            aMonsterType.myAbilityScores.Dexterity = new((int)monsterData["dexterity"]);
            aMonsterType.myAbilityScores.Constitution = new((int)monsterData["constitution"]);
            aMonsterType.myAbilityScores.Intelligence = new((int)monsterData["intelligence"]);
            aMonsterType.myAbilityScores.Wisdom = new((int)monsterData["wisdom"]);
            aMonsterType.myAbilityScores.Charisma = new((int)monsterData["charisma"]);

        }

        static void LoadMonsterNames()
        {
            HttpClient httpClient = new();
            string responseJson = httpClient.GetStringAsync("https://www.dnd5eapi.co/api/monsters").Result;
            myMonsterIndexNames = new List<string>();
            myMonsterNamesIndexDictionary = new Dictionary<string, string>();


            string[] patternsIndex = { "\"index\":\"", "\",\"name\":\"" };
            string[] patternsName = { "\",\"name\":\"", "\",\"url\":" };

            string[] splitIndex = responseJson.Split(patternsIndex, System.StringSplitOptions.RemoveEmptyEntries);
            string[] splitNames = responseJson.Split(patternsName, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < splitIndex.Length; i++)
            {
                if (i % 2 != 0)
                {
                    myMonsterNamesIndexDictionary.Add(splitNames[i], splitIndex[i]);
                    myMonsterIndexNames.Add(splitNames[i]);
                }
            }

        }
    }
}
