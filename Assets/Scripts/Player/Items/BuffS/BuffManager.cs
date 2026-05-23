using UnityEngine;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{
    List<ActiveBuff> activeBuffs = new List<ActiveBuff>();
    StatAdjustmenter stats;
    public bool existing;

    void Awake()
    {
        stats = GetComponent<StatAdjustmenter>();
    }

    void Update()
    {
        TickBuffs();
    }

    public void ApplyBuff(BuffSO buffData)
    {
        //CHECKS IF BUFF IS ALREADY ACTIVE 
        // STACKS BUFF OR RESETS TIME OF BUFF IF APPLICABLE
        ActiveBuff existing = null;
        foreach (ActiveBuff b in activeBuffs)
        {
            if (b.data == buffData)
            {
                existing = b;
            }
        }

        //IF ACTIVE BUFF SHARES SAME DATA EXISTING = TRUE 

        if (existing != null)
        { //ADDS STACKS OR DURATION IF ACTIVEBUFF CALLS FOR IT
            if (existing.stacks < buffData.maxStacks)
                existing.stacks++;

            if (buffData.refreshDuration)
                existing.timeRemaining = buffData.duration;
        }
        else
        {// ADDS BUFFSO TO ACTIVE BUFFS IF PASSES EARLIER CHECKS
         // INSTANTIATES ACTIVE BUFF CONSTRUCTOR AND ADDS THE BUFF TO ACTIVE BUFF LIST 
            activeBuffs.Add(new ActiveBuff(buffData));
        }

        //RECALCULATE UPDATES STATADJUSTMENTER FOR PLAYERSTATECONTROLLER
        stats.Recalculate(activeBuffs);
    }

    public void RemoveBuff(BuffSO buffData)
    {
        //IF BUFFDATA IS THE SAME AS AN ACTIVE BUFFS DATA, REMOVEIT 
        foreach (ActiveBuff b in activeBuffs)
        { 
            if (buffData == b.data)
                activeBuffs.Remove(b);
        }
        stats.Recalculate(activeBuffs);
    }

    void TickBuffs()
    {
        bool changed = false;

        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            if (activeBuffs[i].data.isPermanent) continue;//if status is permanent it dont go awayy

            activeBuffs[i].timeRemaining -= Time.deltaTime;
            //Starts the timer on whatever status or buff is active in the list 

            if (activeBuffs[i].timeRemaining <= 0f)
            { //IF timer reaches less than 0 then status is removed
                activeBuffs.RemoveAt(i);
                changed = true;
            }
        }
        if (changed)
            stats.Recalculate(activeBuffs);
        //IF STATUS IS REMOVED THEN RECALCULATE 
    }
}