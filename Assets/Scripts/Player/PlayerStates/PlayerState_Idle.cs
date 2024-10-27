using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[CreateAssetMenu(menuName ="Data/PlayerState/Idle",fileName ="PlayerState_Idle",order = 0)]
public class PlayerState_Idle : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        playerAnimator.Play("Idle");
    }

    public override void Exit() 
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        SetAnimator_Update();

        if (!playerInput.Interaction || playerStateMachine.interactionObj == null)
        {
            if (playerInput.WantsMove)
            {
                if (playerInput.IsRun && playerData.CurrentEnergy > playerStateMachine.runEnergyLimit)
                    playerStateMachine.SwitchState(typeof(PlayerState_Run));
                else
                    playerStateMachine.SwitchState(typeof(PlayerState_Move));
            }
            else if (playerInput.Magic_1 && playerData.magicUnlockState[0] && playerStateMachine.magicTimer[0] <= 0)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_FlashBang));
            }
            else if (playerInput.Magic_2 && playerData.magicUnlockState[1] && playerStateMachine.magicTimer[1] <= 0)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_Molotov));
            }
            else if (playerInput.Magic_3 && playerData.magicUnlockState[2] && playerStateMachine.magicTimer[2] <= 0)
            {
                playerStateMachine.SwitchState(typeof(PlayerState_BigLight));
            }
            else if (playerInput.RightAttack)
                playerStateMachine.SwitchState(typeof(PlayerState_Charging));

            else if (playerInput.LightAttack)
                playerStateMachine.SwitchState(typeof(PlayerState_LightAttack_1));

            else if (playerInput.Roll && playerController.RollCount < 3)
                playerStateMachine.SwitchState(typeof(PlayerState_FirstRoll));
            else if (playerInput.UseHealthBottle)
                playerStateMachine.SwitchState(typeof(PlayerState_UseHealthBottle));
            else if (playerInput.UseManaBottle)
                playerStateMachine.SwitchState(typeof(PlayerState_UseManaBottle));
        }
        else
        {
            if (playerStateMachine.interactionObj.alwaysInteractive || playerStateMachine.interactionObj.State == false)
            {
                if (playerStateMachine.interactionObj.SwitchState())
                    playerStateMachine.interactionObj.State = true;
            }
        }



        //TODO:打开背包
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        playerController.Idle();
    }
}
