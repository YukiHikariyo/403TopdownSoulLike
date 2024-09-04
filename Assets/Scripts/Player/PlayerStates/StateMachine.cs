using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    IState currentState;


    protected void Update()
    {
        currentState.LogicUpdate();
    }

    protected void FixedUpdate()
    {
        currentState.PhysicUpdate();
    }

    protected void SwitchOn(IState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    public void SwitchState(IState newState)
    {
        currentState.Exit();
        SwitchOn(newState);
    }
    
}
