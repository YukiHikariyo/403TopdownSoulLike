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
    }

    public override void SwitchState()
    {
        if(stateMachine != null)
        {
            stateMachine.playerData.magicUnlockState[magicID] = true;
            MagicUIManager.Instance.UpdateUnlockState(magicID,true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
        }
    }
}
