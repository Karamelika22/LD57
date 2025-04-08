using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    public GameObject fallingObjectPrefab;
    public GameObject leavesPilePrefab;
    public Transform[] spawnPoints;
    public Transform[] pileSpawnPoints; // ��������� ����� ��� ��� �������
    public Transform groundPoint;
    public int maxPiles = 2; // ������������ ���������� ���

    [SerializeField] private int fallCount = 0;
    [SerializeField] private const int FallsToSpawnPile = 2;
    [SerializeField] private bool isSpawningActive = false;
    [SerializeField] private List<GameObject> activePiles = new();
    [SerializeField] float checkRadius = 1f;
    [SerializeField] private LayerMask pileLayer; // ��������� ���� "Pile" � ����������
    public static FallingObjectSpawner Instance { get; private set; }
    // ���������� ������� �������� ��� ������� ������
    private void Awake()
    {
        Instance = this;
    }
    public void StartSpawning(float interval)
    {
        if (!isSpawningActive)
        {
            isSpawningActive = true;
            StartCoroutine(SpawnFallingObjects(interval));
        }
    }

    // ���������� ������� �������� ��� ��������� ������
    public void StopSpawning()
    {
        isSpawningActive = false;
        StopAllCoroutines();
    }

    IEnumerator SpawnFallingObjects(float interval)
    {
        while (isSpawningActive)
        {
            yield return new WaitForSeconds(interval);

            if (spawnPoints.Length == 0)
            {
                Debug.LogWarning("No spawn points assigned!");
                continue;
            }

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject fallingObj = Instantiate(fallingObjectPrefab, spawnPoint.position, Quaternion.identity);
            fallingObj.GetComponent<FallingObject>().Initialize(groundPoint, this);
        }
    }

    public void OnObjectFell()
    {
        fallCount++;
        if (fallCount >= FallsToSpawnPile)
        {
            fallCount = 0;
            TrySpawnLeavesPile();
        }
    }
    void TrySpawnLeavesPile()
    {
        
        // ��������� ������������ ���������� ���
        if (activePiles.Count >= maxPiles) return;

        if (pileSpawnPoints.Length > 0 && leavesPilePrefab != null)
        {
            // ������� ��������� �����
            var freePoints = pileSpawnPoints.Where(p => !IsPointOccupied(p.position)).ToArray();
            if (freePoints.Length == 0) return;

            Transform spawnPoint = freePoints[Random.Range(0, freePoints.Length)];
            GameObject pile = Instantiate(leavesPilePrefab, spawnPoint.position, Quaternion.identity);
            activePiles.Add(pile);

            // ��������� �������������� �������� ��� �����������
            pile.AddComponent<PileCleanup>().Initialize(this);
        }
        Debug.LogWarning($"������� ������ ����. �������� ���: {activePiles.Count}/{maxPiles}");
    }

    bool IsPointOccupied(Vector3 point)
    {
        Collider2D hit = Physics2D.OverlapCircle(point, checkRadius, pileLayer);
        if (hit != null)
        {
            Debug.Log("����� ������ ��������: " + hit.gameObject.name);
            return true;
        }
        return false;
    }

    public void RemovePile(GameObject pile)
    {
        activePiles.Remove(pile);
    }
    
}
