using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float lifeTime = 3f;
    private BoxCollider2D BoxCollider2D;
    private Rigidbody2D rb;
    int damage;  // lives on the class, survives forever
    float hitStopDuration;
    VFX impactEffect;
    Vector2 enemyKnockback;

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    /// inititializes projectile with data from Scriptable Object
    /// when handlers create a projectile, the current itemData will be the arguments 
    public void Init(Vector2 direction, float speed, int damage, float gravity, float hitStopDuration, VFX impactEffect, Vector2 enemyKnockback)
    { 
        this.hitStopDuration = hitStopDuration;
        rb.linearVelocity = direction * speed;
        this.damage = damage;  
        this.rb.gravityScale = gravity;
        this.enemyKnockback = enemyKnockback;
        this.impactEffect = impactEffect;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) return;

        if (hitStopDuration > 0) //CREATES HITSTOP IF ABILITY NEEDS IT {
        {
            HitStop.instance.Freeze(hitStopDuration);
        }

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null) ///APPLIES KNOCKBACK AND DAMAGE
        {
            damageable.TakeDamage(damage);
            applyEnemyKnockback(other);
        }

        VFXManager.Instance.PlayVFX(impactEffect, transform.position);

        Destroy(gameObject);

        //DEBUGGING 
        Debug.Log($"DEALT {damage} TO {other.gameObject}");
    }

    private void applyEnemyKnockback(Collider2D other)
    {
        // Grabs objects rigidbody and proceeds to apply SOs knockback
        // dir gets the direction of the projectile and forces enemy in direction opposite to that 
        Vector2 dir = rb.linearVelocity.normalized;
        if (other.TryGetComponent<Rigidbody2D>(out var enemyRb))
            enemyRb.AddForce(dir * enemyKnockback, ForceMode2D.Impulse);
    }
}
