using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/LightAttack_4", fileName = "PlayerState_LightAttack_4",order = 6)]
public class PlayerState_LightAttack_4 : PlayerState
{
    float degree;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;

        degree = playerStateMachine.RestrictedRotation(lightAtk_3);
        FaceDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * degree), Mathf.Sin(Mathf.Deg2Rad * degree));
        SetAnimator_OnStart_Input();
        playerAnimator.Play("L4_Attack");

        lightAtk_4.transform.localEulerAngles = new Vector3(lightAtk_4.transform.localEulerAngles.x, lightAtk_4.transform.localEulerAngles.y, degree);

        playerData.MotionToughness += 10f;
    }

    public override void Exit()
    {
        base.Exit();
        playerData.MotionToughness = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;
            else if (playerInput.LightAttack)
                playerStateMachine.memory = InputMemory.LightAttack;
            else if (playerInput.RightAttack)
                playerStateMachine.memory = InputMemory.RightAttack;
        }
        if (playerStateMachine.CanStateSwitch)
        {
            switch (playerStateMachine.memory)
            {
                case InputMemory.RightAttack:
                    playerStateMachine.SwitchState(typeof(PlayerState_CatchChance));
                    break;
                case InputMemory.Roll:
                    if(playerController.RollCount < 3)
                        playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
                    break;
            }
        }
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
        playerController.LightAttack_4(FaceDir, StateDuration / AnimationLength);
    }
}
