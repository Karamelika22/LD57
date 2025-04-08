using UnityEngine;

public class PileCleanup : MonoBehaviour
{
    private FallingObjectSpawner spawner;

    public void Initialize(FallingObjectSpawner spawnerSystem)
    {
        spawner = spawnerSystem;
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.RemovePile(gameObject);
        }
    }
}
