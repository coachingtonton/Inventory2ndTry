using UnityEngine;
using System;

[System.Serializable]
public class ResourcePool

    ///THIS HAS EVERY FUCKING RESOURCE CHECK YOU NEED 
    ///WEAPONBASE HAS A PLAYERSTATS GETCOMPONENET ON AWAKE 
    ///This script gets instantiated by whatever object needs it
{
    public float current;
    public float max;
    public float regenRate;

    public readonly float maxReadAmt;
    public readonly float currentReadAmt;


    public event Action OnDepleted; /// REACTION WHEN RESOYURCE HITS ZERO 
    public event Action<float, float> OnChanged; /// EVENT FOR WHEN HEALTH CHANGES

    public ResourcePool(float max)
    {
        this.currentReadAmt = current;
        this.maxReadAmt = max;

        this.max = max;
        this.current = max;
    }

    public void Spend(float amount) /// CONSUME THE RESOURECE 
    {
        current -= amount;
        if (current <= 0f)
        {
            current = 0f;
            OnDepleted?.Invoke();
            //IF THE RESOURCE REACHES 0 THEN ON DEPLETED IS INVOKED
            //Ondepleted 
        }
        OnChanged?.Invoke(current, max);
    }
    public void Gain(float amount) /// GAIN THE RESOURCE 
    {
        current += amount;
        if (current > max)
            current = max;
        OnChanged?.Invoke(current, max);
    }

    public bool HasEnough(float amount) /// 
    {
        return current >= amount;
    }

    public bool TrySpend(float amount) /// RUNS A CHECK FOR COST AND RUNS IF CAN AFFORD
    {
        if (current < amount) return false;
        Spend(amount);
        return true;
    }

    public void Regen(float deltaTime) 
    {
        if (regenRate > 0f && current < max)
            Gain(regenRate * deltaTime);
    }
}