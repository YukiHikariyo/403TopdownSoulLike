using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/FirstRoll", fileName = "PlayerState_FirstRoll",order = 2)]
public class PlayerState_FirstRoll : PlayerState
{

    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;

        SetAnimator_OnStart();
        playerAnimator.Play("FirstRoll");

        FaceDir = playerController.MoveAxis;

        player.damageableIndex = 1;

        if(playerController.RollCount == 0)
            playerController.RollTimer = playerController.RollColdDown;

        playerController.RollCount++;
    }

    public override void Exit()
    {
        base.Exit();
        player.damageableIndex = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerStateMachine.CanAcceptInput)
        {
            player.damageableIndex = 0;
            if (playerInput.Roll && playerController.RollCount < 3)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
            }
        }

        if (IsAnimationEnd)
        {
            //切换至移动
            if (playerInput.WantsMove)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Move));
            }
            //切换至常态
            if (!playerInput.WantsMove)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        //翻滚逻辑
        playerController.FastRoll(FaceDir == Vector2.zero ? Vector2.right : FaceDir, StateDuration / AnimationLength);
        //
    }
}
