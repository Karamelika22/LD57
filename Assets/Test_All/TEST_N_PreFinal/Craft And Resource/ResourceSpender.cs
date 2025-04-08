using System.Collections;
using UnityEngine;

public class ResourceSpender : MonoBehaviour
{
    private Coroutine[] spendingCoroutines;

    private void Start()
    {
        // Инициализируем массив корутин
        spendingCoroutines = new Coroutine[4];

        // Запускаем корутины для каждого ресурса
        for (int i = 0; i < 4; i++)
        {
            if (i == 1)
            {
                // Особый режим для ресурса с индексом 1 (каждые 3 секунды по 0.55)
                spendingCoroutines[i] = StartCoroutine(SpendResourceRoutine(i, 3f, 0.45f, 0.55f));
            }
            else
            {
                // Для остальных ресурсов - каждые 10 секунд от 1 до 3 единиц
                spendingCoroutines[i] = StartCoroutine(SpendResourceRoutine(i, 8f, 1f, 1.8f));
            }
        }
    }

    private IEnumerator SpendResourceRoutine(int resourceIndex, float interval, float minAmount, float maxAmount)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // Проверяем, что система ресурсов существует
            if (ResourceSystem.Instance != null && ResourceSystem.Instance.resources.Length > resourceIndex)
            {
                float amountToSpend = Random.Range(minAmount, maxAmount);
                ResourceSystem.Instance.AddResource(resourceIndex,- amountToSpend);
                Debug.Log($"Потрачено: ");
            }
        }
    }

    private void OnDestroy()
    {
        // Останавливаем все корутины при уничтожении объекта
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