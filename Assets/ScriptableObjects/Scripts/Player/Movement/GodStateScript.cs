using UnityEngine;
using System.Collections;

/// <summary>
/// hOLD onto ur jimmies bro, here comes the god state script :3 
/// Holds every stateScript
/// Fuck having 500 seperate state script
/// GodState only needs a constrcuctor for PlayerStateController 
/// so it can adjust PlayerData for the NEEDS of the state 
/// PLAYER CONTROLLER HOLDS THE DATA, THE GODSTATE SCRIPT PUTS 
/// THE DATA TO WORK IN ACCORDANCE TO THE STATE PLAYER IS IN 
/// </summary>
public class GodStateScript: MonoBehaviour
{






    /// NORMAL STATE NORMAL STATE NORMAL STATE NORMAL STATE NORMAL STATE NORMAL STATE 
    public class NormalState : IState                   
    {
        private PlayerStateController player;

        public NormalState(PlayerStateController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("ENTERED NORMAL STATE");
        }

        public void Update()
        {

            player.FlipSpriteToPlayerInput();

            //Basic left right movement
            player.rb.linearVelocity = new Vector2(player.moveInput * player.moveSpeed, player.rb.linearVelocity.y);

            //if (player.isGrounded && InputManager.Instance.jumpPressed)
            //{ // jump logic
            //    player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpHeight);
            //}
        }

        public void Exit()
        {
            Debug.Log("exited normal state");

        }
        /// NORMAL STATE NORMAL STATE NORMAL STATE NORMAL STATE NORMAL STATE NORMAL STATE STATE NORMAL STATE NORMAL STATE NORMAL STATE NORMAL STATE STATE NORMAL STATE NORMAL STATE NORMAL STATE NORMAL STATE 
    }






    /// JUMPING STATE JUMPING STATE  JUMPING STATE JUMPING STATE JUMPING STATE JUMPING JUMPING STATE JUMPING STATEJUMPING STATE JUMPING STATEJUMPING STATE JUMPING STATE

    public class JumpingState : IState
    {
        // JUMPING STATE TAKES A GROUNDED PLAYER AND JUMPS THEM
        // THIS STATE GETS TRANSITIONED INTO FALLING
        // FALLING STATE HANDLES DOUBLE JUMPS 
        private PlayerStateController player;
        private int jumpsRemaining;

        public JumpingState(PlayerStateController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            // execute the jump
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpHeight);
            Debug.Log("ENTERED JUMP STATE");
        }

        public void Exit()
        {
            Debug.Log("exited JUMP state");
        }

        public void Update()
        {

        }

    }/// JUMPING STATE JUMPING STATE  JUMPING STATE JUMPING STATE JUMPING STATE JUMPING  JUMPING STATE JUMPING STATE JUMPING STATE JUMPING STATE










    /// FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE
    public class FloatState : IState
    {
        private PlayerStateController player;

        public FloatState(PlayerStateController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            // keep whatever velocity player has on entry
            // this is what carries them upward naturally
            player.rb.gravityScale = player.floatGravity;
            Debug.Log("ENTERED FLOAT STATE");
        }

        public void Exit()
        {
            // restore gravity when leaving
            player.rb.gravityScale = 1f;
            Debug.Log("EXITED FLOAT STATE");
        }

        public void Update()
        {
            // horizontal control while floating, slightly boosted
            player.rb.linearVelocity = new Vector2(
                player.moveInput * player.moveSpeed * player.floatHorizontalSpeed,
                player.rb.linearVelocity.y // preserve Y so momentum carries up
            );

            if (player.rb.linearVelocity.y > 0)
            {// slow the upward momentum gradually so it feels like floating not flying
                player.rb.linearVelocity = new Vector2(
                    player.rb.linearVelocity.x,
                    player.rb.linearVelocity.y * 0.98f // gentle drag on upward momentum
                );
            }
        }
    }  /// FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE FLOAT STATE




    /// FALLING STATE FALLING FALLING FALLING FALLINGFALLING STATE FALLING FALLING FALLING FALLINGFALLING STATE FALLING FALLING FALLING FALLING
    public class FallingState : IState
    {
        // DECIDED TO HAVE FALLING STATE HANDLE DOUBLE JUMPS. MUCH CLEANER 
        //Falling state exists so i can transitions from floating to jumping 
        //withouht jumpingStates ICONIC jump on enter. bridges all in air states
        private PlayerStateController player;
        public int jumpsRemaining;

        public FallingState(PlayerStateController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            jumpsRemaining = player.currentJumps;
            Debug.Log("ENTERED falling STATE");
        }

        public void Exit()
        {
            player.currentJumps = jumpsRemaining;
            Debug.Log("EXITED falling STATE");
        }

        public void Update()
        {
            // HORIZONTAL AIR MOVEMENT 
            player.rb.linearVelocity = new Vector2(player.moveInput * player.moveSpeed 
                * player.airMovementResistence, player.rb.linearVelocity.y);

            if (InputManager.Instance.jumpPressed && jumpsRemaining > 0)
            { // PREFORMS DOUBLE JUMP 
                player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpHeight);
                jumpsRemaining--;
            }
        }
    } /// FALLING STATE FALLING FALLING FALLING FALLINGFALLING STATE FALLING FALLING FALLING FALLINGFALLING STATE FALLING FALLING FALLING FALLING
}