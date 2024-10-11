using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/Move", fileName = "PlayerState_Move")]
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

        if (playerInput.RightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_Charging));

        if (playerInput.LightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));

        if (playerInput.Roll && playerController.RollCount < 3)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));

        if (!playerInput.WantsMove)
            playerStateMachine.SwitchState(typeof(PlayerState_Idle));
    }

    public override void PhysicUpdate()
    {
        playerController.Move();
    }
}
