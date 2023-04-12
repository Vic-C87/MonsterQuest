using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomPropertyDrawer(typeof(AbilityScores))]
    public class AbilityScoresPropertyDrawer : PropertyDrawer
    {
        Dictionary<EAbility, Label> myModLabels = new Dictionary<EAbility, Label>();

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            Label label = new Label("\n\nAbility Scores: \n‾‾‾‾‾‾‾‾‾‾‾‾‾");
            container.Add(label);

            foreach (EAbility ability in Enum.GetValues(typeof(EAbility))) 
            {
                if (ability == EAbility.None) continue;

                VisualElement propertyBox = new VisualElement();
                propertyBox.style.flexDirection = FlexDirection.Row;
                container.Add(propertyBox);

                PropertyField field = new PropertyField();
                field.style.minWidth = 200;
                field.label = ability.ToString();
                field.bindingPath = $"<{ability.ToString()}>k__BackingField.<Score>k__BackingField";
                propertyBox.Add(field);

                Label modifier = new Label();
                modifier.style.marginLeft = 10;
                propertyBox.Add(modifier);
                myModLabels[ability] = modifier;
            }

            container.TrackSerializedObjectValue(property.serializedObject, OnValueChanged);
            ModifyModifiers(property.serializedObject);

            return container;
        }

        private void OnValueChanged(SerializedObject serializedObject)
        {
            ModifyModifiers(serializedObject);
        }

        private void ModifyModifiers(SerializedObject serializedObject)
        {
            if (serializedObject.targetObject is not MonsterType monsterType) return;

            foreach (EAbility ability in Enum.GetValues(typeof(EAbility)))
            {
                if (ability is EAbility.None) continue;

                myModLabels[ability].text = $"({monsterType.myAbilityScores[ability].Modifier:+#;-#;+0})";
            }
        }
    }
}
