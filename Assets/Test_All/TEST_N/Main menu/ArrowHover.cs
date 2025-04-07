using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowHover : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform pointer;    // ������, ������� ����� ������������
    public Vector2 offset = new(50, 0); // �������� �� ������

    [Header("Buttons")]
    public Button[] targetButtons;   // ������ ������ ��� ������������

    void Start()
    {
        // ��������� ������� ��� ���� ������
        foreach (Button btn in targetButtons)
        {
            AddHoverEvents(btn);
        }

        // ������ ��������� �������
        pointer.gameObject.SetActive(false);
    }

    private void AddHoverEvents(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null) trigger = button.gameObject.AddComponent<EventTrigger>();

        // ������� ���������
        EventTrigger.Entry enterEntry = new()
        {
            eventID = EventTriggerType.PointerEnter
        };
        enterEntry.callback.AddListener((data) => OnButtonHover(button));
        trigger.triggers.Add(enterEntry);

        // ������� �����
        EventTrigger.Entry exitEntry = new()
        {
            eventID = EventTriggerType.PointerExit
        };
        exitEntry.callback.AddListener((data) => OnButtonExit());
        trigger.triggers.Add(exitEntry);
    }

    private void OnButtonHover(Button button)
    {
        // �������� ������� ������
        RectTransform btnRect = button.GetComponent<RectTransform>();

        // ������������� ������
        pointer.gameObject.SetActive(true);
        pointer.anchoredPosition = btnRect.anchoredPosition + offset;
    }

    private void OnButtonExit()
    {
        pointer.gameObject.SetActive(false);
    }
}
