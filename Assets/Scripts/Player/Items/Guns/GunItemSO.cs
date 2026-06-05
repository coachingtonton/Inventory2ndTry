using UnityEngine;


[CreateAssetMenu(fileName = "NewGun", menuName = "1000pRun/GunItem")]
public class GunItemSO : ItemSO
{
    public enum AmmoType { SMG, RIFLE, SHOTGUN, PISTOL }

    [Header("IMPACT EFFECTS")]
    public VFX impactEffect;

    [Header("OTHER ATTACKS")]
    public GunItemSO secondaryAttackScript;
    public GunItemSO primaryAttackScript;

    [Header("Gun Stats")]
    public int weaponDamage;
    public float fireRate;
    public float spread;

    [Header("AMMO")]
    public int secondaryFireAmmoCost;
    public float reloadTime;
    public int magSize;
    public int maxReserve;
    public int bulletsPerFire;

    [Header("Recoil and Knockback")]
    public float knockBackDealt;
    public float playerRecoil;
    public float inAirPlayerRecoil;
    public bool hasPlayerRecoil;

    [Header("Projectiles")]
    public float projectileSize;
    public float projectileSpeed;
    public float projectileGravity;
    public GameObject projectilePrefab;
    public int projectileCount;

    [Header("HitStop")]
    public bool hasHitStop;
    public float hitStopDuration;
}