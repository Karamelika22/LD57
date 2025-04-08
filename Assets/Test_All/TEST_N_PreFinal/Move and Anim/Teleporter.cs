using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Teleporter : MonoBehaviour
{
    [Header("�� ����� ���� ����������� ��������")]
    public MovementZone zoneFrom;

    [Header("���� �������������")]
    public MovementZone zoneTo;

    [Header("����� ������ � ���� ����������")]
    public Transform destination;

    void Reset()
    {
        // ��������, ��� ��������� � �������
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnDrawGizmos()
    {
        // ������������ �����
        if (zoneFrom != null && zoneTo != null && destination != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
            Gizmos.DrawLine(transform.position, destination.position);
        }
    }
}