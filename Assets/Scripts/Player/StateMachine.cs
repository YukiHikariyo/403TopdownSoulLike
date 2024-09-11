using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]IState currentState;
    
    protected Dictionary<System.Type, IState> dict;
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
    
    public void SwitchState(System.Type newState)
    {
        SwitchState(dict[newState]);
    }
}
