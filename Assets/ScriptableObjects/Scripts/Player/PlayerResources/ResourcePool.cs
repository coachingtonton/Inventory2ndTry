using UnityEngine;
using System;

[System.Serializable]
public class ResourcePool

    ///THIS HAS EVERY FUCKING RESOURCE CHECK YOU NEED 
    ///WEAPONBASE HAS A PLAYERSTATS GETCOMPONENET ON AWAKE 
{
    public float current;
    public float max;
    public float regenRate;



    public event Action OnDepleted; /// REACTION WHEN RESOYURCE HITS ZERO 
    public event Action<float, float> OnChanged;

    public ResourcePool(float max)
    {
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