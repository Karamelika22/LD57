using UnityEngine;
using UnityEngine.UI;

public class ResourceSystem : MonoBehaviour
{
    [System.Serializable]
    public class Resource
    {
        public string name;
        public float currentAmount;
        public float maxAmount;
        public Image uiFillImage;
    }

    public Resource[] resources = new Resource[4];
    public static ResourceSystem Instance { get; private set; }
    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        // Инициализация UI
        UpdateAllUI();
    }

    // Обновление всех UI элементов
    void UpdateAllUI()
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
    }

    // Добавление ресурса
    public void AddResource(int resourceIndex, float amount)
    {
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
}
