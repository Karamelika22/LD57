using System.Resources;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [System.Serializable]
    public class CraftRecipe
    {
        public string itemName;
        public int[] requiredResources; // Индексы ресурсов
        public float[] requiredAmounts;  // Количество каждого ресурса
        public GameObject resultPrefab;  // Префаб создаваемого предмета
    }
    public Transform itemSpawnPoint; // Точка создания предметов
    public CraftRecipe[] recipes = new CraftRecipe[4]; // 4 рецепта крафта
    public static CraftingSystem Instance { get; private set; }
    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // Методы для кнопок
    public void CraftRecipe1() { TryCraftWithUI(0); }
    public void CraftRecipe2() { TryCraftWithUI(1); }
    public void CraftRecipe3() { TryCraftWithUI(2); }
    public void CraftRecipe4() { TryCraftWithUI(3); }

    private void TryCraftWithUI(int recipeIndex)
    {
        if (TryCraft(recipeIndex, itemSpawnPoint))
        {
            Debug.LogWarning(recipes[recipeIndex].itemName + " успешно создан!");
            // Здесь можно добавить звук успешного крафта
        }
        else
        {
            Debug.LogWarning("Не хватает ресурсов для " + recipes[recipeIndex].itemName);
            // Здесь можно добавить звук ошибки
        }
    }
    // Проверка, можно ли скрафтить предмет
    public bool CanCraft(int recipeIndex)
    {
        if (recipeIndex < 0 || recipeIndex >= recipes.Length) return false;

        CraftRecipe recipe = recipes[recipeIndex];

        for (int i = 0; i < recipe.requiredResources.Length; i++)
        {
            if (!ResourceSystem.Instance.HasEnough(recipe.requiredResources[i], recipe.requiredAmounts[i]))
            {
                return false;
            }
        }
        return true;
    }

    // Попытка скрафтить предмет
    public bool TryCraft(int recipeIndex, Transform spawnPoint = null)
    {
        if (!CanCraft(recipeIndex)) return false;

        CraftRecipe recipe = recipes[recipeIndex];

        // Потребляем ресурсы
        for (int i = 0; i < recipe.requiredResources.Length; i++)
        {
            ResourceSystem.Instance.Consume(recipe.requiredResources[i], recipe.requiredAmounts[i]);
        }

        // Создаем предмет
        if (recipe.resultPrefab != null)
        {
            Instantiate(recipe.resultPrefab, spawnPoint ? spawnPoint.position : Vector3.zero,
                       spawnPoint ? spawnPoint.rotation : Quaternion.identity);
        }

        return true;
    }

    // UI метод для отображения доступных рецептов
    public void UpdateCraftingUI()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            // Здесь можно обновлять UI, например, менять цвет кнопок крафта
            // в зависимости от CanCraft(i)
        }
    }
}