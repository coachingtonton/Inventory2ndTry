using UnityEngine;
using UnityEngine.UI;

public class InventoryDragManager : MonoBehaviour
{
    public static InventoryDragManager Instance;

    public Image ghostIcon;
    public SlotUI selectedSlot;

    void Awake()
    {
        Instance = this;
        ghostIcon.enabled = false;
    }

    void Update()
    {
        if (ghostIcon.enabled)
        {
            ghostIcon.transform.position = Input.mousePosition;
        }
    }

    public void SlotClicked(SlotUI slot)
    {

        // first click — pick up
        if (selectedSlot == null)
        {
            selectedSlot = slot;
            ghostIcon.sprite = GetItemFromSlot(slot)?.icon;
            ghostIcon.enabled = ghostIcon.sprite != null;
            return;
        }

        // second click — swap and clear
        SwapSlots(selectedSlot, slot);
        selectedSlot = null;
        ghostIcon.enabled = false;
    }

    ItemSO GetItemFromSlot(SlotUI slot)
    {
        if (slot.isHotbarSlot)
            return Inventory.Instance.hotbar[slot.slotIndex];
        else
            return Inventory.Instance.bag[slot.slotIndex];
    }

    void SwapSlots(SlotUI a, SlotUI b)
    {
        // both in bag
        if (!a.isHotbarSlot && !b.isHotbarSlot)
            Inventory.Instance.SwapBagSlots(a.slotIndex, b.slotIndex);

        // both in hotbar
        else if (a.isHotbarSlot && b.isHotbarSlot)
            Inventory.Instance.SwapSlots(a.slotIndex, b.slotIndex);

        // bag to hotbar
        else if (!a.isHotbarSlot && b.isHotbarSlot)
            Inventory.Instance.MoveToHotbar(a.slotIndex, b.slotIndex);

        // hotbar to bag
        else if (a.isHotbarSlot && !b.isHotbarSlot)
            Inventory.Instance.MoveToInventory(a.slotIndex, b.slotIndex);
    }
}