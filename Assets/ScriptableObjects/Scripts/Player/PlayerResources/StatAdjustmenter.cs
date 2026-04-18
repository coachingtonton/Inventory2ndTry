/// SUMMARY 
/// THIS SCRIPT IS RESPONSIBLE FOR UPDATING STATS BASED ON BUFFS OR STATUS EFFECTS
/// BUFFMANAGER CHECKS IF STATUS IS ALREADY PRESENT OR STACKABLE THEN USES 
/// RECALCULATE METHOD AND PASSES THE LIST OF ACTIVE BUFFS TO STAT ADJUSTMENTER
/// PLAYERSTATECONTROLLER READS THE MODIFIED VARIABLES FOR PLAYER MOVEMENT 


using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Speed,
    Damage,
    MaxHP,
    JumpForce,
    FireRate
}

public class StatAdjustmenter: MonoBehaviour
{
    [Header("Base Stats — set in inspector")]
    public float baseSpeed = 10f;
    public float baseDamage = 5f;
    public float baseMaxHP = 100f;
    public float baseJumpForce = 12f;
    public float baseFireRate = 1f;

    // MODIFIED — what everything reads at runtime
    [HideInInspector] public float speed;
    [HideInInspector] public float damage;
    [HideInInspector] public float maxHP;
    [HideInInspector] public float jumpForce;
    [HideInInspector] public float fireRate;

    void Awake()
    {
        Recalculate(null);
    }

    public void Recalculate(List<ActiveBuff> activeBuffs)
        //Arguments supplied by buffManager)
    {

        //Puts base numbers inside new variables 
        //ENSURES player state manager will read 
        //base + currentBuffs 
        speed = baseSpeed;
        damage = baseDamage;
        maxHP = baseMaxHP;
        jumpForce = baseJumpForce;
        fireRate = baseFireRate;

        if (activeBuffs == null) return;

        // STEP 2: apply all flat bonuses first
        foreach (ActiveBuff buff in activeBuffs)
        {
            switch (buff.data.targetStat)
            {
                case StatType.Speed: speed += buff.FlatTotal(); break;
                case StatType.Damage: damage += buff.FlatTotal(); break;
                case StatType.MaxHP: maxHP += buff.FlatTotal(); break;
                case StatType.JumpForce: jumpForce += buff.FlatTotal(); break;
                case StatType.FireRate: fireRate += buff.FlatTotal(); break;
            }
        }

        // STEP 3: apply all percentage bonuses on top
        // this way +20% scales off (base + flat), which feels right
        foreach (ActiveBuff buff in activeBuffs)
        {
            switch (buff.data.targetStat)
            {
                case StatType.Speed: speed += baseSpeed * buff.PercentTotal(); break;
                case StatType.Damage: damage += baseDamage * buff.PercentTotal(); break;
                case StatType.MaxHP: maxHP += baseMaxHP * buff.PercentTotal(); break;
                case StatType.JumpForce: jumpForce += baseJumpForce * buff.PercentTotal(); break;
                case StatType.FireRate: fireRate += baseFireRate * buff.PercentTotal(); break;
            }
        }
    }
}