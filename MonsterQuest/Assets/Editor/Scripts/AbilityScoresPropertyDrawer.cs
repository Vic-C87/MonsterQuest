using System;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
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

            

            container.style.flexDirection = FlexDirection.Row;
            container.style.alignSelf = Align.FlexEnd;
            
            VisualElement propertyLabel = new VisualElement();
            propertyLabel.style.flexDirection = FlexDirection.Column;
            propertyLabel.style.alignSelf = Align.Center;
            Label label = new Label();
            label.text = "Abilities: ";
            label.style.alignSelf = Align.Center;
            label.style.fontSize = 16;
            label.style.marginRight = 10;
            label.style.color = Color.Lerp(Color.Lerp(Color.red, Color.grey, .8f), Color.yellow, .5f);
            
            propertyLabel.Add(label);
            container.Add(propertyLabel);

            foreach (EAbility ability in Enum.GetValues(typeof(EAbility))) 
            {
                if (ability == EAbility.None) continue;

                VisualElement propertyBox = new VisualElement();
                propertyBox.style.flexDirection = FlexDirection.Column;
                propertyBox.style.paddingTop = 2;
                propertyBox.style.paddingLeft = 5;
                

                Label name = new Label();
                name.text = ability.ToString().Substring(0,3);
                name.style.alignSelf = Align.Center; 
                propertyBox.Add(name);

                IntegerField value = new IntegerField();
                value.style.alignSelf = Align.Center;
                value.bindingPath = $"<{ability.ToString()}>k__BackingField.<Score>k__BackingField";
                propertyBox.Add(value);


                Label modifier = new Label();
                propertyBox.Add(modifier);

                myModLabels[ability] = modifier;
                
                container.Add(propertyBox);
            }

            container.TrackSerializedObjectValue(property.serializedObject, ModifyModifiers);
            ModifyModifiers(property.serializedObject);


            return container;
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
