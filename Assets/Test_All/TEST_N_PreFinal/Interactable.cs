using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactionType; // "Light", "Mineral", "Stick", "Crafting"
    [SerializeField] private GameObject interactionHint; // Подсказка "Press [F]"
    [Header("Pile Settings")]
    [SerializeField] private bool isPile = false;
    private InteractManager interactManager;
    private bool _isPlayerInRange = false; // Локальный флаг для отслеживания игрока в зоне

    private void Start()
    {
        if (interactManager == null)
        {
            interactManager = InteractManager.Instance;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        interactManager.SetPlayerInRange(true);
        _isPlayerInRange = true; // Устанавливаем флаг для этого объекта
        if(interactionHint!=null) interactionHint.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        interactManager.SetPlayerInRange(false);
        _isPlayerInRange = false; // Сбрасываем флаг
        if (interactionHint != null) interactionHint.SetActive(false);
        if (interactManager != null) interactManager.ForceCloseCraftingUI();
    }

    private void Update()
    {
        // Проверяем локальный флаг вместо глобального
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F) && interactManager != null)
        {
            if (isPile)
            {
                interactManager.HandleInteraction(interactionType);
                StartCoroutine(DestroyPileWithDelay(5.01f));
            }
            else
            {
                interactManager.HandleInteraction(interactionType);
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
