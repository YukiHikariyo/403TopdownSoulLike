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

        SetRotationZ(lightAtk_1,playerStateMachine.MouseDegree);
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

    #region 偷懒
    private void SetRotationZ(GameObject obj,float angle)
    {
        obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y,angle);
    }
    #endregion
}
