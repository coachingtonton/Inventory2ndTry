using UnityEngine;

/// <summary>
///             TODO 
///             when inventory is opened, player can simoltaniously attack and move items around
/// </summary>

public class InventoryUI : MonoBehaviour
{
    public GameObject bagPanel;
    SlotUI[] slots;
    public bool isOpen;

    void Start()
    {
        slots = bagPanel.GetComponentsInChildren<SlotUI>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].isHotbarSlot = false;
            slots[i].slotIndex = i;
        }

        bagPanel.SetActive(false);
    }

    void Update()
    {
        if (InputManager.Instance.iKeyPressed)
        {
            Debug.Log("IKEI PRESSED");
            ToggleBag();
        }

        if (isOpen)
        {
            RefreshSlots();
        }
    }

    void ToggleBag()
    {
        isOpen = !isOpen;
        bagPanel.SetActive(isOpen);
    }

    void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UpdateSlot(Inventory.Instance.bag[i]);
        }
    }
}