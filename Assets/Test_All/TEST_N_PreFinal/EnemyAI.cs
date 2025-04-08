using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform spawnPoint;
    private Transform attackPoint;
    private System.Action onDestroyCallback;

    public float moveSpeed = 2f;
    public float attackInterval = 5f;
    public int maxAttacks = 7;
    public float stayDuration = 35f;
    public float moveToPositionTime = 10f;//
    public float rotationSpeed = 180f;

    public int attackCount = 3;
    private bool isLeaving = false;
    private Quaternion targetRotation;
    
    public float health = 100f;
    public float stealAmount = 1f; // ���������� ��������, ���������� �� �����
    public float returnPercentage = 0.5f; // ������� ������������ �������� ��� ������

    private Dictionary<int, float> stolenResources = new(); // ������ ���������� ������� �� ��������
    private bool isDead = false;

    public void SetPoints(Transform spawn, Transform attack, System.Action callback)
    {
        spawnPoint = spawn;
        attackPoint = attack;
        onDestroyCallback = callback;

        // �������������� � ����� �����
        Vector2 direction = (attackPoint.position - transform.position).normalized;
        targetRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90);

        // �������� ��������
        StartCoroutine(RotateThenMove(targetRotation, attackPoint.position));
    }

    IEnumerator RotateThenMove(Quaternion rotation, Vector2 targetPosition)
    {
        // �������
        while (Quaternion.Angle(transform.rotation, rotation) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // ��������
        StartCoroutine(MoveToPositionWithSpeed(targetPosition));
    }
    IEnumerator MoveToPositionWithSpeed(Vector2 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        if (!isLeaving)
        {
            // �������� ��������� ����� ��������
            StartCoroutine(AttackRoutine());
        }
        else
        {
            DestroyEnemy();
        }
    }

    IEnumerator AttackRoutine()
    {
        while (attackCount < maxAttacks && !isLeaving && !isDead)
        {
            Attack();
            attackCount++;
            yield return new WaitForSeconds(attackInterval);
        }

        if (!isDead)
        {
            yield return new WaitForSeconds(stayDuration);
            Leave();
        }
    }

    void Attack()
    {
        // ������� ���� ������
        Health.Instance.Damage(attackCount);
        if (Health.Instance.currentArmor != 15)
        {
            // ������ ��� �������, ����� ������� 1
            for (int i = 0; i < ResourceSystem.Instance.resources.Length; i++)
            {
                if (i == 1) continue; // ���������� ������ � �������� 1

                float stolen = Mathf.Min(ResourceSystem.Instance.resources[i].currentAmount, stealAmount);
                ResourceSystem.Instance.resources[i].currentAmount -= stolen;

                // ��������� ���������� ����������
                if (stolenResources.ContainsKey(i))
                    stolenResources[i] += stolen;
                else
                    stolenResources.Add(i, stolen);

                // ��������� UI
                ResourceSystem.Instance.UpdateAllUI();
            }

            Debug.Log(gameObject.name + " attacking! Stolen resources from all except index 1");
        }
        else
        {
            Debug.Log(gameObject.name + " attacking! Armor is 20 - no resources stolen");
        }
        StartCoroutine(AttackVisual());
    
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        StopAllCoroutines();

        // ���������� ����� ���������� ��������
        foreach (var resource in stolenResources)
        {
            int index = resource.Key;
            float returnedAmount = resource.Value * returnPercentage;
            ResourceSystem.Instance.resources[index].currentAmount += returnedAmount;

            // ��������� UI
            ResourceSystem.Instance.resources[index].uiFillImage.fillAmount =
                ResourceSystem.Instance.resources[index].currentAmount / ResourceSystem.Instance.resources[index].maxAmount;
        }

        stolenResources.Clear();
        DestroyEnemy();
    }
    void Leave()
    {
        isLeaving = true;
        // �������������� � ����� ������
        Vector2 direction = (spawnPoint.position - transform.position).normalized;
        targetRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        StartCoroutine(RotateThenMove(targetRotation, spawnPoint.position));
    }
    IEnumerator AttackVisual()
    {
        // ������ ����������� ������� �����
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sr.color = originalColor;
    }
    void DestroyEnemy()
    {
        onDestroyCallback?.Invoke();
        Destroy(gameObject);
    }
}
