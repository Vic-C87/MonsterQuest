using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomEditor(typeof(MonsterType))]
    public class MonsterTypeEditor : Editor
    {

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();

            InspectorElement.FillDefaultInspector(inspector, serializedObject, this);

            return inspector;
        }
    }
}
