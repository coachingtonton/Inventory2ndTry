using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public ItemSO[] bag = new ItemSO[27];
    public ItemSO[] hotbar = new ItemSO[9];
    public int equippedSlot;

    void Awake()
    {
        Instance = this;
    }

    public bool AddItemToBag(ItemSO newItem, int pickupQuantity)
    {
        // TODO subscribe this method to ItemPickedUpEvent
        // 1. Tries to stack
        if (newItem.isStackable)
        {
            for (int i = 0; i < bag.Length; i++)
            {
                if (bag[i] != null && bag[i].itemName == newItem.itemName)
                {
                    bag[i].count += pickupQuantity;
                    return true;
                }
            }
        }

        // 2. Find empty slot fore newitem if stack fails
        for (int i = 0; i < bag.Length; i++)
        {
            if (bag[i] == null)
            {
                bag[i] = newItem;
                return true;
            }
        }

        return false;
    }

    public void MoveToHotbar(int bagIndex, int hotBarIndex)
    {
        ItemSO temp = hotbar[hotBarIndex];
        hotbar[hotBarIndex] = bag[bagIndex];
        bag[bagIndex] = temp;
    }

    public void SwapSlots(int aHotbarIndex, int bHotbarIndex)
    {
        ItemSO temp = hotbar[aHotbarIndex];
        hotbar[aHotbarIndex] = hotbar[bHotbarIndex];
        hotbar[bHotbarIndex] = temp;
    }

    public void SwitchActiveSlot(int slot)
    {
        //takes in input for Hotbar index
        equippedSlot = slot;
    }

    public void SwapBagSlots(int bagSlotA, int bagSlotB)
    {
        ItemSO temp = bag[bagSlotA];
        bag[bagSlotA] = bag[bagSlotB];
        bag[bagSlotB] = temp;
    }

    public void MoveToInventory(int hotbarIndex, int bagIndex)
    {
        ItemSO temp = hotbar[hotbarIndex];
        hotbar[hotbarIndex] = bag[bagIndex];
        bag[bagIndex] = temp;
    }

    public ItemSO GetActiveItem()
    {
        // Finds highlighted slots index in array
        // Press 3 -> equippedSlot becomes hotbar[2] -> returns the SO for equippedSlot
        // For the handler to later use
        return hotbar[equippedSlot];
    }
}
