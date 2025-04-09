using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ResourceSystem : MonoBehaviour
{
    [System.Serializable]
    public class Resource
    {
        public string name;
        public float currentAmount;
        public float maxAmount;
        public float previousAmount; // Для отслеживания изменений
        public Image uiFillImage;
    }

    public Resource[] resources = new Resource[4];
    public TextMeshProUGUI[] text = new TextMeshProUGUI[4];
    private bool enableColorEffect = true; // Флаг для включения/выключения эффекта

    private Coroutine[] colorResetCoroutines;
    public static ResourceSystem Instance { get; private set; }
    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        // Инициализация UI
        colorResetCoroutines = new Coroutine[text.Length];
        InitializeResources();
        UpdateAllUI();
    }
    private void InitializeResources()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i] == null)
            {
                resources[i] = new Resource();
            }

            if (i == 1)
            {
                // Для ресурса с индексом 1 всегда 100
                resources[i].currentAmount = 100f;
                resources[i].maxAmount = 100f;
                float roundedValue = Mathf.Round(resources[i].currentAmount * 100f) / 100f;
                text[i].text = $"{roundedValue}";
            }
            else
            {
                // Для остальных ресурсов случайное значение от 30 до 50
                resources[i].currentAmount = Random.Range(30f, 50f);
                resources[i].maxAmount = 100f;
                float roundedValue = Mathf.Round(resources[i].currentAmount * 100f) / 100f;
                text[i].text = $"{roundedValue}";
            }

            // Инициализация имени, если не задано
            if (string.IsNullOrEmpty(resources[i].name))
            {
                resources[i].name = $"Resource_{i}";
            }
        }
    }

    // Обновление всех UI элементов
    public void UpdateAllUI()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            UpdateUI(i);
        }
    }

    // Обновление конкретного UI элемента
    void UpdateUI(int resourceIndex)
    {
        if (resources[resourceIndex].uiFillImage != null)
        {
            resources[resourceIndex].uiFillImage.fillAmount =
                resources[resourceIndex].currentAmount / resources[resourceIndex].maxAmount;
        }
        if (text[resourceIndex] != null)
        {
            float roundedValue = Mathf.Round(resources[resourceIndex].currentAmount * 100f) / 100f;
            text[resourceIndex].text = $"{roundedValue}";
        }

        CheckForColorEffect(resourceIndex);
    }

    // Добавление ресурса
    public void AddResource(int resourceIndex, float amount)
    {
        // Сохраняем предыдущее значение перед изменением
        resources[resourceIndex].previousAmount = resources[resourceIndex].currentAmount;

        resources[resourceIndex].currentAmount = Mathf.Clamp(
            resources[resourceIndex].currentAmount + amount,
            0,
            resources[resourceIndex].maxAmount);
        UpdateUI(resourceIndex);

    }

    // Установка значения ресурса
    public void SetResource(int resourceIndex, float amount)
    {
        resources[resourceIndex].currentAmount = Mathf.Clamp(
            amount,
            0,
            resources[resourceIndex].maxAmount);
        UpdateUI(resourceIndex);
    }

    // Проверка, достаточно ли ресурса
    public bool HasEnough(int resourceIndex, float amount)
    {
        return resources[resourceIndex].currentAmount >= amount;
    }

    // Потребление ресурса
    public bool Consume(int resourceIndex, float amount)
    {
        if (HasEnough(resourceIndex, amount))
        {
            AddResource(resourceIndex, -amount);
            return true;
        }
        return false;
    }
    void CheckForColorEffect(int resourceIndex)
    {
        if (!enableColorEffect || text[resourceIndex] == null) return;

        // Если количество увеличилось
        if (resources[resourceIndex].currentAmount > resources[resourceIndex].previousAmount)
        {
            // Останавливаем предыдущую корутину
            if (colorResetCoroutines[resourceIndex] != null)
            {
                StopCoroutine(colorResetCoroutines[resourceIndex]);
            }

            // Устанавливаем зеленый цвет
            text[resourceIndex].color = Color.green;

            // Запускаем корутину для сброса цвета
            colorResetCoroutines[resourceIndex] = StartCoroutine(ResetTextColor(resourceIndex));
        }
    }
    private IEnumerator ResetTextColor(int index)
    {
        yield return new WaitForSeconds(2f);
        text[index].color = Color.white;
        colorResetCoroutines[index] = null;
    }
}
