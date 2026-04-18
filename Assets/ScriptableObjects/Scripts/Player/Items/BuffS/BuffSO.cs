
/// WHAT A BUFF IS, HOLDS STATUS UPDATE, STATUS DURATION AND MAX STACKS 
/// THIS WILL BE FED TO ACTIVE BUFF. NO ONE ELSE 
/// ^^^^^^^^^^^^^^^^^^^
/// 
/// 
/// BUFFSO IS DATA SITTING IN THE PROJECT 
/// THEYRE ATTATHED TO POTION PICKUP, STATUS ZONE, ENEMY ATTACK, STATUS SPELLL
/// BUFF MANAGER WILL TAKE IN THE SO'S PARAMETERS AND APPLY THE BUFF
/// THIS EXISTS FOR buffManager.ApplyBuff(whateverbuffSO)


using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "1000pRun/Buff")]
public class BuffSO : ScriptableObject
{
    [Header("What it does")]
    public StatType targetStat;
    public float flatValue;        // +5 speed
    public float percentValue;     // +0.2 = 20% more speed

    [Header("Duration")]
    public float duration;         // seconds. 0 = permanent
    public bool isPermanent;

    [Header("Stacking")]
    public int maxStacks;          // how many times this buff can stack
    public bool refreshDuration;   // true = new stack resets timer
                                   // false = each stack has its own timer
}
