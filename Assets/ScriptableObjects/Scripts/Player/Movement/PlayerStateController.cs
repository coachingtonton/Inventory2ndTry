using System.Collections;
using UnityEngine;
using static GodStateScript;

public class PlayerStateController : MonoBehaviour
{
    public IState currentState { get; private set; }

    [Header("Movement Values")]
    public int jumpHeight = 12;
    public int moveSpeed = 12;
    public float moveInput;
    public float airMovementResistence;

    [Header("IN AIR VALUES")]
    public float floatGravity = 0.1f;
    public float floatHorizontalSpeed = 1.2f;
    public int DBLjumpsRemaining = 3;
    public int currentJumps;


    /// COMPONENT REFRENCES 
    [SerializeField] Transform wallCheckFront;
    [SerializeField] Transform wallCheckRear;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    public Rigidbody2D rb;

    /// ALL STATE REFRENCES 
    public FallingState fallingState;
    public NormalState normalState;
    public JumpingState jumpingState;
    public FloatState floatState;


    /// ALL DATA CHECKS 
    public bool isGrounded { get; private set; }
    public bool wallTouchingRear { get; private set; }
    public bool wallTouchingFront  { get; private set; }
    public bool isTouchingWall { get; private set; }
    public bool isFloatingTabHeld;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentJumps = DBLjumpsRemaining;
        InitializeAllStatesOnStart();

        rb.gravityScale = 1.0f;
        currentState = normalState;
        currentState.Enter();//manualy calls enter on first state

    }

    private void Update()
    {
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
                break;


/// =============JUMPING TRANSITIONS ============
            case JumpingState:
                ChangeState(fallingState);
            break;


/// =============FLOATING TRANSITIONS ============
            case FloatState:
                if (isFloatingTabHeld == false)
                    ChangeState(fallingState);
                if (isGrounded)
                    ChangeState(normalState);
                break;


/// =============FALLING TRANSITIONS ============
            case FallingState:
                if (isGrounded == true)
                    ChangeState(normalState);
                if (isFloatingTabHeld == true)
                    ChangeState(floatState);
                break;
        }
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

    public void InitializeAllStatesOnStart()
    {
        fallingState = new FallingState(this);
        normalState = new NormalState(this);
        jumpingState = new JumpingState(this);
        floatState = new FloatState(this);
    }

    public void ResetJumpsIfGrounded()
    {// PROTOTYPE SYSTEM FOR RESETTING JUMPS FOR NOW
        if (isGrounded == true)
        {
            currentJumps = DBLjumpsRemaining;
        }
    }
}