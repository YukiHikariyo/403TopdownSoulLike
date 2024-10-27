using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicUnLock : InteractiveComponent
{
    public int magicID;
    public string text;
    public override void Initialization()
    {
        base.Initialization();
        showTips = !state;
    }

    public override bool SwitchState()
    {
        if(stateMachine != null)
        {
            stateMachine.playerData.magicUnlockState[magicID] = true;
            MagicUIManager.Instance.UpdateUnlockState(magicID,true);
            UIManager.Instance.OpenConfirmationPanel(text); 
            return true;
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
        }
    }
}

