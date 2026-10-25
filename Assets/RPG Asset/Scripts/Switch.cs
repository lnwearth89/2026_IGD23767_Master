using UnityEngine;
using UnityEngine.Events;

public class Switch : Identity, IInteractable
{
    [SerializeField] private bool isOn = false;
    public UnityEvent<bool> OnSwitch;

    public bool IsOn
    {
        get { return isOn; }
        set { isOn = value; }
    }

    public void Use(Player player)
    {
        if (!isOn)
        {
            Debug.Log("The switch is off. You need to turn it on first.");
            OnSwitch?.Invoke(true);
            isOn = true;
        }
        else
        {
            Debug.Log("You can turn off the switch.");
            OnSwitch?.Invoke(false);
            isOn = false;
        }
    }
}