using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private Transform groundPoint;
    private FallingObjectSpawner spawner;
    private float fallSpeed = 2f;
    private bool hasFallen = false;

    public void Initialize(Transform ground, FallingObjectSpawner spawnerSystem)
    {
        groundPoint = ground;
        spawner = spawnerSystem;
    }

    void Update()
    {
        if (!hasFallen && groundPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, groundPoint.position.y, transform.position.z),
                fallSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.position.y - groundPoint.position.y) < 0.1f)
            {
                hasFallen = true;
                spawner.OnObjectFell();
                Destroy(gameObject, 1f);
            }
        }
    }
}
