using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    public SlotUI[] slots;
    public Image activeHighlight;

    void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].isHotbarSlot = true;
            slots[i].slotIndex = i;
        }
    }

    void Update()
    {
        RefreshSlots();
        UpdateHighlight();
    }

    void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UpdateSlot(Inventory.Instance.hotbar[i]);
        }
    }

    void UpdateHighlight()
    {
        int active = Inventory.Instance.equippedSlot;
        activeHighlight.transform.position = slots[active].transform.position;
    }
}