using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using UnityEngine;

namespace MonsterQuest
{
    public class ClassFactory : Factory
    {
        List<string> myStartingEquipmentList;
        List<Options> myStartingEquipmentOptionsList;

        public ClassFactory()
            :base()
        {
            myApiAddress = "https://www.dnd5eapi.co/api/classes";
            LoadDictionary();
        }

        public ClassType GetClass(string aName)
        {
            HttpClient httpClient = new();
            JObject classData = JObject.Parse(httpClient.GetStringAsync("https://www.dnd5eapi.co/api/classes/" + myNameIndexDictionary[aName]).Result);
            ClassType classType = ScriptableObject.CreateInstance<ClassType>();

            classType.myDisplayName = (string)classData["name"];
            classType.myHitDie = (string)classData["hit_die"];

            int count = (int)classData["proficiencies"].Children().Count();

            for (int i = 0; i < count; i++)
            {
                classType.myProficiencies.Add((string)classData["proficiencies"][i]["name"]);
            }
            classType.name = classType.myDisplayName;
            GenerateEquipmentLists(classData);

            return classType;
        }

        void GenerateEquipmentLists(JObject someApiData)
        {
            myStartingEquipmentList = new List<string>();
            int count = someApiData["starting_equipment"].Children().Count();
            int innerCount;
            for (int i = 0; i < count; i++) 
            {
                innerCount = (int)someApiData["starting_equipment"][i]["quantity"];
                for (int j = 0; j < innerCount; j++) 
                {
                    myStartingEquipmentList.Add((string)someApiData["starting_equipment"][i]["equipment"]["name"]);
                }
            }
            JToken startingEquipment = someApiData["starting_equipment_options"];
            count = startingEquipment.Children().Count();
            SortOptions(startingEquipment);
        }
        
        void SortOptions(JToken someStartingOptionsData) 
        {
            List<Options> optionsList = new List<Options>();
            int count = someStartingOptionsData.Children().Count();

            for (int i = 0; i < count; i++)
            {
                string description = (string)someStartingOptionsData[i]["desc"];
                int choicesCount = (int)someStartingOptionsData[i]["choose"];
                Options options = new Options(description, choicesCount);

                JToken innerStaringOptions = someStartingOptionsData[i]["from"]["options"];
                if (innerStaringOptions != null)
                {
                    int innerCount = innerStaringOptions.Children().Count();

                    for (int j = 0; j < innerCount; j++) 
                    {
                        switch ((string)innerStaringOptions[j]["option_type"])
                        {
                            case "counted_reference":
                                Single single = GetSingle(innerStaringOptions[j]);
                                options.AddOption(single);
                                break;
                            case "multiple":
                                JToken multipleData = innerStaringOptions[j]["items"];
                                Multiple multiple = GetMultiple(multipleData);
                                options.AddOption(multiple);
                                break;
                            case "choice":
                                Choice choices = GetChoice(innerStaringOptions[j]);
                                options.AddOption(choices);
                                break;
                            default:
                                break;
                        }
                    }
                }
                optionsList.Add(options);
            }

            myStartingEquipmentOptionsList = optionsList;
        }

        Single GetSingle(JToken someCountedReferenceData)
        {
            string name = (string)someCountedReferenceData["of"]["name"];
            int quantity = (int)someCountedReferenceData["count"];
            bool prerequisites = someCountedReferenceData["prerequisites"] != null;

            return new Single(EEquipmentOptionType.Single, new CountedReference(name, quantity, prerequisites));

        }

        Multiple GetMultiple(JToken someMultipleData) 
        {
            Multiple multiple = new Multiple(EEquipmentOptionType.Multiple);

            int count = someMultipleData.Children().Count();

            for (int i = 0; i < count; i++) 
            {
                switch ((string)someMultipleData[i]["option_type"])
                {
                    case "counted_reference":
                        Single single = GetSingle(someMultipleData[i]);
                        multiple.AddOption(single);
                        break;
                    case "choice":
                        Choice choice = GetChoice(someMultipleData[i]);
                        multiple.AddOption(choice);
                        break;
                    default:
                        break;
                }              
            }

            return multiple;
        }

        Choice GetChoice(JToken someChoiceData) 
        {
            int numberOfChoices = (int)someChoiceData["choice"]["choose"];
            Choice choice = new Choice(EEquipmentOptionType.Choice, numberOfChoices);

            string url = (string)someChoiceData["choice"]["from"]["equipment_category"]["url"];
            List<string> equipmentList = GetEquipmentListFromApiAddress(url);

            choice.AddOptions(equipmentList);

            return choice;
        }
        
        List<string> GetEquipmentListFromApiAddress(string anApiAddress)
        {
            List<string> equipmentList = new List<string>();

            HttpClient httpClient = new();
            JObject equipmentData = JObject.Parse(httpClient.GetStringAsync(myBaseApiAddress + anApiAddress).Result);

            int count;
            bool hasCountVariable = false;

            try
            {
                count = (int)equipmentData["count"];
                hasCountVariable = true;

            }
            catch (System.Exception)
            {

                count = equipmentData["equipment"].Children().Count();
            }

            string name;
            for (int i = 0; i < count; i++)
            {

                name = hasCountVariable ? (string)equipmentData["results"][i]["name"] : (string)equipmentData["equipment"][i]["name"];
                equipmentList.Add(name);
            }

            return equipmentList;
        }

        public List<string> GetStartingItemsList()
        {
            return myStartingEquipmentList;
        }

        public List<Options> GetStartingOptionsList()
        {
            return myStartingEquipmentOptionsList;
        }
        
    }


}
