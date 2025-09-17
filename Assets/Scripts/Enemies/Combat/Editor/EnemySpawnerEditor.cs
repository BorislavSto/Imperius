using UnityEditor;
using UnityEngine;

namespace Enemies.Combat.Editor
{
    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerEditor : UnityEditor.Editor
    {
        void OnSceneGUI()
        {
            EnemySpawner spawner = (EnemySpawner)target;
            var spawnPoints = spawner.GetSpawnPoints();

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Vector3 worldPos = spawner.transform.TransformPoint(spawnPoints[i]);

                // Create a draggable handle
                EditorGUI.BeginChangeCheck();
                Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spawner, "Move Spawn Point");
                    spawnPoints[i] = spawner.transform.InverseTransformPoint(newWorldPos);
                    EditorUtility.SetDirty(spawner);
                }

                // Draw label
                Handles.Label(newWorldPos + Vector3.up * 0.5f, $"Spawn Point {i}");
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EnemySpawner spawner = (EnemySpawner)target;
            var spawnPoints = spawner.GetSpawnPoints();

            if (GUILayout.Button("Add Spawn Point"))
            {
                Undo.RecordObject(spawner, "Add Spawn Point");
                spawnPoints.Add(Vector3.forward * 2f); // default position
                EditorUtility.SetDirty(spawner);
            }

            if (GUILayout.Button("Clear Spawn Points"))
            {
                Undo.RecordObject(spawner, "Clear Spawn Points");
                spawnPoints.Clear();
                EditorUtility.SetDirty(spawner);
            }
        }
    }
}