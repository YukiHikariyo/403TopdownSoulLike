using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/BackAttack", fileName = "PlayerState_BackAttack")]
public class PlayerState_BackAttack : PlayerState
{
    bool foresight = false;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        SetAnimator_OnStart();
        playerAnimator.Play("BackAttack");
        foresight = false;
        backAttackArea.successEvent.AddListener(foresightCheck);
    }

    public override void Exit()
    {
        base.Exit();
        backAttackArea.successEvent.RemoveListener(foresightCheck);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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
                    if(foresight)
                        playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_4));
                    else
                        playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));
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
    private void foresightCheck(IDamageable damageable)
    {
        foresight = true;
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.SkillDisplace(Skill_Physics.BackAttack, FaceDir, StateDuration / AnimationLength);
    }
}
