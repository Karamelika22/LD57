using System.Collections;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [Header("TabManager")]
    [SerializeField] private CanvasGroup[] tabs;
    [SerializeField] private int startingTabIndex = 0;    // Индекс стартовой вкладки

    private int currentActiveIndex;     // Индекс текущей активной вкладки
    private bool isTransitioning;       // Флаг для блокировки переходов во время анимации

    private void Start()
    {
        StartTabs();
    }

    private void StartTabs()
    {
        // Инициализация: все вкладки скрыты, кроме стартовой
        foreach (CanvasGroup tab in tabs)
        {
            tab.alpha = 0;
            tab.interactable = false;
            tab.blocksRaycasts = false;
        }

        if (tabs.Length > startingTabIndex)
        {
            currentActiveIndex = startingTabIndex;
            tabs[startingTabIndex].alpha = 1;
            tabs[startingTabIndex].interactable = true;
            tabs[startingTabIndex].blocksRaycasts = true;
        }
    }

    public void SwitchTab(int newIndex)
    {
        if (!isTransitioning && newIndex != currentActiveIndex)
        {
            StartCoroutine(SmoothTransition(newIndex));
        }
    }

    private IEnumerator SmoothTransition(int newIndex)
    {
        isTransitioning = true;

        CanvasGroup oldTab = tabs[currentActiveIndex];
        CanvasGroup newTab = tabs[newIndex];

        // Начало анимации: старая вкладка исчезает, новая появляется
        float duration = 0.25f;
        float elapsed = 0f;

        // Отключаем взаимодействие с вкладками во время анимации
        oldTab.interactable = false;
        oldTab.blocksRaycasts = false;
        newTab.interactable = false;
        newTab.blocksRaycasts = false;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Плавное изменение прозрачности
            oldTab.alpha = Mathf.Lerp(1, 0, t);
            newTab.alpha = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        // Финализация значений
        oldTab.alpha = 0;
        newTab.alpha = 1;
        newTab.interactable = true;
        newTab.blocksRaycasts = true;

        currentActiveIndex = newIndex;
        isTransitioning = false;
    }

}