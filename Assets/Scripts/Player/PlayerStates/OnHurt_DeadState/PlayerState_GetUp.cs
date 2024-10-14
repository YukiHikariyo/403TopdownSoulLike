using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/GetUp", fileName = "PlayerState_GetUp", order = 17)]
public class PlayerState_GetUp : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        player.damageableIndex = 1;
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        //继承自上一个状态的方向，不用设置方向参数
        playerAnimator.Play("GetUp");

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
            if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;
        }
        if (IsAnimationEnd)
        {
            if (playerStateMachine.memory == InputMemory.Roll)
                playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
            else
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

    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.Idle();
    }
}
