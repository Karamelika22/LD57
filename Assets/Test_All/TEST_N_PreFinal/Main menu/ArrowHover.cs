using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowHover : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform pointer;    // Объект, который будет перемещаться
    public Vector2 offset = new(50, 0); // Смещение от кнопки

    [Header("Buttons")]
    public Button[] targetButtons;   // Массив кнопок для отслеживания

    void Start()
    {
        // Назначить события для всех кнопок
        foreach (Button btn in targetButtons)
        {
            AddHoverEvents(btn);
        }

        // Скрыть указатель вначале
        pointer.gameObject.SetActive(false);
    }

    private void AddHoverEvents(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null) trigger = button.gameObject.AddComponent<EventTrigger>();

        // Событие наведения
        EventTrigger.Entry enterEntry = new()
        {
            eventID = EventTriggerType.PointerEnter
        };
        enterEntry.callback.AddListener((data) => OnButtonHover(button));
        trigger.triggers.Add(enterEntry);

        // Событие ухода
        EventTrigger.Entry exitEntry = new()
        {
            eventID = EventTriggerType.PointerExit
        };
        exitEntry.callback.AddListener((data) => OnButtonExit());
        trigger.triggers.Add(exitEntry);
    }

    private void OnButtonHover(Button button)
    {
        // Получаем позицию кнопки
        RectTransform btnRect = button.GetComponent<RectTransform>();

        // Телепортируем объект
        pointer.gameObject.SetActive(true);
        pointer.anchoredPosition = btnRect.anchoredPosition + offset;
    }

    private void OnButtonExit()
    {
        pointer.gameObject.SetActive(false);
    }
}
