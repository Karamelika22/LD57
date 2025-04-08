using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class MovementZone : MonoBehaviour
{
    // Просто маркер на объекте. Collider задаём в инспекторе (Is Trigger = false).
    public string zoneName;
    void OnDrawGizmos()
    {
        var poly = GetComponent<PolygonCollider2D>();
        for (int i = 0; i < poly.pathCount; i++)
        {
            var pts = poly.GetPath(i);
            for (int j = 0; j < pts.Length; j++)
            {
                Vector3 a = transform.TransformPoint(pts[j]);
                Vector3 b = transform.TransformPoint(pts[(j + 1) % pts.Length]);
                Gizmos.DrawLine(a, b);
            }
        }
    }
}