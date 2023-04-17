using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace MonsterQuest
{
    public static class SaveGameHelper
    {
        static string mySaveFilePath = Path.Combine(UnityEngine.Application.persistentDataPath,"Victor.json");

        static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new DefaultContractResolver()
            {
                IgnoreSerializableAttribute = false
            },
            Converters = new List<JsonConverter>
            {
                new UnityObjectConverter()
            }
        };

        public static bool SaveFileExists 
        {
            get
            {   
                return File.Exists(mySaveFilePath);
            }
        }

        public static bool Save(GameState aGameState)
        {
            
            if (SaveFileExists) Delete();

            string save = JsonConvert.SerializeObject(aGameState, serializerSettings);

            try
            {
                File.WriteAllText(mySaveFilePath, save);
            }
            catch (IOException ioE)
            {
                Debug.LogWarning(ioE.Message);
                return false;
            }
            return true;
        }

        public static GameState Load()
        {
            Debug.Log(mySaveFilePath);
            if (!SaveFileExists) { Debug.LogWarning("Cannot locate save file"); return null; }

            string load;

            try 
            {
                load = File.ReadAllText(mySaveFilePath);
            }
            catch(IOException ioE) 
            {
                Debug.LogWarning(ioE.Message); 
                return null;
            }

            GameState gameState = JsonConvert.DeserializeObject<GameState>(load, serializerSettings);

            return gameState;
        }

        public static bool Delete()
        {
            if (SaveFileExists) 
            {
                try
                {
                    File.Delete(mySaveFilePath);
                }
                catch (IOException ioE) 
                {
                    Debug.LogWarning(ioE.Message + "\nFile might be in use by the system, cannot delete.");
                    return false;

                }
                return true;
            }
            Debug.LogWarning("No file exist to delete.");
            return false;
        }

        internal class UnityObjectConverter : JsonConverter<UnityEngine.Object>
        {
            public override UnityEngine.Object ReadJson(JsonReader reader, Type objectType, UnityEngine.Object existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Database.GetAssetForPrimaryKey<UnityEngine.Object>((string)reader.Value);
            }

            public override void WriteJson(JsonWriter writer, UnityEngine.Object value, JsonSerializer serializer)
            {
                writer.WriteValue(Database.GetPrimaryKeyForAsset(value));
            }
        }
    }
}
