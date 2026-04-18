using UnityEngine;

public enum AbilityType
{
    Projectile,
    Status
}

[CreateAssetMenu(fileName = "New Ability", menuName = "Items/Ability")]
public class AbilitySO : ItemSO
{
    public AbilityType abilityType;

    [Header("Shared")]
    public float cooldown;
    public float resourceCost;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float fireRate;
    public float projectileSpeed;
    public int projectileCount;
    public float spread;

    [Header("Status")]
    public BuffSO appliedBuff;
    public float duration;
    public float areaRadius;
}