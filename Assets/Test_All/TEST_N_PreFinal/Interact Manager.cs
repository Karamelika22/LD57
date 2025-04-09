using System.Collections;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [Header("Crafting UI")]
    [SerializeField] private CanvasGroup craftingUI; // ������ �� CanvasGroup

    private bool isPlayerInRange = false;
    private bool isGathering = false;
    private Coroutine gatheringCoroutine;
    private string currentGatheringType;
    
    
    // ���������� ��� �������������� � �������� (������� F)
    public static InteractManager Instance { get; private set; }
    private void Awake()
    {
        Instance= this;
    }
    
    public void HandleInteraction(string interactionType)
    {
        if (!isPlayerInRange) return; // ���� ����� ����� �� ������� - ������ �� ������

        switch (interactionType)
        {
            case "Light":
                if (!isGathering || currentGatheringType != "Light")
                {
                    if (isGathering) StopCoroutine(gatheringCoroutine);
                    gatheringCoroutine = StartCoroutine(GatherResourceRoutine("Light", 2, 5, 11, 4f));
                }
                break;
            case "Mine":
                if (!isGathering || currentGatheringType != "Mine")
                {
                    if (isGathering) StopCoroutine(gatheringCoroutine);
                    gatheringCoroutine = StartCoroutine(GatherResourceRoutine("Mine", 0, 2, 5, 1f));
                }
                break;
            case "Plant":
                if (!isGathering || currentGatheringType != "Plant")
                {
                    if (isGathering) StopCoroutine(gatheringCoroutine);
                    
                    gatheringCoroutine = StartCoroutine(GatherResourceRoutine("Plant", 3, 10, 25, 5f));
                }
                break;
            case "Craft":
                ToggleCraftingUI();
                break;
            default:
                Debug.Log("Unknown interaction type: " + interactionType);
                break;
        }
    }

    // ������������� �������� ��� ����� ��������
    private IEnumerator GatherResourceRoutine(string resourceType, int resourceId, int minAmount, int maxAmount, float time)
    {
        isGathering = true;
        currentGatheringType = resourceType;

        // ��������� �������� ����� ������ �������
        yield return new WaitForSeconds(time);

        while (isPlayerInRange)
        {
            int amount = Random.Range(minAmount, maxAmount); // ��������� ����������
            ResourceSystem.Instance.AddResource(resourceId, amount);
            Debug.Log($"�������: {resourceType} ({amount} ��.)");

            yield return new WaitForSeconds(time); // �������� ����� �������
        }

        isGathering = false;
        currentGatheringType = null;
    }

    // ���������/���������� UI ������
    private void ToggleCraftingUI()
    {
        if (craftingUI == null) return;

        bool isUIVisible = craftingUI.alpha > 0;
        craftingUI.alpha = isUIVisible ? 0 : 1;
        craftingUI.interactable = !isUIVisible;
        craftingUI.blocksRaycasts = !isUIVisible;
    }

    // ����� ����� � ����
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

    // ���� ����� ����, ��������� UI
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
