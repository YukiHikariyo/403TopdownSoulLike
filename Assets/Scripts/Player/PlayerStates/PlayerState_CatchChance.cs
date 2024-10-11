using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/CatchChance", fileName = "PlayerState_CatchChance")]
public class PlayerState_CatchChance : PlayerState
{
    public float UnDamageableStartTime;
    public float PerfectCheckStartTime;
    public float UnDamageableTime;
    public float PerfectCheckTime;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        SetAnimator_OnStart();
        playerAnimator.Play("CatchChance");
        //确定判定区间中点
        UnDamageableStartTime = playerStateMachine.CatchChancepoint - UnDamageableTime / 2;
        PerfectCheckStartTime = playerStateMachine.CatchChancepoint - PerfectCheckTime / 2;
        //
        FaceDir = -playerStateMachine.MouseDistance.normalized;
        SetRotationZ(BackAttack, playerStateMachine.MouseDegree);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (StateDuration > UnDamageableStartTime && StateDuration <= UnDamageableStartTime + UnDamageableTime)
        {
            if (StateDuration > PerfectCheckStartTime && StateDuration <= PerfectCheckStartTime + PerfectCheckTime)
                player.damageableIndex = 2;
            else
                player.damageableIndex = 1;
        }
        else
            player.damageableIndex = 0;
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
                    playerStateMachine.SwitchState(typeof(PlayerState_BackAttack));
                    break;
                case InputMemory.Roll:
                    playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
                    break;
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
        playerController.SkillDisplace(Skill_Physics.CatchChance, FaceDir, StateDuration / AnimationLength);
    }
    private void SetRotationZ(GameObject obj, float angle)
    {
        obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y, angle);
    }
}
