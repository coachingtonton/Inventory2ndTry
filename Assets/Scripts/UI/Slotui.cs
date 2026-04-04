using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
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
}