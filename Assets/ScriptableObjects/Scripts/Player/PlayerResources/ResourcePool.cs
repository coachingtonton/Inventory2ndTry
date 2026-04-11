using UnityEngine;
using System;

[System.Serializable]
public class ResourcePool
{
    public float current;
    public float max;
    public float regenRate;

    public event Action OnDepleted;
    public event Action<float, float> OnChanged;

    public ResourcePool(float max)
    {
        this.max = max;
        this.current = max;
    }

    public void Spend(float amount)
    {
        current -= amount;
        if (current <= 0f)
        {
            current = 0f;
            OnDepleted?.Invoke();
        }
        OnChanged?.Invoke(current, max);
    }
    public void Gain(float amount)
    {
        current += amount;
        if (current > max)
            current = max;
        OnChanged?.Invoke(current, max);
    }

    public bool HasEnough(float amount)
    {
        return current >= amount;
    }

    public void Regen(float deltaTime)
    {
        if (regenRate > 0f && current < max)
            Gain(regenRate * deltaTime);
    }
}