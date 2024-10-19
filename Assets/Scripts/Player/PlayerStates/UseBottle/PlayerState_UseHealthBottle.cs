using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/UseHealthBottle", fileName = "PlayerState_UseHealthBottle", order = 18)]
public class PlayerState_UseHealthBottle : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        SetAnimator_OnStart();
        playerAnimator.Play("UseBottle");
    }

    public override void Exit()
    {
        base.Exit();
        if (PackageManager.Instance.ConsumeHealthBottle())
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Successful_Health));
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.Idle();
    }
}
