using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Date/PlayerState/RightAttack",fileName ="PlayerState_RightAttack")]
public class PlayerState_RightAttack : PlayerState
{
    float degree;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        degree = playerStateMachine.RestrictedRotation(BackAttack);
        FaceDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * degree), Mathf.Sin(Mathf.Deg2Rad * degree));
        BackAttack.transform.localEulerAngles = new Vector3(lightAtk_2.transform.localEulerAngles.x, lightAtk_2.transform.localEulerAngles.y, degree);
        SetAnimator_OnStart();
    }

    public override void Exit()
    {
        base.Exit();
        playerAnimator.speed = 1f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(playerStateMachine.CanAcceptInput)
        {
            if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;
        }
        if (playerStateMachine.CanStateSwitch)
        {
            if (playerStateMachine.memory == InputMemory.Roll)
                playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
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
        playerController.SkillDisplace(Skill_Physics.RightAttack,FaceDir,StateDuration/AnimationLength);
    }
}
