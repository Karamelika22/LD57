using System.Collections;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [Header("Crafting UI")]
    [SerializeField] private CanvasGroup craftingUI; // Ññûëêà íà CanvasGroup
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator luchAnimator;

    private bool isPlayerInRange = false;
    private bool isGathering = false;
    private Coroutine gatheringCoroutine;
    private string currentGatheringType;
    
    
    // Âûçûâàåòñÿ ïðè âçàèìîäåéñòâèè ñ îáúåêòîì (íàæàòèè F)
    public static InteractManager Instance { get; private set; }
    private void Awake()
    {
        Instance= this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void HandleInteraction(string interactionType)
    {
        if (!isPlayerInRange) return; // Åñëè èãðîê âûøåë èç ðàäèóñà - íè÷åãî íå äåëàåì

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



    // Óíèâåðñàëüíàÿ êîðóòèíà äëÿ ñáîðà ðåñóðñîâ
    private IEnumerator GatherResourceRoutine(string resourceType, int resourceId, int minAmount, int maxAmount, float time)
    {
        isGathering = true;
        currentGatheringType = resourceType;

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsDig", resourceType == "Mine");
            playerAnimator.SetBool("IsDancing", resourceType == "Light");
            //playerAnimator.SetBool("isGathering", resourceType != "Mine"); // для других случаев, если нужно
        }

        if (luchAnimator != null)
        {
            luchAnimator.SetBool("IsStart", resourceType == "Light");
        }

        // Íà÷àëüíàÿ çàäåðæêà ïåðåä ïåðâîé äîáû÷åé
        yield return new WaitForSeconds(time);

        while (isPlayerInRange)
        {
            int amount = Random.Range(minAmount, maxAmount); // Ñëó÷àéíîå êîëè÷åñòâî
            ResourceSystem.Instance.AddResource(resourceId, amount);
            Debug.Log($"Ñîáðàíî: {resourceType} ({amount} åä.)");

            yield return new WaitForSeconds(time); // Çàäåðæêà ìåæäó äîáû÷åé
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsDig", false);
            //playerAnimator.SetBool("isGathering", false);
        }

        isGathering = false;
        currentGatheringType = null;


    }

    // Âêëþ÷åíèå/âûêëþ÷åíèå UI êðàôòà
    private void ToggleCraftingUI()
    {
        if (craftingUI == null) return;

        bool isUIVisible = craftingUI.alpha > 0;
        craftingUI.alpha = isUIVisible ? 0 : 1;
        craftingUI.interactable = !isUIVisible;
        craftingUI.blocksRaycasts = !isUIVisible;
    }

    // Èãðîê âîøåë â çîíó
    public void SetPlayerInRange(bool state)
    {
        isPlayerInRange = state;
        if (!state && isGathering)
        {
            StopCoroutine(gatheringCoroutine);
            isGathering = false;
            currentGatheringType = null;

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsDig", false);
                playerAnimator.SetBool("IsDancing", false);
                //playerAnimator.SetBool("isGathering", false);
            }

            if (luchAnimator != null)
            {
                luchAnimator.SetBool("IsEnd", true);
            }

        }
    }

    // Åñëè èãðîê óøåë, çàêðûâàåì UI
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
