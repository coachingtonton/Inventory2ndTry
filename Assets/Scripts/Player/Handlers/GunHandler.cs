using System.Collections;
using System.IO.Pipes;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

/// <summary>
/// GUNITEM SCRIPT
/// 
/// USES CURRENT ATTACK TO DETERMINE IF PRIMARY OR SECONDARY FIRE
/// GUNITEM SCRIPTABLE OBJECTS DETERMINE ALL OF THE GUNS DATA
/// 
/// canSecondaryAttack property 
/// is used for checking if player has enough ammo for secondary attack 
/// 
/// </summary>

public enum AmmoType { SMG, RIFLE, SHOTGUN, PISTOL }

public class GunHandler : WeaponBase
{
    GunItemSO gunData;
    int currentMag;
    float fireTimer;
    public bool canReload;
    public int ammoInInventory;
    public bool secondaryAttackPressed;

    [Header("OtherComponents")]
    private Rigidbody2D playerRB;
    private PlayerStateController playerStateController;
    [SerializeField] Transform firePoint;
    private AmmoInventory ammoInventory;

    public float fireArmPlayerRecoilAmount; // stores inAir or grounded based on playerControlelr property

    override public void Awake()
    {
        playerRB = GetComponentInParent<Rigidbody2D>();
        playerStateController = GetComponentInParent<PlayerStateController>();
        ammoInventory = GetComponentInParent<AmmoInventory>();
    }

    public void Start()
    {
        if (ammoInventory.HasAmmo(gunData.ammoType, gunData.magSize))
        {
            ammoInInventory = ammoInventory.AmmoCheck(gunData.ammoType);
        }
    }

    public override void Equip(ItemSO item) 
    {
        itemData = item;
        gunData = item as GunItemSO;
        currentMag = gunData.magSize;
        fireTimer = 0f;
    }

    private void Update()
    {
        //Debug.Log(fireTimer);
        fireTimer -= Time.deltaTime;

        //Debug.Log(fireTimer);
    }

    public override void SecondaryFire()
    {
        if (gunData == null) return;
        if (currentMag <= 0) return;
        if (fireTimer > 0f) return;
        if (!canSecondaryAttack) return;

        secondaryAttackPressed = true;

        FireWeapon();
    }

    public override void PrimaryFire()
    {
        if (gunData == null) return;
        if (currentMag <= 0) return;
        if (fireTimer > 0f) return;

        secondaryAttackPressed = false;

        FireWeapon();

        fireTimer = gunData.fireRate;

        //Checks if player has enough ammo and fires if conditions met
    }

    public void FireWeapon()
    {
        //Property detemrines if weapon is a primary or secondary attack bnased off Right or left click
        GunItemSO primaryOrSecondary = PrimaryOrSecondaryAttack;


        if (ammoInventory.HasAmmo(primaryOrSecondary.ammoType, primaryOrSecondary.bulletsPerFire))
        {
            currentMag -= primaryOrSecondary.bulletsPerFire; //subs a bullet from currentmag every shot

            for (int i = 0; i < primaryOrSecondary.projectileCount; i++)
            {
                float bulletSpreadRange = Random.Range(-primaryOrSecondary.spread, primaryOrSecondary.spread);
                SpawnProjectile(bulletSpreadRange);
            }

            ApplyPlayerBodyRecoil();
        }


        if (ammoInventory.HasAmmo(primaryOrSecondary.ammoType, primaryOrSecondary.bulletsPerFire) == false)
        {
            Debug.Log("NMO AMMO LEFT");
        }
    }

    public override void Reload()
    {

        int ammoNeededForFullMag = gunData.magSize - currentMag;
        int ammoAvaliable = ammoInventory.AmmoCheck(gunData.ammoType);

        int ammoToReload = Mathf.Min(ammoNeededForFullMag, ammoAvaliable);
        // takes whichever ammo pool is smallest and adds it to the larger one

        currentMag += ammoToReload;
        ammoInventory.SpendAmmo(gunData.ammoType, ammoToReload);
    }

   void SpawnProjectile(float angle)
    {
        GunItemSO primaryOrSecondary = PrimaryOrSecondaryAttack;

        // FOLLOWS FIREPOINT AND INSTANTIATES PROJECTILE.
        Vector2 direction = Quaternion.Euler(0, 0, angle) * firePoint.right;
        GameObject bullet = Instantiate(PrimaryOrSecondaryAttack.projectilePrefab, firePoint.position, firePoint.rotation);

        //Initializes projectile with the needed properties 
        bullet.GetComponent<Projectile>().Init(direction, PrimaryOrSecondaryAttack.projectileSpeed, PrimaryOrSecondaryAttack.damage,
            PrimaryOrSecondaryAttack.projectileGravity, PrimaryOrSecondaryAttack.hitStopDuration,
            PrimaryOrSecondaryAttack.impactEffect, PrimaryOrSecondaryAttack.enemyKnockback);
    }

    public void ApplyPlayerBodyRecoil()
    {
        GunItemSO primaryOrSecondary = PrimaryOrSecondaryAttack;

        if (!PrimaryOrSecondaryAttack.hasPlayerRecoil) return;

        // Decides player recoil based on if player is in the air or if the player is grounded 
        fireArmPlayerRecoilAmount = playerStateController.isGrounded ? PrimaryOrSecondaryAttack.playerRecoil: 
            PrimaryOrSecondaryAttack.inAirPlayerRecoil;

        //applies Force in opposite direction of firepoint to player, SO supplies Recoil amt
        playerRB.AddForce(-firePoint.right * fireArmPlayerRecoilAmount, ForceMode2D.Impulse);
    }

    public GunItemSO PrimaryOrSecondaryAttack
    {
        get
        { //If left or right click is pressed property will act accordigny
        if (gunData == null) return null;

            // runs secondary attack is player has enough ammo for secondary fire && secondary fire pressed 
        if (secondaryAttackPressed == true )
            return gunData.secondaryAttackScript;

        else
            return gunData.primaryAttackScript;
        }
    }

    public bool canSecondaryAttack
    {
        get {return currentMag >= gunData.secondaryFireAmmoCost;} // both SO's have the same value for secondaryfire
    }

}
