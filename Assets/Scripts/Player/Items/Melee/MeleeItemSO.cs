using UnityEngine;
/// <summary>
/// Determins the hitbox size
/// </summary>

[CreateAssetMenu(fileName = "New Melee", menuName = "Items/Melee")]

public class MeleeItemSO : ItemSO
{
    [Header("OTHER ATTACKS")]
    public MeleeItemSO secondaryAttackScript;
    public MeleeItemSO primaryAttackScript;

    [Header("Attack Stats")]
    public float attackDuration;    // total swing time
    public float windupHitBoxDelay;       // wind-up before hitbox appears
    public float hitboxDuration;    // how long hitbox stays active
    public float cooldown;

    [Header("HitStop")]
    public bool hasHitStop;
    public float hitStopDuration;
    //ADD HEAVY ATTACK HITSTOP DURATION 
    //add heavy attack damage 

    [Header("OFFSET")]
    public Vector2 hitboxOffsetRight;
    public Vector2 hitboxOffsetOverhead;
    public Vector2 hitboxOffsetLeft;
    public Vector2 hitboxOffsetLower;


    [Header("Hitbox")]
    public Vector2 hitboxSizeSideXSide;
    public Vector2 hitboxSizeOVERHEAD;
    public Vector2 hitboxSizeLower;

    [Header("ChargeAttack")]
    public float chargeAttackThreshold;


    [Header("Knockback")]
    public bool hasKnockback;
    public Vector2 knockbackOverhead;  // e.g. (0, 15) — launcher
    public Vector2 knockbackLower;     // e.g. (0, -15) — slam
    public Vector2 knockbackLeft;      // e.g. (-8, 3) — combo extender
    public Vector2 knockbackRight;     // e.g. (8, 3)

    [Header("Optional")]
    public BuffSO appliedBuff;      // poison sword, fire axe etc
}
