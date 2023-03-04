using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomPropertyDrawer(typeof(AbilityScores))]
    public class AbilityScoresPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            Label label = new Label("Ability Scores");
            container.Add(label);
            
            container.Add(new PropertyField(property.FindPropertyRelative("<Strength>k__BackingField.<Score>k__BackingField"), "Strenght"));            
            container.Add(new PropertyField(property.FindPropertyRelative("<Dexterity>k__BackingField.<Score>k__BackingField"), "Dexterity"));
            container.Add(new PropertyField(property.FindPropertyRelative("<Constitution>k__BackingField.<Score>k__BackingField"), "Constitution"));
            container.Add(new PropertyField(property.FindPropertyRelative("<Intelligence>k__BackingField.<Score>k__BackingField"), "Intelligence"));
            container.Add(new PropertyField(property.FindPropertyRelative("<Wisdom>k__BackingField.<Score>k__BackingField"), "Wisdom"));
            container.Add(new PropertyField(property.FindPropertyRelative("<Charisma>k__BackingField.<Score>k__BackingField"), "Charisma"));


            return container;
        }
    }
}
