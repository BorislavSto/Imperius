using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DevSettingsWindow : EditorWindow
    {
        private const string BootstrapperPrefKey = "StartFromBootstrapScene";
        private bool bootstrapperIsEnabled;        
        
        private const string SpawnAtZeroPrefsKey = "SpawnAtZero";
        private bool spawnAtZeroIsEnabled;

        [MenuItem("Tools/DevSettings")]
        public static void ShowWindow()
        {
            GetWindow<DevSettingsWindow>("DevSettings");
        }

        private void OnEnable()
        {
            bootstrapperIsEnabled = EditorPrefs.GetBool(BootstrapperPrefKey, false);
            spawnAtZeroIsEnabled = EditorPrefs.GetBool(SpawnAtZeroPrefsKey, false);
        }

        private void OnGUI()
        {
            GUILayout.Label("DevSettings", EditorStyles.boldLabel);
            
            bool bootstrapper = EditorGUILayout.Toggle("Enable Bootstrapper Scene Switch on Play", bootstrapperIsEnabled);
            if (bootstrapper != bootstrapperIsEnabled)
            {
                bootstrapperIsEnabled = bootstrapper;
                EditorPrefs.SetBool(BootstrapperPrefKey, bootstrapperIsEnabled);
            }
            
            bool spawnAtZero = EditorGUILayout.Toggle("Enable Spawning Objects At 0,0,0", spawnAtZeroIsEnabled);
            if (spawnAtZero != spawnAtZeroIsEnabled)
            {
                spawnAtZeroIsEnabled = spawnAtZero;
                EditorPrefs.SetBool(SpawnAtZeroPrefsKey, spawnAtZeroIsEnabled);
            }

            GUILayout.Space(10);
            GUILayout.Label("Other settings go here.");
        }
    }
}