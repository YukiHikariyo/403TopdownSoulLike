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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
    // 二阶贝塞尔曲线
    public Vector3 quardaticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 aa = a + (b - a) * t;
        Vector3 bb = b + (c - b) * t;
        return aa + (bb - aa) * t;
    }

}
