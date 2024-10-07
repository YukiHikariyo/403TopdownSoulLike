using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/LightAttack_3", fileName = "PlayerState_LightAttack_3")]
public class PlayerState_LightAttack_3 : PlayerState
{
    float degree;
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerAnimator.Play("L3_Attack");
        degree = playerStateMachine.RestrictedRotation(lightAtk_2);
        FaceDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * degree), Mathf.Sin(Mathf.Deg2Rad * degree));
        lightAtk_3.transform.localEulerAngles = new Vector3(lightAtk_3.transform.localEulerAngles.x, lightAtk_3.transform.localEulerAngles.y, degree);
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
                    playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_4));
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
        playerController.LightAttack_3(FaceDir, StateDuration / AnimationLength);
    }

}