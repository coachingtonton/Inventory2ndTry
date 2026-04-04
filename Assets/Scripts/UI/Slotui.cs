using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI countText;
    public bool isHotbarSlot;
    public int slotIndex;

    public void UpdateSlot(ItemSO item)
    {
        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;

            if (item.isStackable && item.count > 1)
            {
                countText.text = item.count.ToString();
                countText.enabled = true;
            }
            else
            {
                countText.enabled = false;
            }
        }
        else
        {
            icon.enabled = false;
            countText.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryDragManager.Instance.SlotClicked(this);
    }
}