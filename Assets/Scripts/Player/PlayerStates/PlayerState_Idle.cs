using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/PlayerState/Idle",fileName ="PlayerState_Idle",order = 0)]
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

        if (playerInput.Interaction && playerStateMachine.interactionObj != null)
        {
            if (playerStateMachine.interactionObj.State == false)
            {
                playerStateMachine.interactionObj.State = true;
            }
        }
        else if (playerInput.WantsMove)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Move));
        }
        else if (playerInput.Magic_1)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_FlashBang));
        }
        else if (playerInput.Magic_2)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Molotov));
        }
        else if (playerInput.Magic_3)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_BigLight));
        }
        //TODO:耐力限制
        else if (playerInput.RightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_Charging));

        else if (playerInput.LightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));

        else if (playerInput.Roll && playerController.RollCount < 3)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));

        

        //TODO:打开背包
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.Idle();
    }
}
