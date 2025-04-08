using System.Collections;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    [Header("TabManager")]
    [SerializeField] private CanvasGroup[] tabs;
    [SerializeField] private int startingTabIndex = 0;    // ������ ��������� �������

    private int currentActiveIndex;     // ������ ������� �������� �������
    private bool isTransitioning;       // ���� ��� ���������� ��������� �� ����� ��������

    private void Start()
    {
        StartTabs();
    }

    private void StartTabs()
    {
        // �������������: ��� ������� ������, ����� ���������
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

        // ������ ��������: ������ ������� ��������, ����� ����������
        float duration = 0.25f;
        float elapsed = 0f;

        // ��������� �������������� � ��������� �� ����� ��������
        oldTab.interactable = false;
        oldTab.blocksRaycasts = false;
        newTab.interactable = false;
        newTab.blocksRaycasts = false;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // ������� ��������� ������������
            oldTab.alpha = Mathf.Lerp(1, 0, t);
            newTab.alpha = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        // ����������� ��������
        oldTab.alpha = 0;
        newTab.alpha = 1;
        newTab.interactable = true;
        newTab.blocksRaycasts = true;

        currentActiveIndex = newIndex;
        isTransitioning = false;
    }

}