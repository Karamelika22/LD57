using System.Collections;
using System.Linq;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ClickMoveController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Interaction Settings")]
    public float clickRadius = 0.5f;

    [Header("Sound Settings")]
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
        
        if (!isAwaitingTarget && Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorld.z = transform.position.z;

            
            if ((clickWorld - transform.position).sqrMagnitude <= clickRadius * clickRadius)
            {

                RuntimeManager.PlayOneShot(characterClickSound, transform.position);

                isAwaitingTarget = true;
                Debug.Log("Character selected - awaiting target point");
                return; 
            }
        }

        // Åñëè ðåæèì âûáîðà òî÷êè âêëþ÷¸í — ñëóøàåì âòîðîé êëèê
        if (isAwaitingTarget && Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorld.z = transform.position.z;

            bool insideAny = movementZones.Any(zone =>
            {
                return zone.OverlapPoint((Vector2)clickWorld);
            });

            if (insideAny)
            {
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(MoveTo(clickWorld));
            }
            else
            {
                Debug.Log("Target point is outside allowed movement zones");
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

            if (Physics2D.OverlapPoint(newPos, obstacleMask))
            {

                if (playerAnim != null)
                {
                    playerAnim.PlayWalkAnimation(false);
                }

                //isAwaitingTarget = true;
                StopFootstepsSound();
                Debug.Log("Movement stopped - obstacle detected");
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
                Debug.Log("Movement completed - reached destination");
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


    
    void OnDrawGizmosSelected()
    {
        
        // Ðàäèóñ êëèêà ïî îáúåêòó
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, clickRadius);
    }
}