using UnityEngine;

public class ShieldPotion : CraftedItem
{

    public override void ActivateEffect()
    {
        Health.Instance.HandleShield();
        Debug.Log("ыхр!");
        Destroy(gameObject, 0.1f);
    }
}
