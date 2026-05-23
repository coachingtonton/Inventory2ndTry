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
    public float chargeTimer;

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
        UpdateHitboxDirection();
        DetermineCurrentHitboxDirection(CurrentAttack);   // for hitbox/gizmo preview
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

        if (canAttack && InputManager.Instance.fireHeld)
        {
            chargeTimer += Time.deltaTime;
        }
        else if (canAttack && InputManager.Instance.fireReleased )
        {
            MeleeItemSO primaryOrSecondary = CurrentAttack; //Stores wether attack was Primary Or Secondary 

            chargeTimer = 0f;
            StartCoroutine(SwingAttack(primaryOrSecondary)); //This is what causes an attack to happen
            //PrimaryOrSecondaryFire are stored as ATTACK
            // SWING ATTACK passes attack to all other methods 
        }
        Debug.Log($"called | held: {InputManager.Instance.fireHeld} | released: {InputManager.Instance.fireReleased} | timer: {chargeTimer}");
    }

    public IEnumerator SwingAttack(MeleeItemSO primaryOrSecondary)
    {
        ///SWING ATTACK USES ATTACK DURATION
        ///ATTACK DURATION IS DETERMINED BY AMOUNT OF TIME MOUSE IS HELD FOR 
        ///CURRENT ATTACK USES THE SECONDARYATTACK SCRIPT ATTATCHED TO MAIN GAME OBJECT

        canAttack = false;
        coolDownTimer = primaryOrSecondary.cooldown;
        //SETS COOLDOWN TIMER AND ALSO TURNS CAN ATTACK TO FALSE
        // cooldowntimer is a check insidfe primary fire, UPDATE is ticking down the timer

        HashSet<Collider2D> enemiesHit = new HashSet<Collider2D>();
        // HASH SET DAMAGES ENEMY CORRECT AMOUNT OF TIMES INSTEAD
        // OF DAMAGIN EVERY FRAME ENEMY IS INSIDE MELEE HITBOX

        float timer = 0f;

        while (timer < primaryOrSecondary.attackDuration)
        {   //WHILE TIMER IS LESS THAN ATTACK DURATION, hitbox will be active 
            //And all damage, knockback effects and status affects will be applied

            ActivateHitbox();
            timer += Time.deltaTime;
            isCurrentlySwinging = true;

            foreach (Collider2D hit in hits)
            {///Goes thru every enemylayer inside activated hitbox HITS ARRAY and applies 
             ///attack methods and verbs from meleeitemSO.
             ///Keeps track of enemies hit and allows meleeitem 
             ///to function as intended 

                if (enemiesHit.Contains(hit)) continue;
                //IF ENEMY IS ALREADY PRESENT IN HASH SET SKIP REST OF BLOCK
                //PREVENTS SAME ENEMY BEING DAMAGED OVER DURATION OF HITBOX BEING ACTIVE

                enemiesHit.Add(hit);
                //ADDS ENEMY LAYER TO HASHSET

                TryHit(hit, primaryOrSecondary);
                //RUNS DAMAGE AND HITSOP
            }
            yield return null;
        }
        //ISCURRENTLYSWINGING EXISTS FOR LOCKING ANIMATION AND HITBOX 
        isCurrentlySwinging = false;

        yield return new WaitForSeconds(coolDownTimer);//I refrence cooldownTimer instead of Meleedata.cooldowntimer 
        // in planning for scaleability, buffs and whatever else
        canAttack = true;
    }

    void TryHit(Collider2D hit, MeleeItemSO primaryOrSecondary)
    {
        ///APPLIES DAMAGE TO THE ENEMY AND APPLIES HITSTOP IF REQUIRED
        if (hit.TryGetComponent<IDamageable>(out IDamageable enemyIdamageable))
        {
            enemyIdamageable.TakeDamage(primaryOrSecondary.damage);

            if (primaryOrSecondary.hasHitStop)
            {
                HitStop.instance.Freeze(primaryOrSecondary.hitStopDuration);
            }
        }
        ///APPLIES DAMAGE TO THE ENEMY AND APPLIES HITSTOP IF REQUIRED

        if (primaryOrSecondary.hasKnockback && hit.TryGetComponent<Rigidbody2D>(out Rigidbody2D enemyRB))
        {   //IF MELEE HAS KNOCKBACK, RIGID BODY WILL BE REFRENCED
            // FROM HASH SET AND WILL HAVE KNOCKBACK APPLIED 
            ApplyKnockBack(enemyRB, hit, primaryOrSecondary);
        }
        Debug.Log(hit.name);
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

    public void ApplyKnockBack(Rigidbody2D enemyRB, Collider2D hit, MeleeItemSO primaryOrSecondary)
    {
        ///USES CURRENT DIRECTION TO DETERMINE KNOCKBACK OF WEAPON
        ///WEAPON NEEDS VARIABLE KNOCKBACK SO JUGGLING FEELS GOOOD
        Vector2 force = GetKnockbackForDirection(primaryOrSecondary);

        enemyRB.linearVelocity = Vector2.zero;
        enemyRB.AddForce(force, ForceMode2D.Impulse);
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

    Vector2 GetKnockbackForDirection(MeleeItemSO primaryOrSecondary)
    {
        /// SETS THE PROPER KNOCKBACK POWER IN RELATION TO THE HITBOX DIRECTION 
        switch (currentDirection)
        {
            case HitboxDirection.Overhead: return primaryOrSecondary.knockbackOverhead;
            case HitboxDirection.Lower: return primaryOrSecondary.knockbackLower;
            case HitboxDirection.Left: return primaryOrSecondary.knockbackLeft;
            case HitboxDirection.Right: return primaryOrSecondary.knockbackRight;
            default: return Vector2.zero;
        }
    }

    public void UpdateHitboxDirection()
    {
        ///THIS SCRIPT DETERMINES HITBOX DIRECTION BASED ON AIM POINTS ROTATION ALONG Z AXIS 
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

    public void DetermineCurrentHitboxDirection(MeleeItemSO primaryOrSecondary)
    {
        if (primaryOrSecondary == null) return;

        switch (currentDirection)
        {
            //TAKES in SO's hitbox data so player can have directional swings

            case HitboxDirection.Left:
                hitboxSize = primaryOrSecondary.hitboxSizeSideXSide;
                hitboxOffset = primaryOrSecondary.hitboxOffsetLeft;
                break;
            case HitboxDirection.Right:
                hitboxSize = primaryOrSecondary.hitboxSizeSideXSide;
                hitboxOffset = primaryOrSecondary.hitboxOffsetRight;
                break;
            case HitboxDirection.Overhead:
                hitboxSize = primaryOrSecondary.hitboxSizeOVERHEAD;
                hitboxOffset = primaryOrSecondary.hitboxOffsetOverhead;
                break;
            case HitboxDirection.Lower:
                hitboxSize = primaryOrSecondary.hitboxSizeLower;
                hitboxOffset = primaryOrSecondary.hitboxOffsetLower;
                break;
        }
    }

    public MeleeItemSO CurrentAttack
    {
        get
        {
            if (chargeTimer > meleeData.chargeAttackThreshold)
            {
                return meleeData.secondaryAttackScript;
            }
            else
            {
                return meleeData.primaryAttackScript;
            }
        }
    }

}