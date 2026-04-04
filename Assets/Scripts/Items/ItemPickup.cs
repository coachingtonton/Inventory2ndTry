using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemSO item;
    public int pickupQuantity = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        bool pickedUp = Inventory.Instance.AddItemToBag(item, pickupQuantity);

        if (pickedUp) Destroy(gameObject);
    }
}
