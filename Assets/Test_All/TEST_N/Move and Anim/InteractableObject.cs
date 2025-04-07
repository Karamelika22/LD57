using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public void Interact()
    {
        // Реализация специфического взаимодействия
        Debug.Log("Interacted with: " + gameObject.name);
    }
}
