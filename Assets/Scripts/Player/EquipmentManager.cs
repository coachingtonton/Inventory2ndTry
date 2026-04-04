using UnityEngine;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    // dictionary maps weapon type to its script on the player
    // WeaponType is the key, WeaponBase is the value
    Dictionary<WeaponType, WeaponBase> weapons = new Dictionary<WeaponType, WeaponBase>();

    // whatever weapon is currently in use, null if nothing equipped
    WeaponBase activeWeapon;

    private void Awake()
    {
        // find every WeaponBase script on player children (true = include disabled)
        // add each one to the dictionary and disable it
        foreach (WeaponBase weaponObject in GetComponentsInChildren<WeaponBase>(true))
        {
            weapons.Add(weaponObject.WeaponType, weaponObject);
            weaponObject.gameObject.SetActive(false);
        }
    }

    public void Equip(int hotBarIndex)
    {
        // tell inventory which slot is now active
        Inventory.Instance.SwitchActiveSlot(hotBarIndex);

        // get the ItemSO sitting in that slot
        ItemSO item = Inventory.Instance.GetActiveItem();

        // turn off old weapon if one was equipped
        if (activeWeapon != null)
            activeWeapon.gameObject.SetActive(false);

        // if slot is empty or item isnt a weapon, clear and stop
        if (item == null || item.itemType != ItemType.Weapon)
        {
            activeWeapon = null;
            return;
        }

        // look up the dictionary: weaponType key → WeaponBase value
        // enable it and pass the ItemSO so it knows its stats
        activeWeapon = weapons[item.weaponType];
        activeWeapon.gameObject.SetActive(true);
        activeWeapon.Equip(item);
    }

    private void Update()
    {
        if (InputManager.Instance.onePressed) Equip(0);
        if (InputManager.Instance.twoPressed) Equip(1);
        if (InputManager.Instance.threePressed) Equip(2);
        if (InputManager.Instance.fourPressed) Equip(3);
        if (InputManager.Instance.fivePressed) Equip(4);
        if (InputManager.Instance.sixPressed) Equip(5);
        if (InputManager.Instance.sevenPressed) Equip(6);
        if (InputManager.Instance.eightPressed) Equip(7);
        if (InputManager.Instance.ninePressed) Equip(8);

        if (activeWeapon != null && InputManager.Instance.firePressed)
            activeWeapon.PrimaryFire();
    }
}