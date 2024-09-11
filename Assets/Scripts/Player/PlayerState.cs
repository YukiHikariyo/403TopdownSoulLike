using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerState : ScriptableObject, IState
{
    # region 组件声明
    protected PlayerInput playerInput;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerController playerController;
    #endregion

    public void Initialization(PlayerInput playerInput,
        PlayerStateMachine playerStateMachine,
        PlayerController playerController)
    {
        this.playerInput = playerInput;
        this.playerStateMachine = playerStateMachine;
        this.playerController = playerController;
    }
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicUpdate()
    {
        
    }
}
