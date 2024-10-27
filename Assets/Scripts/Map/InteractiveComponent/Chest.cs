using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractiveComponent
{
    [Serializable]
    public struct Item
    {
        public int id;
        public int num;
    }
    public string text;
    public Item[] itemTable;
    public int[] weaponTable;
    public int[] accessoryTable;
    public int[] magicTable;
    public override void Initialization()
    {
        base.Initialization();
        showTips = !state;
    }

    public override bool SwitchState()
    {
        OpenChest();
        return base.SwitchState();
    }

    private void OpenChest()
    {
        for (int i = 0;i < itemTable.Length; i++) 
        {
            PackageManager.Instance.GetItem(itemTable[i].id, itemTable[i].num);
        }
        for (int i = 0;i < weaponTable.Length; i++)
        {
            PackageManager.Instance.GetWeapon(weaponTable[i]);
        }
        for (int i = 0;i < accessoryTable.Length; i++)
        {
            PackageManager.Instance.GetAccessory(accessoryTable[i]);
        }
        for (int i = 0; i < magicTable.Length; ++i)
        {
            stateMachine.playerData.magicUnlockState[magicTable[i]] = true;
            MagicUIManager.Instance.UpdateUnlockState(magicTable[i], true);
            UIManager.Instance.OpenConfirmationPanel(text);
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
