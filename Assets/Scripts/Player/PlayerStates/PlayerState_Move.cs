using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/Move", fileName = "PlayerState_Move",order = 1)]
public class PlayerState_Move : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerAnimator.Play("Move");
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        SetAnimator_Update();
        //TODO:耐力限制

        if (playerInput.Magic_2)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Molotov));
        }
        if (playerInput.Magic_2)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_Molotov));
        }
        if (playerInput.Magic_3)
        {
            playerStateMachine.SwitchState(typeof(PlayerState_BigLight));
        }
        if (playerInput.RightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_Charging));

        if (playerInput.LightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));

        if (playerInput.Roll && playerController.RollCount < 3)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));

        if (!playerInput.WantsMove)
            playerStateMachine.SwitchState(typeof(PlayerState_Idle));

        //TODO:打开背包
    }

    public override void PhysicUpdate()
    {
        playerController.Move();
    }
}
