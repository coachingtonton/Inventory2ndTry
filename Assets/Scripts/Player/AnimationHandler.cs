using UnityEngine;
using static GodStateScript;


/// <summary>
/// 
/// EXISTS TO HAVE CLEAN SPACE FOR PLAYER TO TRANSITION FROM ONE ANIM TO ANOTHER
/// WILL REFRENCE PLAYER STATE CONTROLLER FOR PROPERTIES
/// 
/// </summary>

public class AnimationHandler : MonoBehaviour
{
    [Header("CurrentAnimation")]
    public string currentAnimation { get; private set; }

    private PlayerStateController player;
    private Animator anim;
    //public float timerSinceWallTouch;
    //public float animationTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<PlayerStateController>();
    }

    private void Update()
    {
        HandleAnimimationTransitions();
        //timerSinceWallTouch -= Time.deltaTime;

    }

    private void HandleAnimimationTransitions()
    {
        switch (player.currentState)
        {
            case NormalState:

                if (InputManager.Instance.moveInput != 0)
                    ChangeAnimation("Running");
                else
                    ChangeAnimation("Idle");

                break;

            case FallingState:

                // Lock animation changes briefly after a wall jump
                //if (timerSinceWallTouch > 0)
                    //return;

                if (player.isRising)
                    ChangeAnimation("Jump");
                else
                    ChangeAnimation("AirFalling");

                break;

            case ClingState:

                //if (InputManager.Instance.jumpPressed && player.isTouchingWall)
                //{
                //    ChangeAnimation("WallJump");
                //    timerSinceWallTouch = animationTime;
                //}
                 if (player.holdingIntoWall && player.isTouchingWall)
                {
                    ChangeAnimation("ClampWall");
                }

                break;

            case TouchingWallStates:

                //if (InputManager.Instance.jumpPressed && player.isTouchingWall)
                //{
                //    ChangeAnimation("WallJump");
                //    timerSinceWallTouch = animationTime;
                //}
                 if (player.holdingIntoWall && player.isTouchingWall)
                {
                    ChangeAnimation("ClampWall");
                }

                break;

            case WallSlideState:

                //if (InputManager.Instance.jumpPressed && player.isTouchingWall)
                //{
                //    ChangeAnimation("WallJump");
                //    timerSinceWallTouch = animationTime;
                //}
                 if (player.isTouchingWall)
                {
                    ChangeAnimation("WallSlide");
                }

                break;
        }
    }

    public void ChangeAnimation(string animationInput)
    {
        //Guards animation froim repeating every frame, allows anim to play out
        //IF desired animation input is new then currentanimation switches to new anim 
        // HANDLE TRANSITIONS will use this method 
        if (currentAnimation == animationInput) return;
        anim.Play(animationInput);
        currentAnimation = animationInput;
    }

    bool CurrentAnimFinished()
    {
        var currentAnimation = anim.GetCurrentAnimatorStateInfo(0); 
        return currentAnimation.normalizedTime >= 1f;
    }


}
