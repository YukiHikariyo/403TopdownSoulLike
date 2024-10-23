using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/PlayerState/SmallStun",fileName ="PlayerState_SmallStun",order = 14)]
public class PlayerState_SmallStun : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerAnimator.speed = 1.0f;
        player.damageableIndex = 1;
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        SetAnimator_OnHurt();
        playerAnimator.Play("SmallStun");
        FaceDir = (playerStateMachine.attacker.position - playerStateMachine.playerTransform.position).normalized;
    }

    public override void Exit()
    {
        base.Exit();
        player.damageableIndex = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (IsAnimationEnd)
        {
            //切换至移动
            if (playerInput.WantsMove)
            {
                if (playerInput.IsRun && playerData.CurrentEnergy > playerStateMachine.runEnergyLimit)
                    playerStateMachine.SwitchState(typeof(PlayerState_Run));
                else
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
        playerController.OnHurtDisplace(Stun_Physics.SmallStun,FaceDir,StateDuration/AnimationLength);
    }
}
