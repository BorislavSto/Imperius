using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DevSettingsWindow : EditorWindow
    {
        private const string EditorPrefKey = "StartFromBootstrapScene";
        private bool isEnabled;

        [MenuItem("Tools/DevSettings")]
        public static void ShowWindow()
        {
            GetWindow<DevSettingsWindow>("DevSettings");
        }

        private void OnEnable()
        {
            isEnabled = EditorPrefs.GetBool(EditorPrefKey, false);
        }

        private void OnGUI()
        {
            GUILayout.Label("DevSettings", EditorStyles.boldLabel);
            
            bool newEnabled = EditorGUILayout.Toggle("Enable Bootstrapper", isEnabled);
            if (newEnabled != isEnabled)
            {
                isEnabled = newEnabled;
                EditorPrefs.SetBool(EditorPrefKey, isEnabled);
            }

            GUILayout.Space(10);
            GUILayout.Label("Other settings go here.");
        }
    }
}