using System.Collections;
using UnityEngine;

public class WoodpeckerAI : MonoBehaviour
{
    private Transform targetPoint;
    private Vector3 spawnPosition; // Òî÷êà, îòêóäà ïîÿâèëñÿ äÿòåë
    private float speed = 2f;
    private bool isReturning = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public void Initialize(Transform target)
    {
        targetPoint = target;
        spawnPosition = transform.position; // Çàïîìèíàåì, îòêóäà ïðèëåòåë
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("isPecking", false);
        }
    }

    void Update()
    {
        if (targetPoint == null) return;

        if (!isReturning)
        {
            // Ëåòèì ê öåëè
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            // Åñëè äîñòèãëè öåëè
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                StartCoroutine(PeckAndReturn());
            }
        }
        else
        {
            // Ëåòèì îáðàòíî ê ñïàâíó
            transform.position = Vector3.MoveTowards(transform.position, spawnPosition, speed * Time.deltaTime);

            // Åñëè âåðíóëèñü — óíè÷òîæàåì
            if (Vector3.Distance(transform.position, spawnPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator PeckAndReturn()
    {

        // Включаем анимацию клёва
        if (animator != null)
        {
            animator.SetBool("isPecking", true);
        }

        FallingObjectSpawner.Instance.StartSpawning(0.8f);
        // 1. Àíèìàöèÿ êëåâàíèÿ (ñòîèì 3 ñåêóíäû)
        yield return new WaitForSeconds(3f);
        FallingObjectSpawner.Instance.StopSpawning();

        // Выключаем анимацию клёва
        if (animator != null)
        {
            animator.SetBool("isPecking", false);
        }

        // 2. Ïëàâíî îòëåòàåì íàçàä íà 3 åäèíèöû ïî X
        Vector3 startPos = transform.position;
        Vector3 retreatPos = startPos + new Vector3(-3f, 0, 0);
        float retreatDuration = 1f; // Çà ñêîëüêî ñåêóíä îòëåòàåì íàçàä
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

        // 3. Ðàçâîðà÷èâàåì ñïðàéò
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = true;
        }

        // 4. Ëåòèì îáðàòíî ê òî÷êå ñïàâíà
        isReturning = true;
    }
}