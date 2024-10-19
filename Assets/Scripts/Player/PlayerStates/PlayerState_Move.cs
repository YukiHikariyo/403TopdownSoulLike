using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/Move", fileName = "PlayerState_Move",order = 1)]
public class PlayerState_Move : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerAnimator.Play("Move");
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        SetAnimator_Update();
        //TODO:耐力限制
        if (playerInput.Interaction && playerStateMachine.interactionObj != null)
        {
            if(playerStateMachine.interactionObj.State == false)
            {
                playerStateMachine.interactionObj.State = true;
            }
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
        else if (playerInput.RightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_Charging));

        else if (playerInput.LightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));

        else if (playerInput.Roll && playerController.RollCount < 3)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));

        else if (!playerInput.WantsMove)
            playerStateMachine.SwitchState(typeof(PlayerState_Idle));
        else if (playerInput.UseHealthBottle)
            playerStateMachine.SwitchState(typeof(PlayerState_UseHealthBottle));
        else if (playerInput.UseManaBottle)
            playerStateMachine.SwitchState(typeof(PlayerState_UseManaBottle));

        //TODO:打开背包
    }

    public override void PhysicUpdate()
    {
        playerController.Move();
    }
}
