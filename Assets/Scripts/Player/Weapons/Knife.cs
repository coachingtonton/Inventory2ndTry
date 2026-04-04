using UnityEngine;

public class Knife : WeaponBase
{
    public override void PrimaryFire()
    {
        Debug.Log("you swing the knife");
    }

    public override void Reload()
    {
        Debug.Log("Knife is cleaned");
    }
}
