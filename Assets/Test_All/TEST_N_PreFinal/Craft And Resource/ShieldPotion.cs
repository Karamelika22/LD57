using UnityEngine;

public class ShieldPotion : CraftedItem
{

    public override void ActivateEffect()
    {
        Health.Instance.HandleShield();
        Debug.Log("���!");
        Destroy(gameObject, 0.1f);
    }
}
