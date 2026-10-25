using UnityEngine;
using UnityEngine.UI;

// Singleton: มี instance เดียวในเกม ให้ Player/Enemy/ระบบอื่นเรียกอัปเดต UI ได้จาก GameManager.Instance
// โดยไม่ต้องลาก reference ของ UI ไปส่งต่อกันไปมาระหว่าง object
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Health UI")]
    [SerializeField] private Image healthFillImage; // ตั้ง Image Type = Filled ใน Inspector แล้วปรับ fillAmount (0-1) แทนการใช้ Slider
    [SerializeField] private Text healthText;

    [Header("Exp UI")]
    [SerializeField] private Image expFillImage;
    [SerializeField] private Text expText;

    [Header("Level UI")]
    [SerializeField] private Text levelText;

    [Header("Gold UI")]
    [SerializeField] private Text goldText;

    // ระบบ Currency (Gold) ก็เป็นสถานะรวมของเกมเหมือน Exp/Level: enemy แค่ "ดรอป" ให้ ไม่ได้ถือเงินเป็นของตัวเอง
    private int gold;
    public int Gold => gold;

    // ระบบ Exp/Level ของผู้เล่นย้ายมาอยู่ที่นี่: มันคือสถานะความก้าวหน้ารวมของเกม ไม่ใช่สถานะเฉพาะตัวของ Character แต่ละตัว
    // (enemy ไม่มีเลเวลเป็นของตัวเอง มันแค่ "ให้" exp เวลาตาย) จึงเหมาะกับ Singleton ตัวเดียวที่ทุกคนเรียกเข้ามาได้ มากกว่าผูกไว้กับ Character
    private int experiencePoints;
    private int level = 1;

    public int Level => level;
    public int ExpToNextLevel => level * 100;

    private void Awake()
    {
        // กันไม่ให้มี GameManager ซ้ำสองตัวในซีนเดียวกัน ถ้ามีอยู่แล้วให้ทำลายตัวที่เพิ่งสร้างทิ้ง
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateExpUI(experiencePoints, ExpToNextLevel);
        UpdateLevelUI(level);
        UpdateGoldUI(gold);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Gained {amount} gold. Total: {gold}");
        UpdateGoldUI(gold);
    }

    public void GainExperience(int exp)
    {
        experiencePoints += exp;
        Debug.Log($"Player gained {exp} experience points.");
        CheckLevelUp();
        UpdateExpUI(experiencePoints, ExpToNextLevel);
        UpdateLevelUI(level);
    }

    private void CheckLevelUp()
    {
        while (experiencePoints >= ExpToNextLevel)
        {
            experiencePoints -= ExpToNextLevel;
            level++;
            Debug.Log($"Leveled up to Level {level}!");
        }
    }

    public void UpdateHealthUI(int currentHp, int maxHp)
    {
        if (healthFillImage != null && maxHp > 0)
        {
            healthFillImage.fillAmount = (float)currentHp / maxHp;
        }
        if (healthText != null)
        {
            healthText.text = $"{currentHp} / {maxHp}";
        }
    }

    public void UpdateExpUI(int currentExp, int expToNextLevel)
    {
        if (expFillImage != null && expToNextLevel > 0)
        {
            expFillImage.fillAmount = (float)currentExp / expToNextLevel;
        }
        if (expText != null)
        {
            expText.text = $"{currentExp} / {expToNextLevel}";
        }
    }

    public void UpdateLevelUI(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Lv. {level}";
        }
    }

    public void UpdateGoldUI(int gold)
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {gold}";
        }
    }
}
