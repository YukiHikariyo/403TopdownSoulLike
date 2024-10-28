using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/LightAttack_2", fileName = "PlayerState_LightAttack_2",order = 4)]

public class PlayerState_LightAttack_2 : PlayerState
{
    float degree;

    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;

        FaceDir = playerStateMachine.MouseDistance.normalized;
        SetAnimator_OnStart_Input();
        playerAnimator.Play("L2_Attack");
        SetRotationZ(lightAtk_2, playerStateMachine.MouseDegree);

        playerData.MotionToughness += 1f;
    }

    public override void Exit()
    {
        base.Exit();
        playerData.MotionToughness -= 1f;
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
                case InputMemory.LightAttack:
                    playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_3));
                    break;
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
        playerController.LightAttack_2(FaceDir, StateDuration / AnimationLength);
    }

    private void SetRotationZ(GameObject obj, float angle)
    {
        obj.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
