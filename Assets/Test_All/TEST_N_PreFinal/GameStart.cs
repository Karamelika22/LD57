using UnityEngine;


public class GameStart : MonoBehaviour
{
    public Health health;
    private void Start()
    {
        InitializePlayerComponents();
    }

    void InitializePlayerComponents()
    {

        health.InitializeHealthSystem();
    }
}
