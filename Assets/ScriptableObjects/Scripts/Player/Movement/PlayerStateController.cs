using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static GodStateScript;

public class PlayerStateController : MonoBehaviour
{
    public IState currentState { get; private set; }

    /// COMPONENT REFRENCES 
    public Rigidbody2D rb;

    /// ALL STATE REFRENCES 
    public NormalState normalState;

    /// ALL DATA CHECKS 
    public bool isGrounded { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.gravityScale = 1.0f;
        normalState = new NormalState(this);
    }

    private void Update()
    {
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


    public void HandleTransition()
        // constantly looking for checks that lead to a state change
        // EX: if player hits left shift, dash happens and player trans from normal state to dash state
        // WORKS IN TANDOM WITH CHANGESTATE METHOD
    {
        // Handles what causes a state to switch from one to the other 
    }




}