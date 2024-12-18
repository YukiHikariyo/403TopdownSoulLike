using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/Charging", fileName = "PlayerState_Charging",order = 9)]
public class PlayerState_Charging : PlayerState
{
    float chargeSpeed;
    float nowTime;
    int chargeState;
    Color color = Color.white;
    ParticleSystem.MainModule mainModule;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanStateSwitch = false;
        nowTime = 0f;
        chargeState = 0;
        chargeSpeed = playerData.chargeSpeedMultiplication;
        OnChargeStage0();
        mainModule = playerStateMachine.chargeVFX.main;
        playerData.MotionToughness -= 10f;
    }

    public override void Exit()
    {
        base.Exit();
        playerData.MotionToughness = 0f;
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
        //TODO：体力限制
        if (playerInput.WantsMove && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            playerAnimator.Play("Move");
        }
        if (!playerInput.WantsMove && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            playerAnimator.Play("Idle");
        }

        if (playerInput.Roll && playerController.RollCount < 3)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));

        else if (playerInput.ChargeRelease || !playerStateMachine.ContinuousConsumeEnergy(Time.deltaTime * playerData.energyCostMultiplication * playerStateMachine.chargeCost))//TODO：体力限制
        {
            switch (chargeState)
            {
                case 0:
                    playerStateMachine.chargeStage = 0;
                    playerStateMachine.SwitchState(typeof(PlayerState_RightAttack));
                    break;
                case 1:
                    playerStateMachine.chargeStage = 1;
                    playerStateMachine.SwitchState(typeof(PlayerState_RightAttack));
                    break;
                case 2:
                    playerStateMachine.chargeStage = 2;
                    playerStateMachine.SwitchState(typeof(PlayerState_RightAttack));
                    break;
                case 3:
                    playerStateMachine.chargeStage = 3;
                    playerStateMachine.SwitchState(typeof(PlayerState_RightAttack));
                    break;
            }
        }
        
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        if (playerInput.WantsMove)
        {
            playerAnimator.speed = 0.8f;
            playerController.Charge_Move();
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
        playerStateMachine.chargeVFXobj.SetActive(false);
        color = Color.white;
        mainModule.startColor = color;
        playerStateMachine.chargeVFXobj.SetActive(true);
    }

    private void OnChargeStage2()
    {
        playerStateMachine.chargeVFXobj.SetActive(false);
        color = Color.yellow;
        mainModule.startColor = color;
        playerStateMachine.chargeVFXobj.SetActive(true);
    }

    private void OnChargeStage3()
    {
        playerStateMachine.chargeVFXobj.SetActive(false);
        color = Color.red;
        mainModule.startColor = color;
        playerStateMachine.chargeVFXobj.SetActive(true);
    }
}
