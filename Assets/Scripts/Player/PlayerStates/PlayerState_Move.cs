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
        if (!playerInput.WantsMove)
            playerStateMachine.SwitchState(typeof(PlayerState_Idle));
        if (playerInput.LightAttack)
            playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));
        //等待添加耐力限制
        if (playerInput.Roll)
            playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
    }

    public override void PhysicUpdate()
    {
        playerController.Move();
    }
}
