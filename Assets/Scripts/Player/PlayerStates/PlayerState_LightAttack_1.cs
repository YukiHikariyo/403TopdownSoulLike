using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/LightAttack_1", fileName = "PlayerState_LightAttack_1")]

public class PlayerState_LightAttack_1 : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerAnimator.Play("L1_Attack");
        FaceDir = playerStateMachine.MouseDistance.normalized;
        SetRotationZ(lightAtk_1,playerStateMachine.MouseDegree);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerStateMachine.CanAcceptInput)
        {
            if (playerInput.Roll)
                playerStateMachine.memory = InputMemory.Roll;
            else if (playerInput.LeftCheck)
                playerStateMachine.memory = InputMemory.LightAttack;
            else if (playerInput.RightCheck)
                playerStateMachine.memory = InputMemory.RightAttack;
        }
        if (playerStateMachine.CanStateSwitch)
        {
            switch (playerStateMachine.memory)
            {
                case InputMemory.LightAttack:
                    Debug.Log("L2");
                    break;
                case InputMemory.RightAttack:
                    Debug.Log("Check");
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
        playerController.LightAttack_1(FaceDir, StateDuration/AnimationLength);
    }

    #region 偷懒
    private void SetRotationZ(GameObject obj,float angle)
    {
        obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y,angle);
    }
    #endregion
}
