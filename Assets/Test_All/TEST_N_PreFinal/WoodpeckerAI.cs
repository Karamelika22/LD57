using System.Collections;
using UnityEngine;

public class WoodpeckerAI : MonoBehaviour
{
    private Transform targetPoint;
    private Vector3 spawnPosition; // Точка, откуда появился дятел
    private float speed = 2f;
    private bool isReturning = false;
    private SpriteRenderer spriteRenderer;

    public void Initialize(Transform target)
    {
        targetPoint = target;
        spawnPosition = transform.position; // Запоминаем, откуда прилетел
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (targetPoint == null) return;

        if (!isReturning)
        {
            // Летим к цели
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            // Если достигли цели
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                StartCoroutine(PeckAndReturn());
            }
        }
        else
        {
            // Летим обратно к спавну
            transform.position = Vector3.MoveTowards(transform.position, spawnPosition, speed * Time.deltaTime);

            // Если вернулись — уничтожаем
            if (Vector3.Distance(transform.position, spawnPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator PeckAndReturn()
    {
        FallingObjectSpawner.Instance.StartSpawning(0.8f);
        // 1. Анимация клевания (стоим 3 секунды)
        yield return new WaitForSeconds(3f);
        FallingObjectSpawner.Instance.StopSpawning();
        // 2. Плавно отлетаем назад на 3 единицы по X
        Vector3 startPos = transform.position;
        Vector3 retreatPos = startPos + new Vector3(-3f, 0, 0);
        float retreatDuration = 1f; // За сколько секунд отлетаем назад
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

        // 3. Разворачиваем спрайт
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = true;
        }

        // 4. Летим обратно к точке спавна
        isReturning = true;
    }
}