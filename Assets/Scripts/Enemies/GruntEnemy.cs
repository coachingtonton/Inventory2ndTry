using System.Collections;
using UnityEngine;
using static GruntEnemyStates;

/// <summary>
/// 
/// GRUNT ENEMY //// this enemy will run at the player, climb walls to attack and get in player's face 
/// THIS SCRIPT HOLDS FUCKING DATA AND SHARED METHODS THAT THE CORRESPONDING GOD STATESCRIPT WILL USE 
/// 
/// </summary>

public class GruntEnemy : MonoBehaviour, IDamageable
{
    public enum GruntState { Idle, Chase, Attack }
    public IState currentState { get; private set; }

    /// COMPONENETS
    public Rigidbody2D rb;
    public PlayerStateController playerPosition;
    private BoxCollider2D col;
    private Transform playerTransform;
    private Health health;
    public EnemySO enemySO;

    /// STATE REFRENCES 
    public GruntIdleState idleState;
    

    /// SURFACE COMPONENTS
    [SerializeField] Transform wallCheckFront;
    [SerializeField] Transform wallCheckRear;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    /// TIMERS 

    public float idleWalkTimer;
    public float patrolTimer;
    public float freezeEnemyInputTimer;
    public float freezeEnemyInputDuration;

    /// MOVEMENT
    public float currentSpeed;

    /// SURFACE CHECKS 
    public bool hitWall { get; private set; }
    public bool noGroundAhead { get; private set; }
    public bool isTouchingWall { get; private set; }
    public bool wallTouchingRear { get; private set; }
    public bool wallTouchingFront { get; private set; }
    public bool isChasingPlayer { get; private set; }
    public bool isGrounded { get; private set; }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        health = GetComponent<Health>();
    }

    public void Start()
    {
        InitializeAllStatesOnStart();
        currentState = idleState;
        currentState.Enter();
    }

    private void Update()
    {
        //Debug.Log("player enemy distance is "+ PlayerDistance);
        //HandleTransitions();
        currentState.Update();
    }

    public void ChangeState(IState newState) // preforms transition from one state to another
    {
        if (currentState == newState) return;
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void HandleTransitions() // WHAT CONDITIONS NEEDS To be present for changeState To occur
    {

    }

    public void TakeDamage(float amount)
    {
        health.TakeDamage(amount);
    }

    public float PlayerDistance 
    {
        get { return Vector2.Distance(transform.position, playerTransform.position); }
    }

    public void ApplyHorizontalMovement(int direction, float accel, float decel) 
    {
        //While idle/patrolling, Grunt will use patrol speed
        currentSpeed = isChasingPlayer ? enemySO.pursueSpeed : enemySO.patrolSpeed;

        // used when states require p[layer input to be temporarily locked 
        if (freezeEnemyInputTimer > 0) return; // stops enemy from moving 

        //METHOD FOR APPLYING HORIZONTAL MOVEMENT CONTEXTUALLY 
        // can be used for air, water or whateeever 
        float targetSpeed = direction * currentSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        float rate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : decel;//IF VELOCITY X IS MORE THAN 0.01 THAN RATE = ACCEL

        float force = speedDiff * rate;
        rb.AddForce(Vector2.right * force, ForceMode2D.Force);

    }

    public void SurfaceChecks()
    {
        //All these checks are assigned to child gameobjects on player
        // rear and front work because when player flips so do adjacent checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
        wallTouchingFront = Physics2D.OverlapCircle(wallCheckFront.position, 0.15f, groundLayer);
        wallTouchingRear = Physics2D.OverlapCircle(wallCheckRear.position, 0.15f, groundLayer);
        isTouchingWall = wallTouchingFront || wallTouchingRear;

    }

    public void InitializeAllStatesOnStart()
    {
         idleState = new GruntIdleState(this);
    }

    public IEnumerator idleShuffle()
    {
        while (true) ///this coroutine will be called once on enter and loop forever, no need for update
        {
            Debug.Log("started loop1");
            int randomDirection = Random.Range(0, 2) == 0 ? -1 : 1; // picks -1 or 1 for movement
            int randomWaitTime = Random.Range(enemySO.idleTimerLow, enemySO.idleTimerHigh); //Sporadic wait time for idle stance
            
            yield return new WaitForSeconds(randomWaitTime); //will play idle animation here

            //picks a duration of time to walk for and stores it in idlewalktimer
            //IdlewalkTImer will be subtracted inside this while loop
            float randomWalkTime = Random.Range(enemySO.randomShuffleTimeLOW, enemySO.randomShuffleTimeHIGH);
            idleWalkTimer = randomWaitTime;

            Debug.Log("idle walk timer is " + idleWalkTimer);

            while (idleWalkTimer > 0) // Grunt continues to move while timer is above 0
            {
                ApplyHorizontalMovement(randomDirection, enemySO.groundAccel, enemySO.groundDecel);
                idleWalkTimer -= Time.deltaTime;
                yield return null;
            }
        }
    }

}
