using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerState/BigLight", fileName = "PlayerState_BigLight",order = 13)]
public class PlayerState_BigLight : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        SetAnimator_OnStart_Mouse();
        playerAnimator.Play("BigLight");
        playerStateMachine.magicEvent.AddListener(BigLight);
    }

    public override void Exit()
    {
        base.Exit();
        playerStateMachine.magicEvent.RemoveListener(BigLight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;

            if (playerInput.LightAttack)
                playerStateMachine.memory = InputMemory.LightAttack;
        }
        if (playerStateMachine.CanStateSwitch)
        {
            if (playerStateMachine.memory == InputMemory.Roll && playerController.RollCount < 3)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
            }
            else if (playerStateMachine.memory == InputMemory.LightAttack)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));
            }
        }
        if (IsAnimationEnd)
        {
            //切换至移动
            if (playerInput.WantsMove)
            {
                if (playerInput.IsRun)
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
        playerController.Idle();
    }

    public void BigLight()
    {
        bigLight.SetActive(true);
    }
}
