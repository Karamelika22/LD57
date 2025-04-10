using UnityEngine;

public class Tip : MonoBehaviour
{
    public GameObject interactionHint; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        if (interactionHint != null) interactionHint.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (interactionHint != null) interactionHint.SetActive(false);
    }
}
