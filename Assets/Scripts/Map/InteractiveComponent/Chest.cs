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

    public Item[] itemTable;
    public int[] weaponTable;
    public int[] accessoryTable;
    public override void Initialization()
    {
        base.Initialization();
    }

    public override void SwitchState()
    {
        base.SwitchState();
        OpenChest();
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
    }
}
