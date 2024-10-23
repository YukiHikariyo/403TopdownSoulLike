using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/UseHealthBottle", fileName = "PlayerState_UseHealthBottle", order = 18)]
public class PlayerState_UseHealthBottle : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerStateMachine.CanAcceptInput = false;
        playerStateMachine.CanStateSwitch = false;
        playerAnimator.Play("UseBottle");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerStateMachine.CanStateSwitch)
        {
            if(playerInput.Rolll && playerController.RollCount < 3)
                playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
        }
        if (IsAnimationEnd)
        {
            if (PackageManager.Instance.ConsumeHealthBottle())
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Successful_Health));
            }
            else
            {
                Debug.Log("Empty!");
                playerStateMachine.SwitchState(typeof (PlayerState_Idle));
            }
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.Idle();
    }
}
