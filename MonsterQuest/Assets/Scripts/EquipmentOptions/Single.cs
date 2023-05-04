using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Single : Option
    {
        public override EEquipmentOptionType Type { get => myType; set => myType = value; }
        public override string EquipmentString => myEquipmentItems[0].EquipmentString;
        public override int Quantity => myEquipmentItems[0].Quantity;
        public override List<Option> Options => null;

        EEquipmentOptionType myType;
        List<CountedReference> myEquipmentItems;

        public Single(EEquipmentOptionType aType, CountedReference anEquipmentItem)
        {
            myType = aType;
            myEquipmentItems = new List<CountedReference> {anEquipmentItem};
        }
        /// <summary>
        /// Throws an error if called on Single,
        /// Adds new Option to List
        /// </summary>
        /// <param name="anOption">Can </param>
        public override void AddOption(Option anOption)
        {
            Debug.LogError("Can not add Option to Single, set Single through constructor");
            return;
        }

        public override List<CountedReference> GetEquipmentItems()
        {
            return myEquipmentItems;
        }

        public override Dictionary<string, int> GetEquipmentNames()
        {
            return new Dictionary<string, int>() { { myEquipmentItems[0].EquipmentString, myEquipmentItems[0].Quantity } };
        }
    }
}
