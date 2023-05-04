using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public abstract class Option
    {
        public abstract EEquipmentOptionType Type { get; set; }
        public abstract string EquipmentString { get; }
        public abstract int Quantity { get; }
        public abstract List<Option> Options { get; }
        public abstract void AddOption(Option anOption);
        public abstract List<CountedReference> GetEquipmentItems();
        public abstract Dictionary<string, int> GetEquipmentNames();
    }
}
