using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class CountedReference
    {
        public string EquipmentString => myEquipmentName;
        public int Quantity => myQuantity;
        public bool RequiresProficiency => myRequiresProficiency;
        
        string myEquipmentName;
        int myQuantity;
        bool myRequiresProficiency;

        public CountedReference(string anEquipmentName, int aQuantity, bool aRequiresProficiency = false)
        {
            myEquipmentName = anEquipmentName;
            myQuantity = aQuantity;
            myRequiresProficiency = aRequiresProficiency;
        }

    }
}
