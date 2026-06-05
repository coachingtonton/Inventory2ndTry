using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "1000pRun/EnemyStats")]
public class EnemySO : ScriptableObject
{
    [Header("Health")]
    public int maxHealth;

    [Header("Movement")]
    public float groundAccel;
    public float groundDecel;

    [Header("IdleTimers")]
    public int idleTimerLow;
    public int idleTimerHigh;
    public float randomShuffleTimeLOW;
    public float randomShuffleTimeHIGH;


    public float patrolSpeed;
    public float pursueSpeed;
    public float jumpForce;
    public float detectionRange;
    public float attackRange;

    [Header("Combat")]
    public int damage;
    public float attackCooldown;
    public Vector2 knockbackDealt;

    [Header("Drop")]
    public int ammoDropAmount;
    public AmmoType ammoDropType;
}
