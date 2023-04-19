using System;
using System.Collections;
using System.Collections.Generic;

namespace MonsterQuest
{
    [Serializable]
    public class Party
    {
        public List<Character> myCharacters { get; private set; }

        public Party(IEnumerable<Character> someInitialCharacters)
        {
            myCharacters = new List<Character>(someInitialCharacters);
        }

        public bool RemoveCharacter(Character aCharacterToRemove)
        {
            if (myCharacters.Contains(aCharacterToRemove))
            {
                myCharacters.Remove(aCharacterToRemove);
                return true;
            }
            return false;
        }

        public bool RemoveAt(int anIndexToRemove)
        {
            if (anIndexToRemove < myCharacters.Count)
            {
                myCharacters.RemoveAt(anIndexToRemove);
                return true;
            }
            return false;
        }

        public int Count()
        {
            int count = 0;
            foreach(Character aCharacter in myCharacters)
            {
                if (aCharacter.myLifeStatus != ELifeStatus.Dead)
                {
                    count += 1;
                }
            }
            return count;
        }

        public bool IsEmpty()
        {
            if (myCharacters.Count > 0)
            {
                return false;
            }
            return true;
        }

        public List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (Character character in myCharacters)
            {
                names.Add(character.myDisplayName);
            }
            return names;
        }

        public bool OneAlive()
        {
            bool isAlive = false;
            foreach (Character character in myCharacters)
            {
                if (character.myLifeStatus == ELifeStatus.Conscious)
                {
                    isAlive = true;
                    break;
                }
            }

            return isAlive;
        }

    }
}
