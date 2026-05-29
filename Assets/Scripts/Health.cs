using UnityEngine;
using System;


public class Health : MonoBehaviour, IDamageable
{
    public ResourcePool health = new ResourcePool(100);
    public bool isDead;
    public event Action OnDeath;
    public event Action onHealthChanged;

    public event Action onHealthDamaged; //HitEffect manager will refrence this one

    public void Awake()
    {
        health.OnDepleted += () => OnDeath?.Invoke();
        health.OnChanged += (current, max) => onHealthChanged?.Invoke();
        // SUBSCRIBING Healths onchanged and OnDeath Events to resource pool's corresponding ones
        // Doing this to lessen my confusion, many componenets will subscribe so i wanna keep it clean
    }

    public void Start()
    {
        Debug.Log(health.current);
        OnDeath += OnZeroHealth;
        onHealthChanged += () => HealthChanged();
    }

    public void TakeDamage(float amount)
    {
        health.Spend(amount);
        onHealthChanged?.Invoke();
        onHealthDamaged?.Invoke();
    }

    public void HealthChanged()
    {
        Debug.Log("HEALTH CHANGED, ITS NOW " + health.current);
    }

    public void OnZeroHealth()
    {
        isDead = true;
        Destroy(gameObject);
    }


}
