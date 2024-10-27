using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : InteractiveComponent
{
    public Spike gate;
    public string successfulUse;
    public string failUse;
    [SerializeField] int id;
    public override void Initialization()
    {
        base.Initialization();
        if (state)
        {
            OpenGate();
        }
    }

    public void OpenGate()
    {
        gate.State = true;
    }

    public override bool SwitchState()
    {
        if(PackageManager.Instance.itemDict.ContainsKey(id))
        {
            if (PackageManager.Instance.itemDict[id].number > 0)
            {
                UIManager.Instance.OpenConfirmationPanel(successfulUse + PackageManager.Instance.allItemList[id].itemName);
                return true;
            }
            else
                UIManager.Instance.OpenConfirmationPanel(failUse);
        }
        else
            UIManager.Instance.OpenConfirmationPanel(failUse);

        return false;
    }
}

