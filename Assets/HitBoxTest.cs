using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// THIS SCRIPT IS ME LEARNING HOW TO PROPERLY PROGRAM MELEE WEAPONS.
/// </summary>
public class HitBoxTest : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Vector2 boxSize;
    public float windupDelay;
    public float coolDownTimer = .2F;
    public bool canAttack;
    float knockBackForce = 5f;
    Collider2D[] hits;
    public float directionY;
    public float directionX;

    public void Start()
    {
        canAttack = true;
    }

    public void Update()
    {
        if (InputManager.Instance.threePressed && canAttack == true)
        {
            StartCoroutine(SwingAttack());
        }

    }

    public void UseHitbox()
    {
        hits = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 0f, enemyLayer);

        if (hits == null)
        {
            return;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPoint.position, boxSize);
    }

    public IEnumerator SwingAttack()
    {
        canAttack = false;

        HashSet<Collider2D> EnemiesHit = new HashSet<Collider2D>();

        float timer = 0f;
        float swingDurationTimer = 0.5f;
        

        yield return new WaitForSeconds(windupDelay);
        
        while (timer < swingDurationTimer )
        { // HITBOX WILL BE PRESENT AS LONMG AS TIMER IS LESS THAN SWING DURATION 
            
            UseHitbox();
            timer += Time.deltaTime;

            foreach (var hits in hits)
            {
                if (EnemiesHit.Contains(hits)) continue;
                ///IF ENEMY IS ALREADY IN HASH SET, SKIP REST OF BLOCK

                Vector2 direction = (hits.transform.position + - attackPoint.position).normalized;
                direction.y *= directionY;
                direction.x *= directionX;


                EnemiesHit.Add(hits);

                Rigidbody2D rb = hits.GetComponent<Rigidbody2D>();
                if (rb !=null)
                {
                    rb.AddForce(direction * knockBackForce, ForceMode2D.Impulse);
                }

                Debug.Log(hits.name);
            }
            yield return null;
        }
        yield return new WaitForSeconds(coolDownTimer);
        canAttack = true;
    }

}
