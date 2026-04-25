using UnityEngine;

public class HitBoxTest : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Vector2 boxSize;
    Collider2D[] hits;

    public void Update()
    {
        UseHitbox();
    }

    public void UseHitbox()
    {
        if (InputManager.Instance.threePressed)
        {
            hits  = Physics2D.OverlapBoxAll(attackPoint.position, boxSize, 0f, enemyLayer);
        }

        if (hits == null)
        {
            return;
        }

        foreach (Collider2D hit in hits)
        {

            Debug.Log(hit.gameObject.name);
        }


    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPoint.position, boxSize);
    }
}
