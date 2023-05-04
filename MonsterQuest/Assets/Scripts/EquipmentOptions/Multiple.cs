using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Multiple : Option
    {
        public override EEquipmentOptionType Type { get => myType; set => myType = value; }
        public override string EquipmentString => myDescription;
        public override int Quantity => Options.Count;
        public override List<Option> Options => myOptions;
        
        EEquipmentOptionType myType;
        string myDescription;
        int myCount;
        List<Option> myOptions;

        public Multiple(EEquipmentOptionType aType) 
        {
            myType = aType;
            myOptions = new List<Option>();
        }

        public override void AddOption(Option anOption)
        {
            for (int i = 0; i < myOptions.Count; i++) 
            {
                if (myOptions[i].EquipmentString == anOption.EquipmentString) 
                {
                    myOptions[i] = new Single(EEquipmentOptionType.Single, new CountedReference(anOption.EquipmentString, myOptions[i].Quantity + anOption.Quantity));
                    return;
                }
            }
            myOptions.Add(anOption);
        }

        public override List<CountedReference> GetEquipmentItems()
        {
            List<CountedReference> countedReferences= new List<CountedReference>();

            foreach(Option option in myOptions)
            {
                switch (option.Type)
                {
                    case EEquipmentOptionType.Single:
                        countedReferences.Add(option.GetEquipmentItems()[0]);
                        break;
                    case EEquipmentOptionType.Choice:
                        for (int i = 0; i < option.Options.Count; i++)
                        {
                            countedReferences.Add(option.Options[i].GetEquipmentItems()[0]);
                        }
                        break;
                    default:
                        break;
                }
            }

            return countedReferences;
        }

        public override Dictionary<string, int> GetEquipmentNames()
        {
            Dictionary<string, int> names = new Dictionary<string, int>();
            foreach (CountedReference item in GetEquipmentItems())
            {
                names.Add(item.EquipmentString, item.Quantity);
            }
            return names;
        }
    }
}
