using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/LightAttack_1", fileName = "PlayerState_LightAttack_1",order =3)]

public class PlayerState_LightAttack_1 : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        FaceDir = playerStateMachine.MouseDistance.normalized;
        SetAnimator_OnStart_Input();
        playerAnimator.Play("L1_Attack");
        SetRotationZ(lightAtk_1,playerStateMachine.MouseDegree);

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
        if (playerStateMachine.CanStateSwitch)
        {
            switch (playerStateMachine.memory)
            {
                //TODO:耐力限制
                case InputMemory.LightAttack:
                    playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_2));
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
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;
            else if (playerInput.LightAttack)
                playerStateMachine.memory = InputMemory.LightAttack;
            else if (playerInput.RightAttack)
                playerStateMachine.memory = InputMemory.RightAttack;
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
        playerController.LightAttack_1(FaceDir, StateDuration/AnimationLength);
    }

    #region 偷懒
    private void SetRotationZ(GameObject obj, float angle)
    {
        obj.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    #endregion
}
