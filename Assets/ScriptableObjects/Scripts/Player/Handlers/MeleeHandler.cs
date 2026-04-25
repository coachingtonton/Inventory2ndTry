using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// THIS IS VIBE CODED, I AM 8 HOURS INTO THIS AND WANT TO MAKE PROGRESS
/// WILL NEED TO LEARN FULLY LATER ONCE I FLESH OUT THIS SYSTEM MORE
/// </summary>

public class MeleeHandler : WeaponBase
{
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    MeleeItemSO meleeData;
    float coolDownTimer;
    bool isAttacking;

    private void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public override void Equip(ItemSO item)
    {
        itemData = item;
        meleeData = item as MeleeItemSO;
        coolDownTimer = 0f;
    }

    public override void PrimaryFire()
    {
        if (meleeData == null) return;
        if (coolDownTimer > 0f) return;
        if (isAttacking) return;

        coolDownTimer = meleeData.cooldown;
        StartCoroutine(SwingAttack());
    }

    IEnumerator SwingAttack()
    {

        Debug.Log("swing");
        isAttacking = true;

        //WIND UP DELAY BEFORE HITBOX IS ACTIVATED
        yield return new WaitForSeconds(meleeData.windupHitBoxDelay);

        // ACTIVE HITBOX FRAMES 
        float timer = 0f;
        HashSet<Collider2D> alreadyHit = new HashSet<Collider2D>(); // HASH SET IS A LIST THAT CANMNOT HAVE DUPLICATES ON IT
        // WITHOUT HASHSET AN ENEMY WILL REPEATEDLY TAKE DAMAGE EVERY FRAME WHILE IN CONTACT WITH MELEE HITBOX 

        while (timer < meleeData.hitboxDuration)
        {
            //CREATES HITBOX INFRONT OF PLAYER 
                        Vector2 center = (Vector2)attackPoint.position 
                + (Vector2)attackPoint.right * meleeData.hitboxOffset.x
                + (Vector2)attackPoint.up * meleeData.hitboxOffset.y;

            //THIS IS AN ARRAY THAT KEEPS STORES ALL ENEMY LAYERS HIT INSIDE HITBOX DURING SWING
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, meleeData.hitboxSize, 0f, enemyLayer);

            foreach (Collider2D b in hits)
            {
                //SKIPS GAMEOBJECT IF COLLIDER WAS ALREADY HIT
                //IF GAMEOBJECT IS APART OF HASHSET ALREADY HIT ITLL BE SKIPPED
                if (alreadyHit.Contains(b)) continue; ///IF GAMEOBJECT ALREADY HIT, SKIP IT 

                alreadyHit.Add(b);///FIRST TIME HITTING THIS ENEMY, ADD THEM TO HASHLIST SO WE DONT HIT AGAIN 

                b.GetComponent<IDamageable>()?.TakeDamage(meleeData.damage);//yARDY know run takedamage

                Rigidbody2D bRB = b.GetComponent<Rigidbody2D>();//GETS FRESHLY HIT GAMEOBJECTS RB
                //FOR THE KNOCKBACK 
                if (bRB != null)
                {
                    //CALCULATES DIRECTION TO KNOCK ENEMY 
                    Vector2 knockBackDir = (b.transform.position - attackPoint.position).normalized;
                    bRB.AddForce(knockBackDir * meleeData.knockbackForce, ForceMode2D.Impulse);
                }

                //ADD SYSTEM FOR APPLYING BUFFS LATER
            }
            timer += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
    }



    public override void SecondaryFire() { }
    public override void Reload() { }

    //SHOWS HITBOX PURE VIBE CODE I DONT UNDERSTAND 
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null || meleeData == null) return;
        Gizmos.color = Color.red;
        Vector3 center = attackPoint.position
            + attackPoint.right * meleeData.hitboxOffset.x
            + attackPoint.up * meleeData.hitboxOffset.y;
        Gizmos.DrawWireCube(center, meleeData.hitboxSize);
    }
}
