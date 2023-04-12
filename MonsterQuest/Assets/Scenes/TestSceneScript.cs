using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MonsterQuest
{
    public class TestSceneScript : MonoBehaviour
    {
        [SerializeField]
        public List<Tester> myMonsterIndexNames = new List<Tester>();

        private void Awake()
        {
            LoadMonsterNames();
        }

        void LoadMonsterNames()
        {
            HttpClient httpClient = new();
            string responseJson = httpClient.GetStringAsync(@"https://www.dnd5eapi.co/api/monsters").Result;

            string[] patternsName = { "\",\"name\":\"", "\",\"url\":" };
            string[] patternsIndex = { "\"index\":\"", "\",\"name\":\"" };

            string[] splitNames = responseJson.Split(patternsName, System.StringSplitOptions.RemoveEmptyEntries);
            string[] splitIndex = responseJson.Split(patternsIndex, System.StringSplitOptions.RemoveEmptyEntries);

            
            for (int i = 0; i < splitIndex.Length; i++)
            {
                if (i % 2 != 0)
                {
                    myMonsterIndexNames.Add(new Tester(splitIndex[i], splitNames[i], "nope"));
                }
            }
            
        }

        [Serializable]
        public class Tester
        {
            public string index;
            public string name;
            public string url;

            public Tester(string index, string name, string url)
            {
                this.index = index;
                this.name = name;
                this.url = url;
            }
        }
    }
}
