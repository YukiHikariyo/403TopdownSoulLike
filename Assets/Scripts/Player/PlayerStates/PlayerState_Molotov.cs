using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerState/Molotov",fileName = "PlayerState_Molotov")]
public class PlayerState_Molotov : PlayerState
{
    Vector3 pa, pb;

    public override void Enter()
    {
        base.Enter();
        SetAnimator_OnStart();
        pa = playerStateMachine.playerTransform.position;
        pb = pa + playerStateMachine.MouseDistance;
        playerStateMachine.magicEvent.AddListener(ShootMolotov);
    }

    public override void Exit()
    {
        base.Exit();
        playerStateMachine.magicEvent.RemoveListener(ShootMolotov);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
    public void ShootMolotov()
    {
        shooter.Molotov(pa, pb,playerController.molotov_Y,playerController.deviation,playerController.molotovBottleCount);
    }
}
