using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public abstract class Factory
    {
        protected Dictionary<string, string> myNameIndexDictionary;

        [SerializeField]
        protected string myApiAddress;

        protected string myBaseApiAddress = "https://www.dnd5eapi.co";

        public Factory()
        {
            Debug.Log(this + " Created");
            myNameIndexDictionary = new Dictionary<string, string>();
        }

        protected virtual void LoadDictionary()
        {
            HttpClient httpClient = new();
            JObject apiResults = JObject.Parse(httpClient.GetStringAsync(myApiAddress).Result);

            int count = (int)apiResults["count"];
            for (int i = 0; i < count; i++)
            {
                myNameIndexDictionary.Add((string)apiResults["results"][i]["name"], (string)apiResults["results"][i]["index"]);
            }
        }

        public List<string> NamesList
        {
            get
            {
                return myNameIndexDictionary.Keys.ToList();
            }
        }

        public bool Contains(string aName)
        {
            return myNameIndexDictionary.ContainsKey(aName);
        }

        protected ArmorCategory GetArmorCategory(string aName)
        {
            switch (aName)
            {
                case "Heavy":
                    return ArmorCategory.Heavy;
                case "Light":
                    return ArmorCategory.Light;
                case "Medium":
                    return ArmorCategory.Medium;
                default:
                    return ArmorCategory.Other;
            }
        }

        protected EAbility GetEAbility(string aName)
        {
            switch (aName)
            {
                case "STR":
                    return EAbility.Strength;

                case "DEX":
                    return EAbility.Dexterity;
                case "CON":
                    return EAbility.Constitution;
                case "INT":
                    return EAbility.Intelligence;
                case "WIS":
                    return EAbility.Wisdom;
                case "CHA":
                    return EAbility.Charisma;
                default:
                    return EAbility.None;

            }
        }
    }
}
