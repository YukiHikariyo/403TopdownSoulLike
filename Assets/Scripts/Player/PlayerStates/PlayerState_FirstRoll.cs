using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/FirstRoll", fileName = "PlayerState_FirstRoll")]
public class PlayerState_FirstRoll : PlayerState
{

    public override void Enter()
    {
        Debug.Log("First");
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        SetAnimator_OnStart();
        playerAnimator.Play("FirstRoll");
        FaceDir = playerController.MoveAxis;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //翻滚逻辑
        playerController.FastRoll(FaceDir, StateDuration/AnimationLength);
        //
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.Roll)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_SecondRoll));
            }
        }

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
    }
}
