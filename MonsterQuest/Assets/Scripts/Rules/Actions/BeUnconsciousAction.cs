using System.Collections;

namespace MonsterQuest
{
    public class BeUnconsciousAction :IAction
    {
        Character myCharacter;

        public BeUnconsciousAction(Character aCharacter)
        {
            myCharacter = aCharacter;
        }


        public IEnumerator Execute()
        {
            int roll = DiceHelper.Roll("d20");
            Console.WriteLine(myCharacter.myDisplayName + " is unconscious and has to perform a death saving roll: " + roll);
            yield return myCharacter.DeathSavingThrow(roll);

            if (myCharacter.myLifeStatus == ELifeStatus.Conscious)
            {
                Console.WriteLine(myCharacter.myDisplayName + " regains consciousness!");
            }
        }

        
    }
}
