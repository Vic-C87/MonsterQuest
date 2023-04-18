using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Random = System.Random;

namespace MonsterQuest
{
    public static class DiceHelper
    {
        static Random myRandom = new Random();

        public static int Roll(string aDiceType)
        {
            string pattern = @"(\d{0,3})d([468]|10|20)(\s|([-+])(\d{1,2}))?";
            int numberOfRolls = 1;
            int facesOnDice = 0;
            int followNumber = 0;
            int result = 0;

            MatchCollection matches = Regex.Matches(aDiceType, pattern);
            foreach (Match match in matches)
            {
                GroupCollection data = match.Groups;

                _ = int.TryParse(data[1].Value, out numberOfRolls);
                _ = int.TryParse(data[2].Value, out facesOnDice);
                _ = int.TryParse(data[5].Value, out followNumber);
                if (data[4].Value == "-")
                {
                    followNumber -= (2 * followNumber);
                }
            }
            if (numberOfRolls == 0)
            {
                numberOfRolls = 1;
            }
            for (int i = 1; i <= numberOfRolls; i++)
            {
                result += GetRandom(facesOnDice);
            }

            result += followNumber;


            return result;
        }

        public static int RollAbilityScore()
        {
            int score = 0;
            int lowest = 7;

            for (int i = 0; i < 4; i++)
            {
                int roll = GetRandom(6);
                score += roll;
                if (roll < lowest)
                {
                    lowest = roll;
                }
            }

            score -= lowest;

            return score;
        }
        /// <summary>
        /// Returns random int between 1 and max inclusive
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandom(int max)
        {
            int result;
            result = myRandom.Next(1, max + 1);
            return result;
        }

    }
}
