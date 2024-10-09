using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/CatchChance", fileName = "PlayerState_CatchChance")]
public class PlayerState_CatchChance : PlayerState
{
    float UnDamageableStartTime;
    float PerfectCheckStartTime;
    float UnDamageableTime;
    float PerfectCheckTime;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        playerAnimator.Play("CatchChance");
        FaceDir = -playerStateMachine.MouseDistance.normalized;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (StateDuration > UnDamageableStartTime && StateDuration <= UnDamageableStartTime + UnDamageableTime)
        {
            if (StateDuration > PerfectCheckStartTime && StateDuration <= PerfectCheckStartTime + PerfectCheckTime)
                player.damageableIndex = 2;
            else
                player.damageableIndex = 1;
        }
        else
            player.damageableIndex = 0;
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.LightAttack)
                playerStateMachine.memory = InputMemory.LightAttack;
            else if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;
        }
        if (playerStateMachine.CanStateSwitch)
        {
            switch (playerStateMachine.memory)
            {
                case InputMemory.LightAttack:
                    playerStateMachine.SwitchState(typeof(PlayerState_BackAttack));
                    break;
                case InputMemory.Roll:
                    playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
                    break;
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
        playerController.SkillDisplace(Skill_Physics.CatchChance, FaceDir, StateDuration / AnimationLength);
    }
}
