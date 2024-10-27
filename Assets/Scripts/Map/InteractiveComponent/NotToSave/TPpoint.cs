using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPpoint : InteractiveComponent
{
    public GameObject target;
    Vector3 targetPosition;
    public override void Initialization()
    {
        alwaysInteractive = true;
    }

    public override bool SwitchState()
    {
        targetPosition = target.transform.position;
        GameManager.Instance.TeleportPlayer(targetPosition);
        return false;
    }
}
