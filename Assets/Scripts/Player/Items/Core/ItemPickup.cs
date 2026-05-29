using UnityEngine;
using static GunItemSO;

public class ItemPickup : MonoBehaviour
{
    public AmmoSO ammoSO;
    public ItemSO item;
    public int pickupQuantity = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        ItemPickupHandling(other);
        AmmoHandling(other);
    }

    private void ItemPickupHandling(Collider2D other)
    {
        ///for player picking up InventoryItems
        if (item != null)
        {
            bool pickedUp = Inventory.Instance.AddItemToBag(item, pickupQuantity);

            if (pickedUp) Destroy(gameObject);
        }
        else return;
    }

    private void AmmoHandling(Collider2D other)
    {
        ///for player picking up AmmoDrops
        if (ammoSO != null)
        {
            AmmoInventory ammoInventory = other.GetComponent<AmmoInventory>();

            if (ammoInventory != null)
            {
                ammoInventory.AddAmmo(ammoSO.ammoType, ammoSO.ammoQuantity);
                Destroy(gameObject);
            }
        }
    }
}
