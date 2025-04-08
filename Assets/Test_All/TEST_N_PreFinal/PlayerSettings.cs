using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public bool isHandleMovement = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void ToggleMovement(bool enable)
    {
        isHandleMovement = enable;
    }
}