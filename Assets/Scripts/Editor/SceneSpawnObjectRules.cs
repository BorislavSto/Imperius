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
            knownObjects.RemoveAll(obj => obj == null);

            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
            {
                if (!knownObjects.Contains(obj))
                {
                    knownObjects.Add(obj);

                    if (EditorPrefs.GetBool(SpawnAtZeroPrefsKey, true))
                        obj.transform.position = Vector3.zero;
                }
            }
        }
    }
}