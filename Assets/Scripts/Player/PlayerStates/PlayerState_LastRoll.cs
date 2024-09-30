using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/SecondRoll", fileName = "PlayerState_SecondRoll")]
public class PlayerState_LastRoll : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        SetAnimator_OnStart();
        playerAnimator.Play("LastRoll");
        FaceDir = playerController.MoveAxis;
    }

    public override void Exit()
    {
        base.Exit();
        //翻滚逻辑
        playerController.SlowRoll(FaceDir, StateDuration / AnimationLength);
        //
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
