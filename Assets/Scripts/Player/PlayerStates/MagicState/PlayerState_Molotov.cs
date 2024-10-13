using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/Molotov",fileName = "PlayerState_Molotov",order = 11)]
public class PlayerState_Molotov : PlayerState
{
    Vector3 pa, pb;

    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        SetAnimator_OnStart();
        playerAnimator.Play("Molotov");
        pa = playerStateMachine.playerTransform.position;
        pb = pa + playerStateMachine.MouseDistance;
        playerStateMachine.magicEvent.AddListener(ShootMolotov);
    }

    public override void Exit()
    {
        base.Exit();
        playerStateMachine.magicEvent.RemoveListener(ShootMolotov);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;

            if(playerInput.LightAttack)
                playerStateMachine.memory = InputMemory.LightAttack;
        }
        if (playerStateMachine.CanStateSwitch)
        {
            if(playerStateMachine.memory == InputMemory.Roll && playerController.RollCount < 3)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
            }
            else if(playerStateMachine.memory == InputMemory.LightAttack)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));
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
        playerController.Idle();
    }
    public void ShootMolotov()
    {
        shooter.Molotov(pa, pb,playerController.molotov_Y,playerController.deviation,playerController.molotovBottleCount);
    }
}
