using UnityEngine;

public class AttackPotion : CraftedItem
{
    public override void ActivateEffect()
    {
        // Найти всех врагов с тегом "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Получить компонент EnemyAI
            
            if (enemy.TryGetComponent<EnemyAI>(out var enemyAI))
            {
                // Нанести урон
                enemyAI.TakeDamage(100);
                
            }
        }

        Debug.Log("Активировано атакующее зелье!");
    }
}