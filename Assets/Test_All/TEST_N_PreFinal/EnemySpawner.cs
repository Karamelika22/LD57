using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public Transform[] spawnPoints;
    public Transform[] attackPoints;
    public float spawnInterval = 10f;

    private List<Transform> availableAttackPoints = new();
    void Start()
    {
        // �������������� ������ ��������� ����� �����
        availableAttackPoints.AddRange(attackPoints);
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (availableAttackPoints.Count == 0)
            {
                Debug.Log("No available attack points, waiting...");
                yield return new WaitForSeconds(1f);
                continue;
            }
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            // ������� ��������� ��������� ����� �����
            Transform closestAttackPoint = GetClosestAttackPoint(spawnPoint.position);
            availableAttackPoints.Remove(closestAttackPoint);

            // ������� ����� � �������� ��� ������ �����
            GameObject enemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPoint.position, Quaternion.identity);
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

            // �������� ����� � ������ ��� ������������ ����� �����
            enemyAI.SetPoints(spawnPoint, closestAttackPoint, () => {
                availableAttackPoints.Add(closestAttackPoint);
            });

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    Transform GetClosestAttackPoint(Vector3 spawnPosition)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform point in availableAttackPoints)
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
