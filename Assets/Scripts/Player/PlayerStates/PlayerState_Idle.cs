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
        if (playerInput.Magic_2)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Molotov));
        }
        //TODO:耐力限制
        if (playerInput.RightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_Charging));

        if(playerInput.LightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));

        if(playerInput.Roll && playerController.RollCount < 3)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.Idle();
    }
}
