using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
/// <summary>
/// Melee handler script controls melee
/// has a state machine that takes in SO's parameters for hitbox direction
/// once hitbox direction is determined then the SO feeds the hitbox data for said direction 
/// This results in air combat and lower combat like smash bros without the inputs
/// Also takes in melee SOs knockback power as well as other qualities of indiviual weapons.
/// Hitbox size will be determined by meleeitemSO
/// </summary>

public enum HitboxDirection { Right, Overhead, Left, Lower }

public class MeleeHandler : WeaponBase
{

    HitboxDirection currentDirection;


    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    MeleeItemSO meleeData;
    float coolDownTimer;
    public bool canAttack;
    Collider2D[] hits;
    public float knockBackForce;
    public Transform RotatePoint;
    public Vector2 hitboxSize;
    Vector2 hitboxOffset;
    public bool isCurrentlySwinging = false;

    public override bool weaponIsLockingOtherWeaponSelection() => isCurrentlySwinging;
    // OVERRIDE FOR ISBUSY, WILL BE USED IN EQUIPMENT MANAGER
    // SO CURRENT WEAPON CAN STOP SELECTION FROM OTHER WEAPONS 
    // WHILE DOING AN ANIMATION OR A SWING.

    private void Start()
    {
        canAttack = true;
    }

    private void Update()
    {
        coolDownTimer -= Time.deltaTime;
        //FlipWeaponHitbox();
        UpdateHitboxDirection();
        DetermineCurrentHitboxDirection();
    }

    public override void Equip(ItemSO item)
    {
        //COMMUNICATES WITH EQUIPMENT HANDLER AND GIVES 
        //ALL INFO NEEDED
        itemData = item;
        meleeData = item as MeleeItemSO;
    }

    public override void PrimaryFire()
    {
        if (meleeData == null) return;
        if (coolDownTimer > 0f) return;
        if (!canAttack) return;
        //if player is attacking or timer is zero return

        if (canAttack)
        {
            Debug.Log("playerSwung");
            StartCoroutine(SwingAttack());
        }
    }

    public IEnumerator SwingAttack()
    {
        canAttack = false;
        coolDownTimer = meleeData.cooldown;
        //SETS COOLDOWN TIMER AND ALSO TURNS CAN ATTACK TO FALSE
        // cooldowntimer is a check insidfe primary fire, UPDATE is ticking down the timer

        HashSet<Collider2D> enemiesHit = new HashSet<Collider2D>();
        // HASH SET DAMAGES ENEMY CORRECT AMOUNT OF TIMES INSTEAD
        // OF DAMAGIN EVERY FRAME ENEMY IS INSIDE MELEE HITBOX

        float timer = 0f;


        while (timer < meleeData.attackDuration)
        {   //WHILE TIMER IS LESS THAN ATTACK DURATION, hitbox will be active 
            //And all damage, knockback effects and status affects will be applied

            ActivateHitbox();
            timer += Time.deltaTime;
            isCurrentlySwinging = true;

            foreach (Collider2D hits in hits)
            {///Goes thru every enemylayer inside activated hitbox HITS ARRAY and applies 
                ///attack methods and verbs from meleeitemSO.
                ///Keeps track of enemies hit and allows meleeitem 
                ///to function as intended 

                if (enemiesHit.Contains(hits)) continue;
                //IF ENEMY IS ALREADY PRESENT IN HASH SET SKIP REST OF BLOCK
                //PREVENTS SAME ENEMY BEING DAMAGED OVER DURATION OF HITBOX BEING ACTIVE

                enemiesHit.Add(hits);
                //ADDS ENEMY LAYER TO HASHSET

                if (meleeData.hasKnockback)
                {   //IF MELEE HAS KNOCKBACK, RIGID BODY WILL BE REFRENCED
                    // FROM HASH SET AND WILL HAVE KNOCKBACK APPLIED 
                    Rigidbody2D enemyRB = hits.GetComponent<Rigidbody2D>();
                    ApplyKnockBack(enemyRB, hits);
                }
                Debug.Log(hits.name);
            }
            yield return null;
        }
        //ISCURRENTLYSWINGING EXISTS FOR LOCKING ANIMATION AND HITBOX 
        isCurrentlySwinging = false;

        yield return new WaitForSeconds(coolDownTimer);//I refrence cooldownTimer instead of Meleedata.cooldowntimer 
        // in planning for scaleability, buffs and whatever else
        canAttack = true;
    }

    public void ActivateHitbox()
    {
        Vector2 meleeHitbocksX = hitboxOffset;

        Vector2 hitboxCenter = (Vector2)attackPoint.position + meleeHitbocksX;

        hits = Physics2D.OverlapBoxAll(hitboxCenter, hitboxSize, 0f, enemyLayer);
        // creates a Array that stores ALL ENEMY LAYERS INSIDE THE HITBOX 
        //CREATES THE HITBOX FOR THE MELEE WEAPON AND CHECKS FOR ENEMY LAYERS
        // ENEMY LAYERS WILL BE STORED INSIDE OF HASHSET 
        if (hits == null) return;
    }

    public void ApplyKnockBack(Rigidbody2D enemyRB, Collider2D hits)
    {
        // ADDS KNOCKBACK FORCE DEPENDING ON WHERE PLAYER HITS THE ENEMY
        // MELEE DATA DETERMINES HOW GNARLY KNOCKBACK FORCE IS
        Vector2 direction = (hits.transform.position + -attackPoint.position).normalized;
        direction.y *= meleeData.knockbackForceY;
        direction.x *= meleeData.knockbackForceX;

        enemyRB = hits.GetComponent<Rigidbody2D>();
        
        if (enemyRB !=null)
        {
            enemyRB.AddForce(direction * knockBackForce, ForceMode2D.Impulse);
        }
    }

    void OnDrawGizmos()
    {
        if (meleeData == null || attackPoint == null) return;

        Vector2 offset = hitboxOffset;

        Vector2 hitboxCenter = (Vector2)attackPoint.position + offset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hitboxCenter, hitboxSize);
    }

    //public void FlipWeaponHitbox()
    //{
    //    // IF PLAYER IS AIMING IN OPPOSITE DIRECTION HITBOX WILL FLIP 
    //    float aimPos = RotatePoint.localScale.y;
        /// <summary>
        /// MAY NO LONGER NEED THIS AS I HAVE OFFSET DETERMINED BY THE SO NOW 
        /// </summary>
    //    if (aimPos == 1)
    //    {
    //        hitBoxIsFlipped = true;
    //    }
    //    else
    //        hitBoxIsFlipped = false;
    //}

    public void UpdateHitboxDirection()
    {
        if (!canAttack) return; // prevents player from swiotching swing direction after attacking 

        float angle = RotatePoint.eulerAngles.z;
    
    if (angle >= 52f && angle < 115f)
        currentDirection = HitboxDirection.Overhead;
    else if (angle >= 115f && angle < 245f)
        currentDirection = HitboxDirection.Left;
    else if (angle >= 245f && angle < 295f)
        currentDirection = HitboxDirection.Lower;
    else
        currentDirection = HitboxDirection.Right;
    }

    public void DetermineCurrentHitboxDirection()
    {
        if (meleeData == null) return;

        switch (currentDirection)
        {
            //TAKES in SO's hitbox data so player can have directional swings

            case HitboxDirection.Left:
                hitboxSize = meleeData.hitboxSizeSideXSide;
                hitboxOffset = meleeData.hitboxOffsetLeft;
                break;
            case HitboxDirection.Right:
                hitboxSize = meleeData.hitboxSizeSideXSide;
                hitboxOffset = meleeData.hitboxOffsetRight;
                break;
            case HitboxDirection.Overhead:
                hitboxSize = meleeData.hitboxSizeOVERHEAD;
                hitboxOffset = meleeData.hitboxOffsetOverhead;
                break;
            case HitboxDirection.Lower:
                hitboxSize = meleeData.hitboxSizeLower;
                hitboxOffset = meleeData.hitboxOffsetLower;
                break;
        }
    }
}
