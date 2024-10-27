using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SavePoint : InteractiveComponent
{
    public string tip;
    public override void Initialization()
    {
        base.Initialization();
    }

    public override bool SwitchState()
    {
        GameManager.Instance.player.playerData.lastPosition = GameManager.Instance.player.transform.position;
        GameManager.Instance.SaveGame();
        UIManager.Instance.PlayTipSequence(tip);
        if (!state)
        {
            animator.Play("LightUp");
            return false;
        }
        else
            return true;
    }

    public void WhenLighted()
    {
        State = true;
    }
}
