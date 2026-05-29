using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "1000pRun/Item")]
public class ItemSO : ScriptableObject
{
    [Header("ITEM SO STATS")]
    public int count;
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public WeaponType weaponType;
    public AmmoType ammoType;
    public int damage;
    public bool isStackable;
    [TextArea] public string description;
    public Vector2 enemyKnockback;
}

public enum ItemType
{
    Weapon,
    Consumable,
    Passive
}

public enum WeaponType
{
    FireArm,
    Melee,
    Ability,
    Explosive,
}