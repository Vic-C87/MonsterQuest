using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UnityEngine;

namespace MonsterQuest
{
    public class RaceFactory : Factory
    {
        public RaceFactory()
            : base()
        {
            myApiAddress = "https://www.dnd5eapi.co/api/races";
            LoadDictionary();
        }

        public RaceType GetRace(string aName)
        {
            HttpClient httpClient = new();
            JObject raceData = JObject.Parse(httpClient.GetStringAsync("https://www.dnd5eapi.co/api/races/" + myNameIndexDictionary[aName]).Result);
            Dictionary<EAbility, int> abilityScoreDictionary = new Dictionary<EAbility, int>();

            int count = (int)raceData["ability_bonuses"].Children().Count();
            for (int i = 0; i < count; i++)
            {
                abilityScoreDictionary.Add(GetEAbility((string)raceData["ability_bonuses"][i]["ability_score"]["name"]), (int)raceData["ability_bonuses"][i]["bonus"]);
            }
            RaceType raceType = ScriptableObject.CreateInstance<RaceType>();
            raceType.myDisplayName = (string)raceData["name"];
            raceType.mySpeed = (int)raceData["speed"];
            raceType.myAbilityScoreBonuses = abilityScoreDictionary;
            raceType.myDescription = (string)raceData["age"] + "\n" + (string)raceData["alignment"] + "\n" + (string)raceData["size_description"];
            raceType.mySize = SizeHelper.GetSizeCategory((string)raceData["size"]);
            raceType.name = raceType.myDisplayName;
            Debug.Log(raceType.name);
            foreach (var kvp in raceType.myAbilityScoreBonuses)
            {
                Debug.Log(kvp.Key + " : " + kvp.Value);
            }
            return raceType;
        }
    }
}
