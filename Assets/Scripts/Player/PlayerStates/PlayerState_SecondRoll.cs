using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/SecondRoll", fileName = "PlayerState_SecondRoll")]
public class PlayerState_SecondRoll : PlayerState
{
    Vector2 FaceDir;
    public override void Enter()
    {
        Debug.Log("2");
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        SetAnimator_OnStart();
        playerAnimator.Play("SecondRoll");
        FaceDir = playerController.MoveAxis;
        playerController.SlowRollStart();
        playerController.LastRollDuration = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (IsAnimationEnd)
        {
            //切换至移动
            if (playerInput.WantsMove)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Move));
            }
            //切换至常态
            if (!playerInput.WantsMove)
                playerStateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.SlowRoll(FaceDir);
    }
}
