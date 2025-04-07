using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Teleporter : MonoBehaviour
{
    [Header("Из какой зоны срабатывает телепорт")]
    public MovementZone zoneFrom;

    [Header("Куда телепортируем")]
    public MovementZone zoneTo;

    [Header("Точка выхода в зоне назначения")]
    public Transform destination;

    void Reset()
    {
        // Убедимся, что коллайдер — триггер
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnDrawGizmos()
    {
        // Визуализация связи
        if (zoneFrom != null && zoneTo != null && destination != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
            Gizmos.DrawLine(transform.position, destination.position);
        }
    }
}