using UnityEngine;
/// <summary>
/// Determins the hitbox size
/// </summary>

[CreateAssetMenu(fileName = "New Melee", menuName = "Items/Melee")]

public class MeleeItemSO : ItemSO
{
    [Header("Attack Stats")]
    public float attackDuration;    // total swing time
    public float windupHitBoxDelay;       // wind-up before hitbox appears
    public float hitboxDuration;    // how long hitbox stays active
    public float cooldown;

    [Header("OFFSET")]
    public Vector2 hitboxOffsetRight;
    public Vector2 hitboxOffsetOverhead;
    public Vector2 hitboxOffsetLeft;
    public Vector2 hitboxOffsetLower;


    [Header("Hitbox")]
    public Vector2 hitboxSizeSideXSide;
    public Vector2 hitboxSizeOVERHEAD;
    public Vector2 hitboxSizeLower;


    [Header("Knockback")]
    public bool hasKnockback;
    public float knockbackForceX;
    public float knockbackForceY;

    [Header("Optional")]
    public BuffSO appliedBuff;      // poison sword, fire axe etc
}
