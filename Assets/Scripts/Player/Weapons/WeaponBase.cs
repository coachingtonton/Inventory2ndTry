using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public WeaponType WeaponType;
    public ItemSO itemData;

    public virtual void Equip(ItemSO item)
    {
        itemData = item;
    }
        public virtual void PrimaryFire() { }
    public virtual void SecondaryFire() { }
    public virtual void Reload() { }
}
