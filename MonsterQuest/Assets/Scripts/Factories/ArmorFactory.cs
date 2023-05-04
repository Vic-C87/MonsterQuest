using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

namespace MonsterQuest
{
    public class ArmorFactory : ItemFactory
    {
        public ArmorFactory()
            : base()
        {
            myApiAddress = "https://www.dnd5eapi.co/api/equipment-categories/armor";
            LoadDictionary();
        }

        public void GetMonsterArmor(MonsterType aMonster, JObject someApiData)
        {
            string name = "";       //!!!
            try
            {
                name = (string)someApiData["armor_class"][0]["armor"][0]["name"];
            }
            catch (System.Exception) { }

            if (name != "")
            {
                aMonster.myArmorType = GetArmor(name);
            }
            else
            {
                ArmorType armor = ScriptableObject.CreateInstance<ArmorType>();

                armor.myDisplayName = "Natural";
                armor.myWeight = 0;
                armor.myArmorCategory = ArmorCategory.Other;
                armor.myArmorClass = (int)someApiData["armor_class"][0]["value"];
                armor.name = armor.myDisplayName;
                aMonster.myArmorType = armor;
            }
            Debug.Log("Armor type: " + aMonster.myArmorType.myDisplayName);
            Debug.Log("Armor class: " + aMonster.myArmorType.myArmorClass);
        }

        public ArmorType GetArmor(string aName)
        {
            HttpClient httpClient = new();
            JObject armorData = JObject.Parse(httpClient.GetStringAsync("https://www.dnd5eapi.co/api/equipment/" + myNameIndexDictionary[aName]).Result);
            ArmorType armor = ScriptableObject.CreateInstance<ArmorType>();

            armor.myDisplayName = (string)armorData["name"];
            armor.myWeight = (int)armorData["weight"];

            armor.myArmorCategory = GetArmorCategory((string)armorData["armor_category"]);
            armor.myArmorClass = (int)armorData["armor_class"]["base"];
            armor.name = armor.myDisplayName;
            return armor;
        }


    }
}
