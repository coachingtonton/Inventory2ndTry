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

    // Arguments for parameters are present inside Corresponding weaponSO
    public void Init(Vector2 direction, float speed, int damage)
    {
        rb.linearVelocity = direction * speed;
        this.damage = damage;  // saves the parameter into the class field
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // try to find a damageable component on whatever we hit
        // we havent built this yet — placeholder for now
        Debug.Log("Hit: " + other.gameObject.name);

        Destroy(gameObject);
    }
}
