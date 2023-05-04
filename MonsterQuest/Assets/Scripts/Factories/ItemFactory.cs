using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UnityEngine;

namespace MonsterQuest
{
    public abstract class ItemFactory : Factory
    {
        protected override void LoadDictionary()
        {
            HttpClient httpClient = new();
            JObject apiResults = JObject.Parse(httpClient.GetStringAsync(myApiAddress).Result);

            int count = apiResults["equipment"].Children().Count();
            for (int i = 0; i < count; i++)
            {
                myNameIndexDictionary.Add((string)apiResults["equipment"][i]["name"], (string)apiResults["equipment"][i]["index"]);
            }          
        }
    }
}
