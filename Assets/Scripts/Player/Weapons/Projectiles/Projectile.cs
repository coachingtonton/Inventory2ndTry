using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float lifeTime = 3f;
    private BoxCollider2D BoxCollider2D;
    private Rigidbody2D rb;
    int damage;  // lives on the class, survives forever



    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    //GUNHANDLER IS gonna use this method
    // the arguments are for the specific weapons qualities to 
    // affect the bullets, ammo is the same, weapon determines damage

    /// <summary> This init method is called when a projectile is created
    /// It sets the properties for the projectile
    /// Handler spawns projectile -> Handler feeds SO properties to Init -> Desired projectile is created 
    public void Init(Vector2 direction, float speed, int damage, float gravity)
    {
        rb.linearVelocity = direction * speed;
        this.damage = damage;  // THIS STORES GUNHANDLERS GUNITEM SO'S DAMAGE IN THIS 
        // SCRIPT SO IT CAN BE USED BY OTHER METHODS 
        
        this.rb.gravityScale = gravity;
        //SETS gravity for projectile. 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        //cURRENTL in place to avoid bullet destroying when making player contact

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        // try to find a damageable component on whatever we hit
        // we havent built this yet — placeholder for now

        Debug.Log($"DEALT {damage} TO {other.gameObject}");
            
        Destroy(gameObject);
    }
}
