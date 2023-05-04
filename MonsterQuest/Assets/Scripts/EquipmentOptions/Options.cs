using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class Options
    {
        public string Description { get; private set; }
        public int NumberOfChoices { get; private set; }
        public List<Option> MyOptions { get; private set; }



        public Options(string aDescription, int aNumberOfChoices)
        {
            MyOptions = new List<Option>();
            Description = aDescription;
            NumberOfChoices = aNumberOfChoices;
        }

        /// <summary>
        /// Adds an option type to OptionList
        /// </summary>
        /// <param name="anOption">Single,Multiple or Choice</param>
        public void AddOption(Option anOption) 
        {
            MyOptions.Add(anOption);
        }
    }
}
