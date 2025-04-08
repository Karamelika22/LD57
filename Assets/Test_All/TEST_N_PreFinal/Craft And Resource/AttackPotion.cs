using UnityEngine;

public class AttackPotion : CraftedItem
{
    public override void ActivateEffect()
    {
        // ����� ���� ������ � ����� "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // �������� ��������� EnemyAI
            
            if (enemy.TryGetComponent<EnemyAI>(out var enemyAI))
            {
                // ������� ����
                enemyAI.TakeDamage(100);
                
            }
        }

        Debug.Log("������������ ��������� �����!");
    }
}