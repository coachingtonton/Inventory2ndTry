using UnityEngine;
using System.Collections.Generic;

/// <summary>
///                 EQUIPMENT SYSTEM 
///         activeHandler holds whichever handler is currently in use.
///         a handler is a sc
///         One handler per weapon category:
///      FirearmHandler  — all guns (shotgun, pistol, SMG, rifle)
///      MeleeHandler    — all melee (knife, sword, bat)
///      MagicHandler    — all magic (staff, wand, tome)
///      ExplosiveHandler — all explosives (grenade, rocket, mine)
///         Each handler reads its stats from the SO equipped in the hotbar.
///         Same handler, different SO = different weapon.
/// </summary>

public class EquipmentManager : MonoBehaviour
{
    // DICTIONARY: maps WeaponType to the handler script on the player
    // Awake fills this by searching children for any script that extends WeaponBase
    // GunHandler extends WeaponBase so it gets found
    // MeleeHandler, MagicHandler etc will also get found when added later
    Dictionary<WeaponType, WeaponBase> weapons = new Dictionary<WeaponType, WeaponBase>();

    // variable for wahtever weaponhandler is currently active 
    WeaponBase equippedDataHandler;

    private void Awake()
    {
        // GRABS EVERY WEAPON HANDLER INSIDE PLAYER GAMEOBJECT AND STORES IN DICTIONARY 
        // EACH WEAPON HANDLER WILL USE EQUIPPED ITEMS QUALITIES AND FUNCTIONS 
        foreach (WeaponBase weaponObject in GetComponentsInChildren<WeaponBase>(true))
        {
            Debug.Log("Found: " + weaponObject.WeaponType);
            weapons.Add(weaponObject.WeaponType, weaponObject);
            weaponObject.gameObject.SetActive(false);
        }
    }

    public void Equip(int hotBarIndex)
    {
        // tell inventory which slot is selected
        Inventory.Instance.SwitchActiveSlot(hotBarIndex);


        // get the ITEMS SPECIFIC reference sitting in THE CURRENTLY SELECTED slot
        // this could be a GunItemSO, MeleeItemSO, or null if empty
        ItemSO item = Inventory.Instance.GetActiveItem();

        // turn off old data handler if one was active
        if (equippedDataHandler != null)
            equippedDataHandler.gameObject.SetActive(false);

        // if slot is empty or not a datahandlerREFRENCE, clear and stop
        if (item == null || item.itemType != ItemType.Weapon)
        {
            equippedDataHandler = null;
            return;
        }

        // DICTIONARY LOOKUP:
        // item.weaponType is the KEY (ex: WeaponType.Shotgun)
        // dictionary returns the VALUE (ex: GunHandler script)
        // this works because Awake registered GunHandler with WeaponType.Shotgun
        equippedDataHandler = weapons[item.weaponType];

        // turn on the handler's GameObject so it runs
        equippedDataHandler.gameObject.SetActive(true);

        // pass the ItemSO into the handler
        // GunHandler casts it to GunItemSO to read gun-specific stats
        equippedDataHandler.Equip(item);
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

        // CURRENTLY EQUIPPED DATA HANDLERS METHODS 
        if (equippedDataHandler != null && InputManager.Instance.fireHeld)
            equippedDataHandler.PrimaryFire();
        if (equippedDataHandler != null && InputManager.Instance.rPressed)
            equippedDataHandler.Reload();
    }

    // CURRENTLY USING THIS FOR TESTING
    //  TODO ::: FIRE THIS ALONG SIDE ITEM PICKED UP EVENT

}