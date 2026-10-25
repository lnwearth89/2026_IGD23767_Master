using UnityEngine;

public class Door : Identity, IInteractable
{
    [SerializeField] private bool isOpen = true;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value; }
    }

    public void Use(Player player)
    {
        if (!isOpen)
        {
            Debug.Log("The door is closed. You need to open it first.");
            animator.SetBool("isOpen", true);
            isOpen = true;
        }
        else
        {
            Debug.Log("You can pass through the open door.");
            animator.SetBool("isOpen", false);
            isOpen = false;
        }
    }
}