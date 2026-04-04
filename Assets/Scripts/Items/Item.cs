using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "1000pRun/Item")]
public class ItemSO : ScriptableObject
{
    public int count;
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public int damage;
    public float fireRate;
    public bool isStackable;
    [TextArea] public string description;
}

public enum ItemType
{
    Weapon,
    Consumable,
    Passive
}