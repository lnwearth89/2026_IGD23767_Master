using UnityEngine;

public class Player : Character
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactableLayer;

    private CharacterController controller;
    private Animator animator;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 1.5f;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Equipment")]
    [SerializeField] private Transform weaponSocket; // จุดยึดที่มือขวา (child ของ bone มือขวา)
    [SerializeField] private Transform shieldSocket; // จุดยึดที่มือซ้าย (child ของ bone มือซ้าย)
    [SerializeField] private EquipmentSO equippedWeapon;
    [SerializeField] private EquipmentSO equippedShield;
    private GameObject equippedWeaponModel;
    private GameObject equippedShieldModel;

    protected override void Start()
    {
        base.Start();
        Debug.Log($"{IdentityName} has entered the game!");
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // ตั้งค่า UI เลือดให้ตรงกับสถานะเริ่มต้นของ Player ตั้งแต่ตอนเข้าเกม (exp/level เริ่มต้นถูกตั้งค่าโดย GameManager เอง)
        GameManager.Instance?.UpdateHealthUI(Hp, MaxHp);
    }
    void Update()
    {
        Vector3 currentVelocity = velocity;
        currentVelocity.y += gravity * Time.deltaTime;
        velocity = currentVelocity;
        controller.Move(velocity * Time.deltaTime);

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            currentVelocity = velocity;
            currentVelocity.y = -2f;
            velocity = currentVelocity;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        Movement(moveDirection);
        Turn(moveDirection);
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                Debug.Log("Cannot jump: CharacterController is not grounded.");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction();
        }
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.up * 0.8f, transform.forward, out hit, attackRange))
            {
                IDamageable target = hit.collider.GetComponent<IDamageable>();
                if (target != null)
                {
                    animator.SetTrigger("Attack");
                    Attack(target);
                }
            }
        }
    }
    public void Movement(Vector3 moveDirection)
    {
        controller.Move(moveDirection * movementSpeed * Time.deltaTime);
        animator.SetFloat("MoveSpeed", moveDirection.magnitude);
    }
    public void Turn(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public void Jump()
    {
        Vector3 currentVelocity = velocity;
        currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity = currentVelocity;
        animator.SetTrigger("Jump");
    }
    void Interaction()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.8f, transform.forward, out hit, interactRange, interactableLayer))
        {
            Debug.Log("Interacted with: " + hit.collider.name);
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Use(this);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + transform.up * 0.8f, transform.forward * interactRange);
    }

    public void Equip(EquipmentSO equipment)
    {
        if (equipment == null) return;

        if (equipment.slot == EquipSlot.Weapon)
        {
            if (equippedWeapon != null)
            {
                // ถอด bonus ของเดิมออกก่อน ป้องกันสวมทับแล้วค่าพลังบวกซ้อนกันไม่รู้จบ
                attackPoints -= equippedWeapon.bonusAttack;
                defensePoints -= equippedWeapon.bonusDefense;
            }
            if (equippedWeaponModel != null)
            {
                Destroy(equippedWeaponModel);
            }

            equippedWeapon = equipment;
            equippedWeaponModel = SpawnModel(equipment.modelPrefab, weaponSocket);
        }
        else
        {
            if (equippedShield != null)
            {
                attackPoints -= equippedShield.bonusAttack;
                defensePoints -= equippedShield.bonusDefense;
            }
            if (equippedShieldModel != null)
            {
                Destroy(equippedShieldModel);
            }

            equippedShield = equipment;
            equippedShieldModel = SpawnModel(equipment.modelPrefab, shieldSocket);
        }

        attackPoints += equipment.bonusAttack;
        defensePoints += equipment.bonusDefense;

        Debug.Log($"{IdentityName} equipped {equipment.itemName} (+{equipment.bonusAttack} ATK, +{equipment.bonusDefense} DEF).");
    }

    private GameObject SpawnModel(GameObject modelPrefab, Transform socket)
    {
        if (modelPrefab == null || socket == null) return null;

        GameObject model = Instantiate(modelPrefab, socket);
        model.transform.SetLocalPositionAndRotation(Vector3.zero, modelPrefab.transform.localRotation);
        return model;
    }

    public override void TakeDamage(int damage, Character attacker)
    {
        base.TakeDamage(damage, attacker);
        GameManager.Instance?.UpdateHealthUI(Hp, MaxHp);
        if (Hp > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            animator.SetBool("isDead", true);
        }
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        GameManager.Instance?.UpdateHealthUI(Hp, MaxHp);
    }
}
