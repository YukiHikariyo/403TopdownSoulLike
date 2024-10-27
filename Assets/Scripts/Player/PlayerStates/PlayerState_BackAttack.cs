using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/BackAttack", fileName = "PlayerState_BackAttack",order = 10)]
public class PlayerState_BackAttack : PlayerState
{
    float degree;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;


        degree = playerStateMachine.RestrictedRotation(BackAttack);
        FaceDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * degree), Mathf.Sin(Mathf.Deg2Rad * degree));
        SetAnimator_OnStart_Input();
        playerAnimator.Play("BackAttack");

        BackAttack.transform.localEulerAngles = new Vector3(BackAttack.transform.localEulerAngles.x, BackAttack.transform.localEulerAngles.y, degree);
        
        playerStateMachine.successfulBack = false;
        playerData.MotionToughness += 80f;
    }

    public override void Exit()
    {
        base.Exit();
        player.damageableIndex = 0;
        playerData.MotionToughness = 0f;
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
                    if(playerStateMachine.successfulBack)
                        playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_4));
                    else
                        playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));
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
        playerController.SkillDisplace(Skill_Physics.BackAttack, FaceDir, StateDuration / AnimationLength);
    }
}
