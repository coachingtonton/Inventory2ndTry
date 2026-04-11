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
    //NORMAL STATE NORMAL STATE NORMAL STATE 
    public class NormalState : IState                   
    {
        private PlayerStateController player;

        public NormalState(PlayerStateController player)
        {
            this.player = player;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }           // END OF NORMAL STATE


}