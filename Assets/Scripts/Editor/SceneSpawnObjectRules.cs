using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    [InitializeOnLoad]
    public static class SceneObjectRules
    {
        private const string SpawnAtZeroPrefsKey = "SpawnAtZero";

        static List<GameObject> knownObjects = new();

        static SceneObjectRules()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            RefreshKnownObjects();
        }

        private static void RefreshKnownObjects()
        {
            knownObjects.Clear();
            foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                knownObjects.Add(obj);
            }
        }

        private static void OnHierarchyChanged()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isUpdating)
                return;
            
            knownObjects.RemoveAll(obj => obj == null);

            bool spawnAtZero = EditorPrefs.GetBool(SpawnAtZeroPrefsKey);
            
            foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (!knownObjects.Contains(obj))
                {
                    knownObjects.Add(obj);

                    if (spawnAtZero)
                        obj.transform.position = Vector3.zero;
                }
            }
        }
    }
}