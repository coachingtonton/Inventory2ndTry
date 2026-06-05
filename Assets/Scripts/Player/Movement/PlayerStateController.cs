using System.Collections;
using UnityEngine;
using static GodStateScript;

public class PlayerStateController : MonoBehaviour
{
    public IState currentState { get; private set; }

    private Coroutine repeatingWallJumpCoroutine;

    #region FIELDS



    [Header("Movement - Ground")]
    public float maxSpeed = 10f;
    public float groundAcceleration = 30f;
    public float groundDeceleration = 40f;

    [Header("Movement - Air")]
    public float jumpForce = 12f;
    public float airControl = 15f;
    public float airDrag = 10f;
    public float fallMultiplier = 2.5f;
    public float maxFallSpeed = -15f;
    public int DBLjumpsRemaining = 3;
    public int currentJumps;
    public int regularGravity;

    [Header("Movement - Float")]
    public float floatGravity = 0.1f;
    public float floatHorizontalSpeed = 1.2f;

    [Header("Movement - Wall")]
    public float wallSlideSpeed = -2f;
    public float wallSlideMaxSpeed = -10f;

    public float climbJumpForceX = 3f;
    public float climbJumpForceY = 18f;
    public float powerKickForceX = 18f;
    public float powerKickForceY = 10f;
    public float diagonalJumpForceX = 10f;
    public float diagonalJumpForceY = 10f;
    public float wallDetachTime = 0.15f;

    [Header("Movement - Dodge Roll")]
    public float dodgeRollForce = 15f;
    public float dodgeRollDuration = 0.4f;

    [Header("Movement - Feel Polish")]
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    public float apexHangThreshold = 1f;
    public float apexHangGravity = 0.4f;

    [Header("Input")]
    public float moveInput;
    public float moveInputY;
    public bool isFloatingTabHeld;

    [Header("Timers")]
    public float coyoteTimer;
    public float jumpBufferTimer;
    public float wallDetachTimer;
    public float dodgeRollTimer;
    public float freezePlayerInputTimer;
    public float freezePlayerInputDuration;

    public float wallSlideTimer;
    public float wallSlideTimerDuration;

    //public float wallClingTimer;
    //public float wallClingDuration = 0.03f;

    [Header("Status")]

    public bool isDodgeRolling;
    public bool isGravityFlipped;
    public bool holdingIntoWall;
    public bool repeatingWallJump;
    public bool kicksOffWall;
    public bool diggingInWall;

    /// COMPONENT REFERENCES
    [SerializeField] Transform wallCheckFront;
    [SerializeField] Transform wallCheckRear;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    public Rigidbody2D rb;

    /// STATE REFERENCES
    public WallSlideState wallSlideState;
    public FallingState fallingState;
    public NormalState normalState;
    public JumpingState jumpingState;
    public FloatState floatState;
    public TouchingWallStates touchingWallState;
    public ClingState clingState;

    /// DATA CHECKS
    public bool isGrounded { get; private set; }
    public bool wallTouchingRear { get; private set; }
    public bool wallTouchingFront { get; private set; }
    public bool isTouchingWall { get; private set; }
    public bool isRising { get; private set; }
    public bool isFallinginAir { get; private set; }

    public float wallJumpDirection;
    #endregion FIELDS


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentJumps = DBLjumpsRemaining;
        InitializeAllStatesOnStart();

        rb.gravityScale = rb.gravityScale;
        currentState = normalState;
        currentState.Enter();//manualy calls enter on first state
    }

    private void Update()
    {
        if (freezePlayerInputTimer > 0)//TIMER FOR PLAYER FREEZE 
            freezePlayerInputTimer -= Time.deltaTime;

        ResetJumpsIfGrounded();
        isFloatingTabHeld = InputManager.Instance.tabHeld;//simple isfloatingscript
        moveInput = InputManager.Instance.moveInput;
        SurfaceChecks();
        HandleTransitions();
        currentState.Update(); //Runs current states logic 
    }

    public void ChangeState(IState newState)
    {
        // Used to transition from state to state
        // POOLYMOORPHIZUMM BRUHH 
        if (currentState == newState) return;
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void HandleTransitions()
    { // SIMPLE switch for what causes a transition from one state to another
      // PLAYER STARTS IN NORMAL STATE  
      // 

        switch (currentState)
        {
            /// =============NORMAL TRANSITIONS ============
            case NormalState:

                if (InputManager.Instance.jumpPressed && isGrounded)
                    ChangeState(jumpingState);
                if (!isGrounded)
                    ChangeState(fallingState); // walked off a ledge
                if (isFloatingTabHeld)
                    ChangeState(floatState);
                if ((isTouchingWall && !isGrounded))
                    ChangeState(touchingWallState);
                break;


            /// =============JUMPING TRANSITIONS ============
             
            case JumpingState:
                ChangeState(fallingState);
                if (isTouchingWall)
                    ChangeState(touchingWallState);
                break;



            /// =============FLOATING TRANSITIONS ============
            case FloatState:
                if (isFloatingTabHeld == false)
                    ChangeState(fallingState);
                if (isGrounded)
                    ChangeState(normalState);
                if (isTouchingWall)
                    ChangeState(touchingWallState);
                break;


            /// =============FALLING TRANSITIONS ============
            case FallingState:
                if (isGrounded == true)
                    ChangeState(normalState);
                if (isFloatingTabHeld == true)
                    ChangeState(floatState);
                if (isTouchingWall)
                    ChangeState(touchingWallState);
                break;

            /// ============= TOUCHING WALL STATE ============
            case TouchingWallStates:

                if (isGrounded)
                    ChangeState(normalState);
                else if (!isTouchingWall && !repeatingWallJump)
                    ChangeState(fallingState);
                else if (isTouchingWall && holdingIntoWall)
                    ChangeState(clingState);          // grabbing → cling
                else if (!holdingIntoWall && isTouchingWall)
                    ChangeState(wallSlideState);      // not grabbing → slide
                break;

            /// ============= CLING STATE CLING STATE ============
            case ClingState:
                if (isGrounded)
                    ChangeState(normalState);
                else if (!isTouchingWall)
                    ChangeState(fallingState);            // wall ended / let go off edge
                else if (!holdingIntoWall)
                    ChangeState(wallSlideState);          // released grab → slide down
                else if (InputManager.Instance.jumpPressed)
                    ChangeState(touchingWallState);       // jump off → wall jump logic
                break;

            /// ============= WALL SLIDING STATE ============
            case WallSlideState:
                if (!isTouchingWall)
                    ChangeState(fallingState);
                if (isGrounded)
                    ChangeState(normalState);
                if (InputManager.Instance.jumpPressed)
                    ChangeState(touchingWallState);
                if (moveInput != 0 && !holdingIntoWall)  // tap opposite direction = leave wall
                    ChangeState(fallingState);
                if (holdingIntoWall)
                    ChangeState(clingState);
                break;
        }
    }

    public void InitializeAllStatesOnStart()
    {
        wallSlideState = new WallSlideState(this);
        fallingState = new FallingState(this);
        normalState = new NormalState(this);
        jumpingState = new JumpingState(this);
        floatState = new FloatState(this);
        touchingWallState = new TouchingWallStates(this);
        clingState = new ClingState(this);
    }

    public void SurfaceChecks()
    {
        //All these checks are assigned to child gameobjects on player
        // rear and front work because when player flips so do adjacent checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
        wallTouchingFront = Physics2D.OverlapCircle(wallCheckFront.position, 0.15f, groundLayer);
        wallTouchingRear = Physics2D.OverlapCircle(wallCheckRear.position, 0.15f, groundLayer);
        isTouchingWall = wallTouchingFront || wallTouchingRear;

        holdingIntoWall = isTouchingWall && moveInput == 1 || isTouchingWall && moveInput == -1;
        if (isTouchingWall)
            wallJumpDirection = wallTouchingFront ? -1f : 1f;

        if (rb.linearVelocity.y > 0.01f)
            { isRising = true; isFallinginAir = false; }
        else
            {isRising = false; isFallinginAir = true; }

    }

    public void FlipSpriteToPlayerInput()
    {   //SIMPLPE player flip script, ALSO Records player input horizontal
        // Put the instance in a variable to make it look cleaner :33333

        if (moveInput == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void ResetJumpsIfGrounded()
    {// PROTOTYPE SYSTEM FOR RESETTING JUMPS FOR NOW
        if (isGrounded == true)
        {
            currentJumps = DBLjumpsRemaining;
        }
    }

    public void ApplyHorizontalMovement(float accel, float decel)
    {
        if (freezePlayerInputTimer > 0) return; // stops player from moving 
        // used when states require p[layer input to be temporarily locked 


        //METHOD FOR APPLYING HORIZONTAL MOVEMENT CONTEXTUALLY 
        // can be used for air, water or whateeever 
        float targetSpeed = moveInput * maxSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        float rate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : decel;//IF VELOCITY X IS MORE THAN 0.01 THAN RATE = ACCEL

        float force = speedDiff * rate;
        rb.AddForce(Vector2.right * force, ForceMode2D.Force);
    }

    public void StartRepeatingWallJump()
    {
        {
            if (repeatingWallJumpCoroutine != null)
                StopCoroutine(repeatingWallJumpCoroutine);
            repeatingWallJumpCoroutine = StartCoroutine(RepeatingWallJumpLoop());
        }
    }

    private IEnumerator RepeatingWallJumpLoop()
    {   // REPEATS UPWARD WALL JUMP WHILE THE PLAYER IS 
        // HOLDING SPACE, 
        // YIELD RETURN NEW WAIT UNTILL PREVENTS 
        // PLAYER FROM ENTERING FALLING STATE ONCE
        // CONTACT WITH THE WALL IS LOST.
        // ONCE SPACE IS LET GO WALL JUMP REPEAT IS FALSE 
        while (InputManager.Instance.spaceHeld)
        {
            if (isTouchingWall)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(
                    -moveInput * climbJumpForceX,
                    climbJumpForceY
                ), ForceMode2D.Impulse);
            }
            yield return new WaitUntil(() => isTouchingWall);
        }
        repeatingWallJump = false;
    }


}