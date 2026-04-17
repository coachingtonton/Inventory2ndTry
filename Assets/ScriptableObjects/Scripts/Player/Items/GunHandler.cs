using UnityEngine;

public class GunHandler : WeaponBase
{
    [SerializeField] Transform firePoint;
    GunItemSO gunData;
    int currentMag;
    float fireTimer;

    public override void Equip(ItemSO item) 
    {
        itemData = item;
        gunData = item as GunItemSO;
        currentMag = gunData.magSize;
        fireTimer = 0f;
    }

    private void Update()
    {
        Debug.Log(fireTimer);
        fireTimer -= Time.deltaTime;

    }

    public override void PrimaryFire()
    {
        if (gunData == null) return;
        if (currentMag <= 0) return;
        if (fireTimer > 0f) return;

        fireTimer = gunData.fireRate;

        for (int i = 0; i < gunData.projectileCount; i++)
        {
            float bulletSpreadRange = Random.Range(-gunData.spread, gunData.spread);
            SpawnProjectile(bulletSpreadRange);
        }
    }

    public override void SecondaryFire()
    {
        // ADD SECONDARY FIRE LATYER 
    }

    public override void Reload()
    {
        currentMag = gunData.magSize;
    }

    void SpawnProjectile(float angle)
    {
        Vector2 direction = Quaternion.Euler(0, 0, angle) * firePoint.right;
        GameObject bullet = Instantiate(gunData.projectilePrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().Init(direction, gunData.projectileSpeed, gunData.damage);
    }
}
