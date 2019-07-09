using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core
{
    [CustomEditor(typeof(TextLocalization))]
    [CanEditMultipleObjects]
    public class UILocalizationEditor : Editor
    {
        //private readonly int lineHeight = 20;
        //private bool lanquageFoldot =false;
        private int intPopup = 0;
        SerializedProperty SearchVarProperty;

        public void OnEnable()
        {
            SearchVarProperty = serializedObject.FindProperty("Search");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var myTarget = (TextLocalization)target;

            if (!myTarget.IsHasOutputHelper())
            {
                EditorGUILayout.HelpBox("[UI Text] or [Text Mesh] script were not added to GameObject ", MessageType.Error);
                return;
            }

            //ShowAvailableKeyValues(myTarget);


            //search value func
            serializedObject.Update();

            string search = SearchVarProperty.stringValue;

            if (String.IsNullOrEmpty(search) || search.Trim().Length == 0)
                //ShowAvailableSearch(myTarget, "");
                ShowAvailableKeyValues(myTarget);
            else
                ShowAvailableSearch(myTarget, search);

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowAvailableKeyValues(TextLocalization myTarget)
        {
            var localizationKeys = LocalizationManager.GetLocalizationKeys();

            var keyId = GetIdByKey(myTarget.Key, localizationKeys);
            if (keyId == -1)
            {
                keyId = 0;
                EditorGUILayout.HelpBox("KEY not found in localization file. ", MessageType.Error);
            }

            intPopup = keyId;
            //Debug.Log("intPopup: " + intPopup);

            var listId = new int[localizationKeys.Length];
            for (var i = 0; i < localizationKeys.Length; i++)
                listId[i] = i;
            intPopup = EditorGUILayout.IntPopup("List of Keys", intPopup, localizationKeys, listId);

            if (keyId != intPopup || string.IsNullOrEmpty(myTarget.Key))
                myTarget.Key = localizationKeys[intPopup];
        }

        private void ShowAvailableSearch(TextLocalization myTarget, string search)
        {
            //var localizationKeys = LocalizationManager.GetLocalizationKeys();
            var localizationSearchKeys = SearchByKey(search, LocalizationManager.GetLocalizationKeys());

            var keyId = GetIdByKey(myTarget.Key, localizationSearchKeys);
            if (keyId == -1)
            {
                keyId = 0;
                //EditorGUILayout.HelpBox("KEY not found in localization file. ", MessageType.Error);
            }

            intPopup = keyId;

            var listId = new int[localizationSearchKeys.Length];
            for (var i = 0; i < localizationSearchKeys.Length; i++)
                listId[i] = i;
            //intPopup = EditorGUILayout.IntPopup("List of Keys", intPopup, localizationSearchKeys, listId);

            GUIStyle style = new GUIStyle();
            style.fixedHeight = 300;

            GUILayoutOption[] options = new GUILayoutOption[2] { GUILayout.ExpandHeight(true), GUILayout.ExpandHeight(true) };

            //GUILayoutOption.ExpandHeight(true);

            //intPopup = EditorGUILayout.IntPopup("List of Keys", intPopup, localizationSearchKeys, listId, options);
            intPopup = EditorGUILayout.IntPopup("List of Keys", intPopup, localizationSearchKeys, listId, options);

            if (keyId != intPopup || string.IsNullOrEmpty(myTarget.Key))
                myTarget.Key = localizationSearchKeys[intPopup];

            if (listId.Length == 1)
                myTarget.Key = localizationSearchKeys[listId[0]];
        }

        private string[] SearchByKey(string search, string[] localizationKey)
        {
            List<string> tempKeys = new List<string>();

            foreach (string key in localizationKey)
            {
                if (key.Contains(search))
                    tempKeys.Add(key);
            }

            return tempKeys.ToArray();
        }

        private int GetIdByKey(string key, string[] keys)
        {
            for (int index = 0; index < keys.Length; index++)
            {
                if (keys[index] == key)
                    return index;
            }
            return -1;
        }
    }

}