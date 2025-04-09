using Unity;
public class RootsPotion : CraftedItem
{
    public float increaseAmount = 33f;

    public override void ActivateEffect()
    {
        WinDefeat.Instance.TryToWin(1);
        Health.Instance.UpdateMaxHealth(increaseAmount);
        Destroy(gameObject, 0.1f); // Удаляем после использования
    }
}