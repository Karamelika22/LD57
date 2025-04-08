using UnityEngine;

public class HealthPotion : CraftedItem
{
    public float healAmount = 33f;

    public override void ActivateEffect()
    {
        Health.Instance.Heal(healAmount);
        Debug.Log($"Зелье здоровья использовано! Восстановлено {healAmount} HP");
        Destroy(gameObject, 0.1f); // Удаляем после использования
    }
}
