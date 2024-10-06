using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/PlayerState/Idle",fileName ="PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerAnimator.Play("Idle");
    }

    public override void Exit() 
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        SetAnimator_Update();

        if (playerInput.WantsMove)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Move));
        }

        if(playerInput.LightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));

        //等待添加耐力限制
        if(playerInput.Roll)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.Idle();
    }
}
