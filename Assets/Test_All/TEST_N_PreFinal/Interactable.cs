using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactionType; // "Light", "Mineral", "Stick", "Crafting"
    [SerializeField] private GameObject interactionHint; // Подсказка "Press [F]"
    [Header("Pile Settings")]
    [SerializeField] private bool isPile = false;
    
    private bool _isPlayerInRange = false; // Локальный флаг для отслеживания игрока в зоне
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        InteractManager.Instance.SetPlayerInRange(true);
        _isPlayerInRange = true; // Устанавливаем флаг для этого объекта
        if(interactionHint!=null) interactionHint.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        InteractManager.Instance.SetPlayerInRange(false);
        _isPlayerInRange = false; // Сбрасываем флаг
        if (interactionHint != null) interactionHint.SetActive(false);
        if (InteractManager.Instance != null) InteractManager.Instance.ForceCloseCraftingUI();
    }

    private void Update()
    {
        // Проверяем локальный флаг вместо глобального
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F) && InteractManager.Instance != null)
        {
            if (isPile)
            {
                InteractManager.Instance.HandleInteraction(interactionType);
                StartCoroutine(DestroyPileWithDelay(5.01f));
            }
            else
            {
                InteractManager.Instance.HandleInteraction(interactionType);
            }
        }
    }
    private IEnumerator DestroyPileWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    public void EndGame()
    {
        interactionHint.SetActive(false);
    } 

}
