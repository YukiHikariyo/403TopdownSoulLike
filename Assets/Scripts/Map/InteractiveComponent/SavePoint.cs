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
        GameManager.Instance.SaveGame();
        UIManager.Instance.PlayTipSequence(tip);
        return base.SwitchState();
    }
}
