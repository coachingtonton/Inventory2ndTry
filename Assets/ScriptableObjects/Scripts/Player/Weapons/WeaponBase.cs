using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    // VIRTUAL MEANS ALL CHILDREN INHERIT BUT CAN ALSO BE 
    // OVVERIEDEDEDEDEDED

    public WeaponType WeaponType;
    public ItemSO itemData;
    protected PlayerStats playerStats;

    public virtual void Awake()
    {
        //Used for handlers that cost resources.
        playerStats = GetComponentInParent<PlayerStats>();
    }

    public virtual void Equip(ItemSO item)
    {
        itemData = item;
    }

        public virtual void PrimaryFire() { }
    public virtual void SecondaryFire() { }
    public virtual void Reload() { }
}
