using System;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScore 
    {
        [field: SerializeField]
        public int Score { get; set; }

        public int Modifier 
        { 
            get 
            {
                int mod = Score;
                if (mod%2 != 0)
                {
                    mod -= 1;
                }
                return (mod - 10) / 2;
            } 
        }

        public AbilityScore()
        {

        }

        public AbilityScore(int aScore)
        {
            Score = aScore;
        }

        public static implicit operator int(AbilityScore anAbilityScore)
        {
            return anAbilityScore.Score;
        }
    }
}
