using System.Collections;
using System.Linq;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ClickMoveController : MonoBehaviour
{
    [Header("Íàñòðîéêè ïåðåäâèæåíèÿ")]
    public float speed = 5f;
    
    [Header("Íàñòðîéêè êëèêà ïî îáúåêòó")]
    public float clickRadius = 0.5f; // ðàäèóñ, â êîòîðîì êëèê ñ÷èòàåòñÿ ïî îáúåêòó

    [Header("Настройки звука")]
    [SerializeField] private EventReference footstepsSound; // Ссылка на событие FMOD
    [SerializeField] private EventReference characterClickSound;
    private FMOD.Studio.EventInstance footstepsInstance;

    private bool isMoving = false; // Флаг движения для управления звуком

    private bool isAwaitingTarget = false;
    private Coroutine moveCoroutine;

    // Êýø âñåõ çîí íà ñöåíå
    [SerializeField] private PolygonCollider2D[] movementZones;

    private Rigidbody2D rb;
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private LayerMask obstacleMask;

    private PlayerAnim playerAnim;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        playerAnim = GetComponent<PlayerAnim>();

        footstepsInstance = RuntimeManager.CreateInstance(footstepsSound);
        footstepsInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }
    void Update()
    {
        // Îáðàáîòêà êëèêà ïî îáúåêòó (åñëè åù¸ íå â ðåæèìå âûáîðà òî÷êè)
        if (!isAwaitingTarget && Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorld.z = transform.position.z;

            // Ïðîâåðÿåì, ïîïàë ëè êëèê â îáëàñòü âîêðóã îáúåêòà
            if ((clickWorld - transform.position).sqrMagnitude <= clickRadius * clickRadius)
            {

                RuntimeManager.PlayOneShot(characterClickSound, transform.position);

                isAwaitingTarget = true;
                Debug.Log("Êëèê ïî èãðîêó — âûáåðèòå òî÷êó íàçíà÷åíèÿ");
                return; // ÷òîáû â òîò æå êàäð íå îáðàáîòàòü âûáîð òî÷êè
            }
        }

        // Åñëè ðåæèì âûáîðà òî÷êè âêëþ÷¸í — ñëóøàåì âòîðîé êëèê
        if (isAwaitingTarget && Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorld.z = transform.position.z;

            // Ïðîâåðÿåì, ëåæèò ëè òî÷êà âíóòðè ëþáîé èç çîí
            bool insideAny = movementZones.Any(zone =>
            {
                // OverlapPoint ðàáîòàåò òîëüêî â ðåæèìå 2D, ò.å. z-êîîðäèíàòà èãíîðèðóåòñÿ
                return zone.OverlapPoint((Vector2)clickWorld);
            });

            if (insideAny)
            {
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(MoveTo(clickWorld));
            }
            else
            {
                Debug.Log("Òî÷êà çà ïðåäåëàìè äîïóñòèìîé çîíû");
            }

            isAwaitingTarget = false;
        }
    }

    private IEnumerator MoveTo(Vector3 dest)
    {
        // Начинаем звук шагов
        StartFootstepsSound();

        if (playerAnim != null)
        {
            playerAnim.PlayWalkAnimation(true);
        }

        while (true)
        {

            Vector3 newPos = Vector3.MoveTowards(
                transform.position,
                dest,
                speed * Time.deltaTime
            );

            // Ïðîâåðêà êîëëèçèé òîëüêî ïðè íåîáõîäèìîñòè
            if (Physics2D.OverlapPoint(newPos, obstacleMask))
            {

                if (playerAnim != null)
                {
                    playerAnim.PlayWalkAnimation(false);
                }

                //isAwaitingTarget = true;
                StopFootstepsSound();
                Debug.Log("Îáíàðóæåíî ïðåïÿòñòâèå!");
                yield break;
            }

            transform.position = newPos;

            if ((transform.position - dest).sqrMagnitude < stoppingDistance)
            {
                transform.position = dest;
                if (playerAnim != null)
                {
                    playerAnim.PlayWalkAnimation(false);
                }

                StopFootstepsSound();
                Debug.Log("Äîñòèãíóòà êîíå÷íàÿ òî÷êà");
                yield break;
            }

            yield return null;
        }
    }

    // Новые методы для управления звуком шагов
    private void StartFootstepsSound()
    {
        if (!isMoving)
        {
            footstepsInstance.start();
            isMoving = true;
        }
    }

    private void StopFootstepsSound()
    {
        if (isMoving)
        {
            footstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isMoving = false;
        }
    }

    void OnDestroy()
    {
        // Освобождаем ресурсы FMOD при уничтожении объекта
        footstepsInstance.release();
    }


    // Äëÿ íàãëÿäíîñòè ìîæíî â OnDrawGizmos îòðèñîâàòü çîíó ïåðåäâèæåíèÿ è çîíó êëèêà:
    void OnDrawGizmosSelected()
    {
        
        // Ðàäèóñ êëèêà ïî îáúåêòó
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, clickRadius);
    }
}