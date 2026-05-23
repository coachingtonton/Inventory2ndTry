using UnityEngine;
using System.Collections;

public interface IState
{
    // Controls what happens when entered new state
    void Enter();
    // update that makes the state have its qualities
    void Update();
    // cleanup for when leaving this state and transition to a new one 
    void Exit();
}
