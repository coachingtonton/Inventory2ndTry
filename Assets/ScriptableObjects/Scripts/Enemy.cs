using UnityEngine;
using System;


public class Enemy : MonoBehaviour, IDamageable
{
    public ResourcePool health = new ResourcePool(100);


    public void Start()
    {
        Debug.Log(health.current);
        health.OnChanged += DamageTaken;
        health.OnDepleted += OnZeroHealth;
    }


    public void TakeDamage(float amount)
    {
            health.Spend(amount);
    }

    public void HealthChanged()
    {

    }

    public void DamageTaken(float current, float max)
    {
        Debug.Log("current health is " + health.current + " OUT OF " + health.max);
    }

    public void OnZeroHealth()
    {
        Destroy(gameObject);
    }



}
