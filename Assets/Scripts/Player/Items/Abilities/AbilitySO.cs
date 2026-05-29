using UnityEngine;

public enum AbilityType
{
    Projectile,
    Status
}

[CreateAssetMenu(fileName = "New Ability", menuName = "Items/Ability")]
public class AbilitySO : ItemSO
{
    [Header("IMPACT EFFECTS")]
    public VFX impactEffect;

    public AbilityType abilityType;

    [Header("HitStop")]
    public bool hasHitStop;
    public float hitStopDuration;

    [Header("Shared")]
    public float cooldown;
    public float resourceCost;

    [Header("Projectile")]
    public int projectileSize;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float projectileGravity;
    public int projectileCount;
    public float spread;

    [Header("Status")]
    public BuffSO appliedBuff;
    public float duration;
    public float areaRadius;

    [Header("KNOCKBACK AND RECOIL")]
    public float knockback;
    public float recoil;
}