using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryPanel : BasePanel
{
    public override void OnClose()
    {

    }

    public override void OnOpen()
    {
        UIManager.Instance.CurrentAccessoryInfUpdate();
    }
}
