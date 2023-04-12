using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace MonsterQuest
{
    [CustomEditor(typeof(MonsterType))]
    public class MonsterTypeEditor : Editor
    {
        DropdownField myMonstersDropdown;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();

            VisualElement importField = new VisualElement();
            importField.style.flexDirection = FlexDirection.Row;

            importField.Add(new Label("Import Monster: "));
            myMonstersDropdown = new DropdownField();
            myMonstersDropdown.choices.AddRange(MonsterTypeImporter.MonsterIndexNames);
            myMonstersDropdown.RegisterValueChangedCallback(OnMonsterChange);
            importField.Add(myMonstersDropdown);

            inspector.Add(importField);


            InspectorElement.FillDefaultInspector(inspector, serializedObject, this);

            return inspector;
        }

        private void OnMonsterChange(ChangeEvent<string> aChange)
        {
            
        }
    }
}
