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

    [Header("Hitbox")]
    public Vector2 hitboxSize;      // width and height of attack box
    public Vector2 hitboxOffset;    // how far in front of player

    [Header("Knockback")]
    public float knockbackForce;

    [Header("Optional")]
    public BuffSO appliedBuff;      // poison sword, fire axe etc
}
