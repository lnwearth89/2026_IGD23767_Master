using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Item System/Potion")]
public class PotionSO : ItemSO
{
    [Header("Potion Settings")]
    public int healAmount = 20;

    public override void ApplyEffect(Player player)
    {
        player.Heal(healAmount);
        Debug.Log($"{player.IdentityName} used {itemName} and recovered {healAmount} HP.");
    }
}
