using UnityEngine;


[CreateAssetMenu(fileName = "NewGun", menuName = "1000pRun/GunItem")]
public class GunItemSO : ItemSO
{
    [Header("Gun Stats")]
    public GameObject projectilePrefab;
    public int projectileCount;
    public float spread;
    public float projectileSpeed;
    public int magSize;
    public float reloadTime;
    public int weaponDamage;
}