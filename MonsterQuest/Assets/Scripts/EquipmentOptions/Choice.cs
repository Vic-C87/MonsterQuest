using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Choice : Option
    {
        public override EEquipmentOptionType Type { get => myType; set => myType = value; }
        public override string EquipmentString => myDescription;
        public override int Quantity => myNumberOfChoices;
        public override List<Option> Options => myOptions;

        EEquipmentOptionType myType;
        string myDescription;
        int myNumberOfChoices;
        List<Option> myOptions;

        public Choice(EEquipmentOptionType aType, int aNumberOfChoices)
        {
            myType= aType;
            myNumberOfChoices = aNumberOfChoices;
            myOptions = new List<Option>();
        }

        public override void AddOption(Option anOption)
        {
            myOptions.Add(anOption);
        }

        public void AddOptions(List<string> anEquipmentList) 
        {
            foreach (string item in anEquipmentList)
            {
                Single equipment = new Single(EEquipmentOptionType.Single, new CountedReference(item, 1));
                AddOption(equipment);
            }
        }

        public override List<CountedReference> GetEquipmentItems()
        {
            List<CountedReference> countedReferences = new List<CountedReference>();

            foreach (Option option in myOptions)
            {
                if (option.Type == EEquipmentOptionType.Single)
                {
                    countedReferences.Add(option.GetEquipmentItems()[0]);
                }
                else
                {
                    Debug.Log(option.Type);
                }
            }

            return countedReferences;
        }

        public override Dictionary<string, int> GetEquipmentNames()
        {
            Dictionary<string, int> names = new Dictionary<string, int>();
            foreach(CountedReference item in GetEquipmentItems())
            {
                names.Add(item.EquipmentString, item.Quantity);
            }
            return names;
        }

        public void SetDescriptionString(string aDescription)
        {
            myDescription = aDescription;
        }
    }
}
