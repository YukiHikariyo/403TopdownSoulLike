using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/Charging", fileName = "PlayerState_Charging",order = 9)]
public class PlayerState_Charging : PlayerState
{
    float chargeSpeed;
    float nowTime;
    int chargeState;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanStateSwitch = false;
        nowTime = 0f;
        chargeState = 0;
        chargeSpeed = playerData.chargeSpeedMultiplication;
        OnChargeStage0();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        SetAnimator_Update();
        //计时
        nowTime += Time.deltaTime * chargeSpeed;
        //蓄力时的移动和站立
        if (chargeState < 3)
        {
            if (nowTime > playerController.ChargeMaxTime)
            {
                nowTime = 0f;
                chargeState++;
                switch (chargeState)
                {
                    case 1:
                        OnChargeStage1(); break;
                    case 2:
                        OnChargeStage2(); break;
                    case 3:
                        OnChargeStage3(); break;
                }
            }
        }
        if (playerInput.Roll && playerController.RollCount < 3)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));

        if (playerInput.ChargeRelease)//TODO：体力限制
        {
            switch (chargeState)
            {
                case 0:
                    playerAnimator.speed = 1.1f;

                    break;
                case 1:
                    playerAnimator.speed = 1f;

                    break;
                case 2:
                    playerAnimator.speed = 0.9f;

                    break;
                case 3:
                    playerAnimator.speed = 0.8f;

                    break;
            }
        }
        //TODO：体力限制
        if (playerInput.WantsMove && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            playerAnimator.Play("Move");
        }
        if (!playerInput.WantsMove && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            playerAnimator.Play("Idle");
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        if (playerInput.WantsMove)
        {
            playerAnimator.speed = 0.6f;
            playerController.Charge_Move(StateDuration);
        }
        else
        {
            playerAnimator.speed = 1f;
            playerController.Idle();
        }
    }
    private void OnChargeStage0()
    {

    }

    private void OnChargeStage1()
    {

    }

    private void OnChargeStage2()
    {

    }

    private void OnChargeStage3()
    {

    }
}
