using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/PlayerState/Dead",fileName ="PlayerState_Dead",order = 99)]

public class PlayerState_Dead : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        player.damageableIndex = 1;
        playerAnimator.Play("Dead");
    }

    public override void Exit()
    {
        base.Exit();
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
