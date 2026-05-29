using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    Dictionary<AmmoType, int> ammo = new Dictionary<AmmoType, int>();
    //public List<int> ammoList = new List<int>();

    /// FOR DEBUGGING, NEED TO VIEW AMMO  
    [SerializeField] private int shotGunAmmo;


    private void Start()
    {
        ammo.Add(AmmoType.SHOTGUN, 500);
        shotGunAmmo = ammo[AmmoType.SHOTGUN];
    }

    //Checks if player has enough ammo for the firearm
    public bool HasAmmo(AmmoType type, int ammoAmount)
    {
        Debug.Log($"HasAmmo called: {type}, have {AmmoCheck(type)}, need {ammoAmount}");
        if (AmmoCheck(type) >= ammoAmount)
        {
            return true;
        }
        else return false;
    }

    public int AmmoCheck(AmmoType ammoTypeCheck)
    {
        if (ammo.ContainsKey(ammoTypeCheck))
        {
            return ammo[ammoTypeCheck];
        }
        else return 0;
    }

    public void SpendAmmo(AmmoType ammoTypeSpent, int ammoSpent)
    {
        if (ammo.ContainsKey(ammoTypeSpent))
        {

            if (ammo[ammoTypeSpent] <= 0) { ammoTypeSpent = 0; }//PREVENTS going below 0

            ammo[ammoTypeSpent] -= ammoSpent;
            Debug.Log(ammo[ammoTypeSpent]);
        }
    }

    public void AddAmmo(AmmoType ammoTypeReceived, int ammoQuantityReceived)
    {
        if (ammo.ContainsKey(ammoTypeReceived))
        {
            ammo[ammoTypeReceived] += ammoQuantityReceived; 
        }
    }

    public void CheckAndSpendAmmo(AmmoType ammoType, int ammoQuantity)
    { /// Created to make other scripts neater
        if (HasAmmo(ammoType, ammoQuantity))
        {
            SpendAmmo(ammoType, ammoQuantity);
        }
        else
        {
            Debug.Log("Not Enough AMMO");
        }
    }
}
