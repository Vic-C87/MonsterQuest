using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UnityEngine;

namespace MonsterQuest
{
    public class WeaponFactory : ItemFactory
    {

        public WeaponFactory()
            : base()
        {
            myApiAddress = "https://www.dnd5eapi.co/api/equipment-categories/weapon";
            LoadDictionary();
        }

        public void GetMonsterWeapons(MonsterType aMonster, JObject someApiData)
        {
            int count = (int)someApiData["actions"].Children().Count();
            for (int i = 0; i < count; i++)
            {
                string action = (string)someApiData["actions"][i]["name"];

                if (myNameIndexDictionary.ContainsKey(action))
                {
                    Debug.Log("Entered dictionary");
                    aMonster.myWeaponTypes.Add(GetWeapon(action));
                }
                else
                {
                    string damageRoll;
                    try
                    {
                        damageRoll = (string)someApiData["actions"][i]["damage"][0]["damage_dice"];
                    }
                    catch (System.Exception)
                    {

                        continue;
                    }

                    WeaponType weapon = ScriptableObject.CreateInstance<WeaponType>();
                    weapon.myDisplayName = action;
                    weapon.myWeight = 0;
                    weapon.myDamageRoll = damageRoll;
                    string meleeOrRanged = (string)someApiData["actions"][i]["desc"];
                    weapon.myIsRanged = !meleeOrRanged.ToLower().Contains("melee");
                    weapon.myIsFinesse = false;
                    weapon.myWeaponCategory.Add(action);
                    weapon.name = weapon.myDisplayName;
                    aMonster.myWeaponTypes.Add(weapon);
                    Debug.Log(weapon.myDisplayName);
                    Debug.Log(weapon.myDamageRoll);
                }
            }
        }

        public WeaponType GetWeapon(string aName)
        {
            HttpClient httpClient = new();
            JObject weaponData = JObject.Parse(httpClient.GetStringAsync("https://www.dnd5eapi.co/api/equipment/" + myNameIndexDictionary[aName]).Result);
            WeaponType weapon = ScriptableObject.CreateInstance<WeaponType>();

            weapon.myDisplayName = (string)weaponData["name"];
            Debug.Log(weapon.myDisplayName);
            weapon.myWeight = (int)weaponData["weight"];
            weapon.myDamageRoll = (string)weaponData["damage"]["damage_dice"];
            Debug.Log(weapon.myDamageRoll);
            if ((string)weaponData["weapon_range"] == "Ranged")
            {
                weapon.myIsRanged = true;
            }
            else
            {
                weapon.myIsRanged = false;
            }

            int count = (int)weaponData["properties"].Children().Count();
            for (int i = 0; i < count; i++)
            {
                if ((string)weaponData["properties"][i]["name"] == "Finesse")
                {
                    weapon.myIsFinesse = true;
                    break;
                }
                else
                {
                    weapon.myIsFinesse = false;
                }
            }

            weapon.myWeaponCategory.Add((string)weaponData["weapon_category"] + " Weapons");
            weapon.myWeaponCategory.Add(weapon.myDisplayName + "s");
            weapon.name = weapon.myDisplayName;
            return weapon;
        }
    }
}
