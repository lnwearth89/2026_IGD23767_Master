using UnityEngine;

// World pickup: กด E เพื่อเก็บและสวมใส่ทันที (implement IInteractable)
public class EquipmentPickup : Identity, IInteractable
{
    [SerializeField] private EquipmentSO equipmentData;

    public void Use(Player player)
    {
        if (equipmentData == null)
        {
            Debug.LogWarning($"{IdentityName} has no EquipmentSO assigned.");
            return;
        }
        equipmentData.ApplyEffect(player);
        Destroy(gameObject);
    }
}
