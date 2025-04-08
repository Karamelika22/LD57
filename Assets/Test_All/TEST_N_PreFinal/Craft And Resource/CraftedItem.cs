using UnityEngine;

public abstract class CraftedItem : MonoBehaviour
{
    public abstract void ActivateEffect();

    // ������������� ���������� ������ ��� ��������� ��������
    private void Start()
    {
        ActivateEffect();
    }
}