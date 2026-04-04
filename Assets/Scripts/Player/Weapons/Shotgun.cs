using UnityEngine;

public class Shotgun : WeaponBase
{
    public override void PrimaryFire()
    {
        Debug.Log("Shotgun fires " + itemData.pelletCount + " pellets at " + itemData.damage + " damage");
    }

    public override void Reload()
    {
        Debug.Log("Shotgun reloading");
    }
}
