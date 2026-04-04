using NUnit.Framework.Interfaces;
using UnityEngine;

public class Pistol : WeaponBase
{
    public override void PrimaryFire()
    {
        Debug.Log("pistol fires " + itemData.damage);
    }

    public override void Reload()
    {
        Debug.Log("PistolReloads");
    }
}
