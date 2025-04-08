using System.Collections;
using UnityEngine;

public class WoodpeckerAI : MonoBehaviour
{
    private Transform targetPoint;
    private Vector3 spawnPosition; // �����, ������ �������� �����
    private float speed = 2f;
    private bool isReturning = false;
    private SpriteRenderer spriteRenderer;

    public void Initialize(Transform target)
    {
        targetPoint = target;
        spawnPosition = transform.position; // ����������, ������ ��������
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (targetPoint == null) return;

        if (!isReturning)
        {
            // ����� � ����
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            // ���� �������� ����
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                StartCoroutine(PeckAndReturn());
            }
        }
        else
        {
            // ����� ������� � ������
            transform.position = Vector3.MoveTowards(transform.position, spawnPosition, speed * Time.deltaTime);

            // ���� ��������� � ����������
            if (Vector3.Distance(transform.position, spawnPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator PeckAndReturn()
    {
        FallingObjectSpawner.Instance.StartSpawning(0.8f);
        // 1. �������� �������� (����� 3 �������)
        yield return new WaitForSeconds(3f);
        FallingObjectSpawner.Instance.StopSpawning();
        // 2. ������ �������� ����� �� 3 ������� �� X
        Vector3 startPos = transform.position;
        Vector3 retreatPos = startPos + new Vector3(-3f, 0, 0);
        float retreatDuration = 1f; // �� ������� ������ �������� �����
        float elapsedTime = 0f;

        while (elapsedTime < retreatDuration)
        {
            transform.position = Vector3.Lerp(
                startPos,
                retreatPos,
                elapsedTime / retreatDuration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 3. ������������� ������
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = true;
        }

        // 4. ����� ������� � ����� ������
        isReturning = true;
    }
}