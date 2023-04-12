using System;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScores
    {
        
        [field: SerializeField]
        public AbilityScore Strength { get; set; }
        [field: SerializeField]
        public AbilityScore Dexterity { get; set; }
        [field: SerializeField]
        public AbilityScore Constitution { get; set; }
        [field: SerializeField]
        public AbilityScore Intelligence { get; set; }
        [field: SerializeField]
        public AbilityScore Wisdom { get; set; }
        [field: SerializeField]
        public AbilityScore Charisma { get; set; }
        
        public AbilityScores(bool aRollAutomatic = false)
        {
            if (aRollAutomatic)
            {
                Strength = new(DiceHelper.RollAbilityScore());
                Dexterity = new(DiceHelper.RollAbilityScore());
                Constitution = new(DiceHelper.RollAbilityScore());
                Intelligence = new(DiceHelper.RollAbilityScore());
                Wisdom = new(DiceHelper.RollAbilityScore());
                Charisma = new(DiceHelper.RollAbilityScore());
            }
        }

        public AbilityScore this[EAbility anAbility]
        {
            get
            {
                switch (anAbility)
                {
                    case EAbility.None:
                        return null;
                    case EAbility.Strength:
                        return Strength;
                    case EAbility.Dexterity:
                        return Dexterity;
                    case EAbility.Constitution:
                        return Constitution;
                    case EAbility.Intelligence:
                        return Intelligence;
                    case EAbility.Wisdom:
                        return Wisdom;
                    case EAbility.Charisma:
                        return Charisma;
                    default:
                        return null;
                }
            }
        }

    }
}
