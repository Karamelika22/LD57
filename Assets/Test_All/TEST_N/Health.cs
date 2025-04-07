using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private string healthBarTag = "HealthBar";
    [SerializeField] private float currentHp;
    [SerializeField] private Image healthBar;

    private float regenerationTimer;
    private bool isgametime = false;
    public float currentMaxHp = 100;
    public float currentRegeneration = 0.5f;
    public float currentArmor = 0;

    private void Update()
    {
        if (isgametime) 
        {
            HandleRegeneration();
            Test();
        }
    }

    void Test()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TrueDamage(20);

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Damage(20);

        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(20);
        }
    }

    public void InitializeHealthSystem()
    {
        if (!healthBar) FindHealthBar();
        UpdateHealthBar();
        isgametime = true;
    }

    void FindHealthBar()
    {
        GameObject barObject = GameObject.FindWithTag(healthBarTag);
        if (barObject != null)
        {
            if (!barObject.TryGetComponent<Image>(out healthBar))
                Debug.LogError($"������ � ����� {healthBarTag} �� ����� ���������� Image!");
        }
        else
        {
            Debug.LogError($"�� ������ ������ � ����� {healthBarTag}!");
        }
    }

    public void TrueDamage(float damageAmount)
    {
        if (currentHp <= 0) return;

        currentHp = Mathf.Max(currentHp - damageAmount, 0);
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }
    public void Damage(float damageAmount)
    {
        if (currentHp <= 0) return;
        // ������� ������������ �����
        float armorModifier = 1f - (currentArmor * 0.05f);

        // ����������� ���� - 0% �� ���������, ������������ ��������� ����� - 200%
        armorModifier = Mathf.Clamp(armorModifier, 0f, 2f);

        float finalDamage = damageAmount * armorModifier;

        currentHp = Mathf.Max(currentHp - finalDamage, 0);
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }
    void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.fillAmount = currentHp / currentMaxHp;
    }

    void Die()
    {
        // ������ ������ ���������
        Debug.Log("�������� ����!");
        GetComponent<Animator>().SetTrigger("isdead");
    }

    // ��� �������������� ��������
    public void Heal(float healAmount)
    {
        currentHp = Mathf.Min(currentHp + healAmount, currentMaxHp);
        UpdateHealthBar();
    }

    // ��� ��������� ������������� ��������
    public void UpdateMaxHealth()
    {
        currentHp = Mathf.Min(currentHp, currentMaxHp);
        UpdateHealthBar();
    }

    private void HandleRegeneration()
    {
        if (currentHp >= currentMaxHp || currentRegeneration==0) return;

        regenerationTimer += Time.deltaTime;

        if (regenerationTimer >= 1f)
        {
            Regeneration();
            regenerationTimer = 0f;
        }
    }

    private void Regeneration()
    {
        currentHp = Mathf.Min(currentHp+currentRegeneration, currentMaxHp);
        UpdateHealthBar();
    }
    
}
