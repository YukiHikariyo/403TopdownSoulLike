using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : BasePanel
{
    public override void OnClose()
    {

    }

    public override void OnOpen()
    {
        if (UIManager.Instance.currentSelectedItem)
        {
            UIManager.Instance.selectedItemName.text = PackageManager.Instance.allItemList[UIManager.Instance.currentSelectedItem.itemID].itemName;
            UIManager.Instance.selectedItemNumber.text = " 数量：" + PackageManager.Instance.itemDict[UIManager.Instance.currentSelectedItem.itemID].number + "个";
        }

        foreach (ItemSlotUI itemSlotUI in UIManager.Instance.itemSlotDict.Values)
        {
            itemSlotUI.itemNumber = PackageManager.Instance.itemDict[itemSlotUI.itemID].number;
            itemSlotUI.itemNumberText.text = itemSlotUI.itemNumber.ToString();
        }
    }
}
