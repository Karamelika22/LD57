using UnityEngine;

public abstract class CraftedItem : MonoBehaviour
{
    public abstract void ActivateEffect();

    // Автоматически активируем эффект при появлении предмета
    private void Start()
    {
        ActivateEffect();
    }
}