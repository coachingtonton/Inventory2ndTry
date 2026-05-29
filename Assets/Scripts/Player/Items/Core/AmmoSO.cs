using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "1000pRun/AmmoItem")]
public class AmmoSO : ScriptableObject
{
    [Header("AMMO STATS")]
    public int ammoQuantity;
    public AmmoType ammoType;
}
