using Core;
using Enemies.Combat;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    public static class CustomHierarchyMenu
    {
        [MenuItem("GameObject/Custom/Enemy Spawner", false, 10)]
        public static void CreateEnemySpawner(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Enemy Spawner");
            go.AddComponent<EnemySpawner>();

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }        
        
        [MenuItem("GameObject/Custom/Level Manager", false, 10)]
        public static void CreateLevelManager(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Level Manager");
            go.AddComponent<SceneManager>();

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}