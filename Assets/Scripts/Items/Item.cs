using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "1000pRun/Item")]
public class ItemSO : ScriptableObject
{
    public int count;
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public WeaponType weaponType;
    public int damage;
    public float fireRate;
    public bool isStackable;
    [TextArea] public string description;

    [Header("Shotgun Stats")]
    public int pelletCount;
    public float spread;
}

public enum ItemType
{
    Weapon,
    Consumable,
    Passive
}

public enum WeaponType
{
    Shotgun,
    Pistol,
    Knife,
    SMG,
    Rifle,
}