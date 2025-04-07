using UnityEngine;
using System.Collections;

public class AdvancedTabManager : MonoBehaviour
{
    public CanvasGroup mainMenuPanel; // ������ �������� ����
    public CanvasGroup[] tabs;        // ������ �������
    public float transitionDuration = 0.5f;

    private int currentActiveIndex = -1; // -1 = ���� �������
    private bool isTransitioning;

    void Start()
    {
        // �������������: �� ������, ����� �������� ����
        SetPanelState(mainMenuPanel, true);

        foreach (CanvasGroup tab in tabs)
        {
            SetPanelState(tab, false);
        }
    }

    void Update()
    {
        // ��������� ������ "�����" (Esc)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            if (currentActiveIndex == -1)
            {
                StartCoroutine(TransitionRoutine(mainMenuPanel, tabs[0], 0));
            }
            else
            {
                // ���� ������� �����-���� �������, ��������� � (������� � �������� ����)
                CloseCurrentTab();
            }
        }
    }

    public void ToggleTab(int targetIndex)
    {
        if (isTransitioning) return;

        if (currentActiveIndex == targetIndex)
        {
            CloseCurrentTab();
        }
        else
        {
            if (currentActiveIndex == -1)
            {
                // ��������� ����� ������� �� �������� ����
                StartCoroutine(TransitionRoutine(mainMenuPanel, tabs[targetIndex], targetIndex));
            }
            else
            {
                // ����������� ����� ���������
                StartCoroutine(TransitionRoutine(tabs[currentActiveIndex], tabs[targetIndex], targetIndex));
            }
        }
    }

    public void CloseCurrentTab()
    {
        if (isTransitioning || currentActiveIndex == -1) return;

        StartCoroutine(TransitionRoutine(tabs[currentActiveIndex], mainMenuPanel, -1));
    }

    private IEnumerator TransitionRoutine(CanvasGroup from, CanvasGroup to, int newIndex)
    {
        isTransitioning = true;

        // ��������� ��������
        SetPanelInteractive(from, false);
        SetPanelInteractive(to, false);

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            if (from != null) from.alpha = Mathf.Lerp(1, 0, t);
            to.alpha = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        // �����������
        if (from != null) SetPanelState(from, false);
        SetPanelState(to, true);

        currentActiveIndex = newIndex;
        isTransitioning = false;
    }

    private void SetPanelState(CanvasGroup panel, bool state)
    {
        panel.alpha = state ? 1 : 0;
        panel.interactable = state;
        panel.blocksRaycasts = state;
    }

    private void SetPanelInteractive(CanvasGroup panel, bool interactive)
    {
        panel.interactable = interactive;
        panel.blocksRaycasts = interactive;
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}