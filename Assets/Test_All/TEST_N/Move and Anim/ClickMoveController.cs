using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ClickMoveController : MonoBehaviour
{
    [Header("��������� ������������")]
    public float speed = 5f;
    
    [Header("��������� ����� �� �������")]
    public float clickRadius = 0.5f; // ������, � ������� ���� ��������� �� �������

    private bool isAwaitingTarget = false;
    private Coroutine moveCoroutine;

    // ��� ���� ��� �� �����
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
    }
    void Update()
    {
        // ��������� ����� �� ������� (���� ��� �� � ������ ������ �����)
        if (!isAwaitingTarget && Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorld.z = transform.position.z;

            // ���������, ����� �� ���� � ������� ������ �������
            if ((clickWorld - transform.position).sqrMagnitude <= clickRadius * clickRadius)
            {
                isAwaitingTarget = true;
                Debug.Log("���� �� ������ � �������� ����� ����������");
                return; // ����� � ��� �� ���� �� ���������� ����� �����
            }
        }

        // ���� ����� ������ ����� ������� � ������� ������ ����
        if (isAwaitingTarget && Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorld.z = transform.position.z;

            // ���������, ����� �� ����� ������ ����� �� ���
            bool insideAny = movementZones.Any(zone =>
            {
                // OverlapPoint �������� ������ � ������ 2D, �.�. z-���������� ������������
                return zone.OverlapPoint((Vector2)clickWorld);
            });

            if (insideAny)
            {
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(MoveTo(clickWorld));
            }
            else
            {
                Debug.Log("����� �� ��������� ���������� ����");
            }

            isAwaitingTarget = false;
        }
    }

    private IEnumerator MoveTo(Vector3 dest)
    {

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

            // �������� �������� ������ ��� �������������
            if (Physics2D.OverlapPoint(newPos, obstacleMask))
            {
                if (playerAnim != null)
                {
                    playerAnim.PlayWalkAnimation(false);
                }
                //isAwaitingTarget = true;
                Debug.Log("���������� �����������!");
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
                Debug.Log("���������� �������� �����");
                yield break;
            }

            yield return null;
        }
    }


    // ��� ����������� ����� � OnDrawGizmos ���������� ���� ������������ � ���� �����:
    void OnDrawGizmosSelected()
    {
        
        // ������ ����� �� �������
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, clickRadius);
    }
}