using UnityEngine;

public class WinDefeat : MonoBehaviour
{
    public static WinDefeat Instance { get; private set; }
    public CanvasGroup defeatGroup;
    public CanvasGroup winGroup;
    private int rootLevel;
    private PauseManager pause;
    public Interactable[] interactables;
    public GameObject[] levelImages; // Добавляем массив картинок по уровням
    void Awake()
    {
        Instance= this;
        pause= GetComponent<PauseManager>();
    }

    public void Defeat()
    {
        if (defeatGroup == null) return;
        pause.Pause();
        TriggerEndGameForAll();
        bool isUIVisible = defeatGroup.alpha > 0;
        defeatGroup.alpha = isUIVisible ? 0 : 1;
        defeatGroup.interactable = !isUIVisible;
        
    }

    public void TryToWin(int i)
    {
        rootLevel += i;
        UpdateLevelImage(); // Обновляем изображение при изменении уровня
        if (rootLevel == 3) { Win(); }
    }
    void Win()
    {
        if (winGroup == null) return;
        pause.Pause();
        TriggerEndGameForAll();
        bool isUIVisible = winGroup.alpha > 0;
        winGroup.alpha = isUIVisible ? 0 : 1;
        winGroup.interactable = !isUIVisible;
    }
    public void TriggerEndGameForAll()
    {
        foreach (Interactable interactable in interactables)
        {
            if (interactable != null)
            {
                interactable.EndGame();
            }
        }
    }

    void UpdateLevelImage()
    {

        // Включаем только текущее изображение уровня, если оно есть
        if (rootLevel >= 0 && rootLevel < levelImages.Length)
        {
            if (levelImages[rootLevel] != null)
                levelImages[rootLevel].SetActive(true);
        }
    }
}