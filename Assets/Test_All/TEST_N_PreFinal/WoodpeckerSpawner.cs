using System.Collections;
using UnityEngine;

public class WoodpeckerSpawner : MonoBehaviour
{
    public GameObject woodpeckerPrefab;
    public Transform[] spawnPoints;
    public Transform[] resourcePoints;
    public float spawnInterval = 18f;

    void Start()
    {
        StartCoroutine(SpawnWoodpeckers());
    }

    IEnumerator SpawnWoodpeckers()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spawnPoints.Length == 0 || resourcePoints.Length == 0)
            {
                Debug.LogWarning("No spawn points or resource points assigned!");
                continue;
            }

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Transform targetPoint = GetClosestResourcePoint(spawnPoint.position);

            GameObject woodpecker = Instantiate(woodpeckerPrefab, spawnPoint.position, Quaternion.identity);
            woodpecker.GetComponent<WoodpeckerAI>().Initialize(targetPoint);
        }
    }

    Transform GetClosestResourcePoint(Vector3 spawnPosition)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform point in resourcePoints)
        {
            float distance = Vector3.Distance(spawnPosition, point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = point;
            }
        }
        return closest;
    }
}
