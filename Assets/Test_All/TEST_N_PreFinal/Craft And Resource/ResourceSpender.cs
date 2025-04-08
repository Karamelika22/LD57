using System.Collections;
using UnityEngine;

public class ResourceSpender : MonoBehaviour
{
    private Coroutine[] spendingCoroutines;

    private void Start()
    {
        // �������������� ������ �������
        spendingCoroutines = new Coroutine[4];

        // ��������� �������� ��� ������� �������
        for (int i = 0; i < 4; i++)
        {
            if (i == 1)
            {
                // ������ ����� ��� ������� � �������� 1 (������ 3 ������� �� 0.55)
                spendingCoroutines[i] = StartCoroutine(SpendResourceRoutine(i, 3f, 0.45f, 0.55f));
            }
            else
            {
                // ��� ��������� �������� - ������ 10 ������ �� 1 �� 3 ������
                spendingCoroutines[i] = StartCoroutine(SpendResourceRoutine(i, 8f, 1f, 1.8f));
            }
        }
    }

    private IEnumerator SpendResourceRoutine(int resourceIndex, float interval, float minAmount, float maxAmount)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // ���������, ��� ������� �������� ����������
            if (ResourceSystem.Instance != null && ResourceSystem.Instance.resources.Length > resourceIndex)
            {
                float amountToSpend = Random.Range(minAmount, maxAmount);
                ResourceSystem.Instance.AddResource(resourceIndex,- amountToSpend);
                Debug.Log($"���������: ");
            }
        }
    }

    private void OnDestroy()
    {
        // ������������� ��� �������� ��� ����������� �������
        if (spendingCoroutines != null)
        {
            foreach (var coroutine in spendingCoroutines)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }
        }
    }
}