using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Combat
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawning")] [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private List<Vector3> spawnPoints = new();

        private List<GameObject> spawnedEnemies = new();

        void Start()
        {
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0 || spawnPoints.Count == 0)
            {
                Debug.LogWarning("Enemy prefab or spawn points not set!");
                return;
            }

            foreach (var localPos in spawnPoints)
            {
                SpawnEnemyAtPoint(localPos);
            }
        }

        GameObject SpawnEnemyAtPoint(Vector3 localPos)
        {
            Vector3 worldPos = transform.TransformPoint(localPos);
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(prefab, worldPos, transform.rotation, transform);

            spawnedEnemies.Add(enemy);
            return enemy;
        }

        public void DespawnAllEnemies()
        {
            foreach (GameObject enemy in spawnedEnemies)
            {
                if (enemy != null)
                    Destroy(enemy);
            }

            spawnedEnemies.Clear();
        }

        // For better visualisation of the spawn positions
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            foreach (var localPos in spawnPoints)
            {
                Vector3 worldPos = transform.TransformPoint(localPos);
                Gizmos.DrawWireCube(worldPos, Vector3.one * 0.5f);
            }
        }

        // Only used by the custom editor
        public List<Vector3> GetSpawnPoints() => spawnPoints;
    }
}
