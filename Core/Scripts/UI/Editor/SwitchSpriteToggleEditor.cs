//Developed by Pavel Kravtsov.
using UnityEditor;

namespace Core
{
    [CustomEditor(typeof(SwitchSpriteToggle))]
    public class SwitchSpriteToggleEditor : UnityEditor.UI.ToggleEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            SwitchSpriteToggle toggle = (SwitchSpriteToggle)target;

            EditorGUILayout.BeginHorizontal();
            serializedObject.Update();
            toggle.onImage = (UnityEngine.UI.Image)EditorGUILayout.ObjectField("On Image", toggle.onImage, typeof(UnityEngine.UI.Graphic), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            serializedObject.Update();
            toggle.offImage = (UnityEngine.UI.Image)EditorGUILayout.ObjectField("Off Image", toggle.offImage, typeof(UnityEngine.UI.Graphic), true);
            EditorGUILayout.EndHorizontal();
        }
    }
}