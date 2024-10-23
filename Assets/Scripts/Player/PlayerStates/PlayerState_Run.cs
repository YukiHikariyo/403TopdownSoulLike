using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/Run", fileName = "PlayerState_Run", order = 22)]
public class PlayerState_Run : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerAnimator.Play("Run");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        SetAnimator_OnStart_Input();
        FaceDir = playerController.MoveAxis;
        if (!playerInput.IsRun)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Move));
        }

        if (playerInput.Magic_1)
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
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}