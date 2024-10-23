using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/RightAttack", fileName = "PlayerState_RightAttack",order = 7)]
public class PlayerState_RightAttack : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;

        FaceDir = playerStateMachine.MouseDistance.normalized;
        SetRotationZ(RightAttack,playerStateMachine.MouseDegree);

        SetAnimator_OnStart_Input();
        playerAnimator.Play("RightAttack");
    }

    public override void Exit()
    {
        base.Exit();
        playerAnimator.speed = 1f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;
        }
        if (playerStateMachine.CanStateSwitch)
        {
            if (playerStateMachine.memory == InputMemory.Roll && playerController.RollCount < 3)
                playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
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
        playerController.SkillDisplace(Skill_Physics.RightAttack, FaceDir, StateDuration / AnimationLength);
    }
    private void SetRotationZ(GameObject obj, float angle)
    {
        obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y, angle);
    }
}
