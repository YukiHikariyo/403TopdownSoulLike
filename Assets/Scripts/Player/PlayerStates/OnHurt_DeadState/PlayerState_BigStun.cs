using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/BigStun", fileName = "PlayerState_BigStun", order = 16)]
public class PlayerState_BigStun : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerAnimator.speed = 1.0f;
        player.damageableIndex = 1;
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        SetAnimator_OnHurt();
        playerAnimator.Play("BigStun");
        FaceDir = (playerStateMachine.attacker.position - playerStateMachine.playerTransform.position).normalized;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.WantsMove)
                playerStateMachine.memory = InputMemory.Direction;
            else if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;
        }
        if (playerStateMachine.CanStateSwitch)
        {
            if (playerStateMachine.memory == InputMemory.Roll || playerStateMachine.memory == InputMemory.Direction)
                playerStateMachine.SwitchState(typeof(PlayerState_GetUp));
        }
        if(IsAnimationEnd)
            playerStateMachine.SwitchState(typeof(PlayerState_GetUp));
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.OnHurtDisplace(Stun_Physics.BigStun,FaceDir,StateDuration/AnimationLength);
    }
}
