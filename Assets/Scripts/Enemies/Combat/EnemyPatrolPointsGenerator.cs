using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Combat
{
    public static class EnemyPatrolPointsGenerator
    {
        public static List<GameObject> GeneratePatrolPoints(
            Transform center, 
            float radius, 
            int numPoints,
            LayerMask groundMask,
            Transform parentContainer = null)
        {
            List<GameObject> points = new List<GameObject>();

            // have them all under one PatrolPointsGO
            GameObject container;
            if (parentContainer != null)
            {
                container = parentContainer.gameObject;
            }
            else
            {
                container = new GameObject("PatrolPoints");
                container.transform.position = center.position;
            }


            for (int i = 0; i < numPoints; i++)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-radius, radius),
                    10f,
                    Random.Range(-radius, radius)
                );

                Vector3 spawnPos = center.position + randomOffset;

                if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f, groundMask))
                {
                    spawnPos = hit.point;
                }
                else
                {
                    spawnPos = center.position;
                }

                GameObject pointObj = new GameObject("PatrolPoint");
                pointObj.transform.position = spawnPos;
                pointObj.transform.parent = container.transform;
                points.Add(pointObj);
            }

            return points;
        }
    }
}
