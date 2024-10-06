using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/LastRoll", fileName = "PlayerState_LastRoll")]
public class PlayerState_LastRoll : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Last");
        playerStateMachine.CanAcceptInput = false;
        SetAnimator_OnStart();
        playerAnimator.Play("LastRoll");
        FaceDir = playerController.MoveAxis;
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
                Debug.Log("Move");
                playerStateMachine.SwitchState(typeof(PlayerState_Move));
            }
            //切换至常态
            if (!playerInput.WantsMove)
            {
                Debug.Log("Idle");
                playerStateMachine.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        //翻滚逻辑
        playerController.SlowRoll(FaceDir, StateDuration / AnimationLength);
        //
    }
}
