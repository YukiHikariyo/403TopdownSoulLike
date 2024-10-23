using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/PlayerState/UseManaBottle",fileName ="PlayerState_UseManaBottle",order =20)]

public class PlayerState_UseManaBottle : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        playerAnimator.Play("UseBottle");
    }

    public override void Exit()
    {
        base.Exit();
        if (PackageManager.Instance.ConsumeManaBottle())
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
