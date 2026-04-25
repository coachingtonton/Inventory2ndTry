using System;
using UnityEngine;

public class Testing : MonoBehaviour 
{
    public Enemy enemy;
    public ResourcePool pool;

    public void Start()
    {
        pool = enemy.health;
    }
    
}

