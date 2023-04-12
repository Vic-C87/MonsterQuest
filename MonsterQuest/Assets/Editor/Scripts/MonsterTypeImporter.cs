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

        private static List<MonsterIndexEntry> myMonsterIndexEntries = null;

        static void LoadMonsterNames()
        {
            HttpClient httpClient = new();
            string responseJson = httpClient.GetStringAsync("https://www.dnd5eapi.co/api/monsters").Result;
            myMonsterIndexNames = new List<string>();
            myMonsterIndexEntries= new List<MonsterIndexEntry>();


            string[] patternsIndex = { "\"index\":\"", "\",\"name\":\"" };
            string[] patternsName = { "\",\"name\":\"", "\",\"url\":" };

            string[] splitIndex = responseJson.Split(patternsIndex, System.StringSplitOptions.RemoveEmptyEntries);
            string[] splitNames = responseJson.Split(patternsName, System.StringSplitOptions.RemoveEmptyEntries);


            for (int i = 0; i < splitIndex.Length; i++)
            {
                if (i % 2 != 0)
                {
                    myMonsterIndexEntries.Add(new MonsterIndexEntry(splitIndex[i], splitNames[i]));
                    myMonsterIndexNames.Add(splitNames[i]);
                }
            }

        }
    }
}
