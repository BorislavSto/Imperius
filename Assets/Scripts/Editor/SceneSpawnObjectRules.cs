using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
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
            
            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
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