using System.Collections;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [Header("Crafting UI")]
    [SerializeField] private CanvasGroup craftingUI; // Ссылка на CanvasGroup

    private bool isPlayerInRange = false;
    private bool isGathering = false;
    private Coroutine gatheringCoroutine;
    private string currentGatheringType;
    
    
    // Вызывается при взаимодействии с объектом (нажатии F)
    public static InteractManager Instance { get; private set; }
    private void Awake()
    {
        Instance= this;
    }
    
    public void HandleInteraction(string interactionType)
    {
        if (!isPlayerInRange) return; // Если игрок вышел из радиуса - ничего не делаем

        switch (interactionType)
        {
            case "Light":
                if (!isGathering || currentGatheringType != "Light")
                {
                    if (isGathering) StopCoroutine(gatheringCoroutine);
                    gatheringCoroutine = StartCoroutine(GatherResourceRoutine("Light", 2, 3, 8));
                }
                break;
            case "Mineral":
                if (!isGathering || currentGatheringType != "Mineral")
                {
                    if (isGathering) StopCoroutine(gatheringCoroutine);
                    gatheringCoroutine = StartCoroutine(GatherResourceRoutine("Mineral", 0, 1, 3));
                }
                break;
            case "Plant":
                if (!isGathering || currentGatheringType != "Plant")
                {
                    if (isGathering) StopCoroutine(gatheringCoroutine);
                    
                    gatheringCoroutine = StartCoroutine(GatherResourceRoutine("Plant", 3, 10, 25));
                }
                break;
            case "Crafting":
                ToggleCraftingUI();
                break;
            default:
                Debug.Log("Unknown interaction type: " + interactionType);
                break;
        }
    }

    // Универсальная корутина для сбора ресурсов
    private IEnumerator GatherResourceRoutine(string resourceType, int resourceId, int minAmount, int maxAmount)
    {
        isGathering = true;
        currentGatheringType = resourceType;

        // Начальная задержка перед первой добычей
        yield return new WaitForSeconds(5f);

        while (isPlayerInRange)
        {
            int amount = Random.Range(minAmount, maxAmount); // Случайное количество
            ResourceSystem.Instance.AddResource(resourceId, amount);
            Debug.Log($"Собрано: {resourceType} ({amount} ед.)");

            yield return new WaitForSeconds(5f); // Задержка между добычей
        }

        isGathering = false;
        currentGatheringType = null;
    }

    // Включение/выключение UI крафта
    private void ToggleCraftingUI()
    {
        if (craftingUI == null) return;

        bool isUIVisible = craftingUI.alpha > 0;
        craftingUI.alpha = isUIVisible ? 0 : 1;
        craftingUI.interactable = !isUIVisible;
        craftingUI.blocksRaycasts = !isUIVisible;
    }

    // Игрок вошел в зону
    public void SetPlayerInRange(bool state)
    {
        isPlayerInRange = state;
        if (!state && isGathering)
        {
            StopCoroutine(gatheringCoroutine);
            isGathering = false;
            currentGatheringType = null;
        }
    }

    // Если игрок ушел, закрываем UI
    public void ForceCloseCraftingUI()
    {
        if (craftingUI != null && craftingUI.alpha > 0)
        {
            craftingUI.alpha = 0;
            craftingUI.interactable = false;
            craftingUI.blocksRaycasts = false;
        }
    }
   
}
