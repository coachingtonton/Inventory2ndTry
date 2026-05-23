using UnityEngine;

/// <summary>
/// THIS SCRIPT ALLOWS THE USE OF WHATEVER ABILITY IS CURRENTLY EQUIPPED 
/// 
/// 
/// TO DO 
/// FIGURE OUT A WAY TO CHARGE A SPELL BEFORE CASTING, LIKE FIREBALL SKYRIM SHIT
/// </summary>

public class AbilityHandler : WeaponBase
{
    [SerializeField] Transform firePoint;
    AbilitySO abilityData;
    float coolDownTimer;
    BuffManager buffManager;

    public override void Awake()
    {
        base.Awake();
        buffManager = GetComponentInParent<BuffManager>();
    }

    void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public override void Equip(ItemSO item)
    {
        // EQUIP IS RUN INSIDE EQUIPMENT MANAGER 
        // ITS CALLED TO SEE ITEMDATA AND WEAPON TYPE
        // SO EQUIPMANAGER KNOWS TO SWITCH TO CORRESPONDING
        // ITEM MANAGER 
        itemData = item;
        abilityData = item as AbilitySO;//sTOPS crashing from happenig if not abilitySO 
        coolDownTimer = abilityData.cooldown;
    }


    public override void PrimaryFire()
    {
        if (abilityData == null) return;

        if (coolDownTimer > 0f) return;  // checks cooldown 

        if (!playerStats.mana.TrySpend(abilityData.resourceCost))
        {// IF PLAYER CANNOT AFFORD a SPELL's RESOURCE COST
            Debug.Log("not enough MANA ");
            return;
        }

        coolDownTimer = abilityData.cooldown; // cooldown till next spell is Casted

        switch (abilityData.abilityType)
        {
            //ABILITYSO STATES WHAT ABILITY TYPE
            //ABILITY TYPE DETERMINES METHOD RAN 
            case AbilityType.Projectile:
                CastProjectile();
                break;
            case AbilityType.Status:
                CastStatus();
                break;
        }
    }

    public void CastStatus()
    {
        if (abilityData.appliedBuff == null) return;
        //iFAPPLIED buff is missing, wont crash the game 
        buffManager.ApplyBuff(abilityData.appliedBuff);
        //GRABS EQUIPPED SO AND USES ITS STATUS ABILITY  
    }

    public void CastProjectile()
    {
        ///SAME PROJECTILESPAWNING LOGIC AS GUNHANDLER WITH FIELDS FROM ABILITYSO


        //Some spells will have rapid fire, some will need to be charged like skyrim 

        for (int i = 0; i < abilityData.projectileCount; i++)
        {
            float bulletSpreadRange = Random.Range(-abilityData.spread, abilityData.spread);
            SpawnProjectile(bulletSpreadRange);
        }
    }

    void SpawnProjectile(float angle)
    {
        // FOLLOWS FIREPOINT AND INSTANTIATES PROJECTILE.
        Vector2 direction = Quaternion.Euler(0, 0, angle) * firePoint.right;
        GameObject bullet = Instantiate(abilityData.projectilePrefab, firePoint.position, firePoint.rotation);

        bullet.GetComponent<Projectile>().Init(direction, abilityData.projectileSpeed, abilityData.damage, abilityData.projectileGravity);
        //Gets projectile componenet on initialization 
        bullet.transform.localScale = Vector3.one * abilityData.projectileSize;
    }
}
