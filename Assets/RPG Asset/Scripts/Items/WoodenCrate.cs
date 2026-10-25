using System.Collections.Generic;
using UnityEngine;

// กล่องไม้: ตีให้แตกได้ (IDamageable) และกด E เปิดได้ (IInteractable) แตกแล้วสุ่มของจาก possibleDrops
public class WoodenCrate : Identity, IDamageable, IInteractable
{
    [Header("Durability")]
    [SerializeField] private int maxHp = 30;
    private int hp;

    [Header("Drops")]
    [SerializeField] private List<GameObject> possibleDrops = new List<GameObject>(); // prefab ของ pickup ที่อาจตกออกมา
    [SerializeField, Min(0)] private int minDrops = 1;
    [SerializeField, Min(0)] private int maxDrops = 2;
    [SerializeField] private float scatterRadius = 0.5f;

    private bool isBroken;

    private void Awake()
    {
        hp = maxHp;
    }

    public void TakeDamage(int damage, Character attacker)
    {
        if (isBroken) return;

        hp = Mathf.Max(hp - damage, 0);
        Debug.Log($"{IdentityName} took {damage} damage. Remaining HP: {hp}");
        if (hp <= 0)
        {
            Break();
        }
    }

    public void Use(Player player)
    {
        Debug.Log($"{player.IdentityName} opened {IdentityName}.");
        Break();
    }

    private void Break()
    {
        if (isBroken) return;

        isBroken = true;
        DropItems();
        Destroy(gameObject);
    }

    private void DropItems()
    {
        if (possibleDrops.Count == 0) return;

        int count = Random.Range(minDrops, maxDrops + 1);
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = possibleDrops[Random.Range(0, possibleDrops.Count)];
            if (prefab == null) continue;

            Vector2 offset = Random.insideUnitCircle * scatterRadius;
            Vector3 position = transform.position + new Vector3(offset.x, 0.5f, offset.y);
            Instantiate(prefab, position, Quaternion.identity);
            Debug.Log($"{IdentityName} dropped {prefab.name}.");
        }
    }
}
