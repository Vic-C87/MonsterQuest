using System.Collections;
using System.Collections.Generic;

namespace MonsterQuest
{
    public class Party
    {
        List<Character> myCharacters;

        public Party(IEnumerable<Character> someInitialCharacters)
        {
            myCharacters = new List<Character>(someInitialCharacters);
        }

        public IEnumerable<Character> Characters => myCharacters;

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
            if (anIndexToRemove < Count())
            {
                myCharacters.RemoveAt(anIndexToRemove);
                return true;
            }
            return false;
        }

        public int Count()
        {
            return myCharacters.Count;
        }

        public bool IsEmpty()
        {
            if (myCharacters.Count > 0)
            {
                return false;
            }
            return true;
        }

    }
}
