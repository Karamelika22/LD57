using System.Resources;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [System.Serializable]
    public class CraftRecipe
    {
        public string itemName;
        public int[] requiredResources; // ������� ��������
        public float[] requiredAmounts;  // ���������� ������� �������
        public GameObject resultPrefab;  // ������ ������������ ��������
    }
    public Transform itemSpawnPoint; // ����� �������� ���������
    public CraftRecipe[] recipes = new CraftRecipe[4]; // 4 ������� ������
    public static CraftingSystem Instance { get; private set; }
    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // ������ ��� ������
    public void CraftRecipe1() { TryCraftWithUI(0); }
    public void CraftRecipe2() { TryCraftWithUI(1); }
    public void CraftRecipe3() { TryCraftWithUI(2); }
    public void CraftRecipe4() { TryCraftWithUI(3); }

    private void TryCraftWithUI(int recipeIndex)
    {
        if (TryCraft(recipeIndex, itemSpawnPoint))
        {
            Debug.LogWarning(recipes[recipeIndex].itemName + " ������� ������!");
            // ����� ����� �������� ���� ��������� ������
        }
        else
        {
            Debug.LogWarning("�� ������� �������� ��� " + recipes[recipeIndex].itemName);
            // ����� ����� �������� ���� ������
        }
    }
    // ��������, ����� �� ��������� �������
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

    // ������� ��������� �������
    public bool TryCraft(int recipeIndex, Transform spawnPoint = null)
    {
        if (!CanCraft(recipeIndex)) return false;

        CraftRecipe recipe = recipes[recipeIndex];

        // ���������� �������
        for (int i = 0; i < recipe.requiredResources.Length; i++)
        {
            ResourceSystem.Instance.Consume(recipe.requiredResources[i], recipe.requiredAmounts[i]);
        }

        // ������� �������
        if (recipe.resultPrefab != null)
        {
            Instantiate(recipe.resultPrefab, spawnPoint ? spawnPoint.position : Vector3.zero,
                       spawnPoint ? spawnPoint.rotation : Quaternion.identity);
        }

        return true;
    }

    // UI ����� ��� ����������� ��������� ��������
    public void UpdateCraftingUI()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            // ����� ����� ��������� UI, ��������, ������ ���� ������ ������
            // � ����������� �� CanCraft(i)
        }
    }
}