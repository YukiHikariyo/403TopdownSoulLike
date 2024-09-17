using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PackageManager : MonoSingleton<PackageManager>, ISaveable
{
    public PlayerData playerData;

    [SerializeField][Tooltip("当前金币")] private int coin;

    [Tooltip("所有物品列表")] public List<StaticItemData> allItemList;
    [Tooltip("已获得道具字典")] public Dictionary<int, LocalItemData> itemDict;

    [Tooltip("所有武器列表")] public List<StaticWeaponData> allWeaponList;
    [Tooltip("已获得武器字典")] public Dictionary<int, LocalWeaponData> weaponDict;
    public LocalWeaponData currentWeapon;

    [Tooltip("所有饰品列表")] public List<StaticAccessoryData> allAccessoryList;
    [Tooltip("已获得饰品字典")] public Dictionary<int, LocalAccessoryData> accessoryDict;

    protected override void Awake()
    {
        base.Awake();

        itemDict = new Dictionary<int, LocalItemData>();
        weaponDict = new Dictionary<int, LocalWeaponData>();
        accessoryDict = new Dictionary<int, LocalAccessoryData>();

        allItemList.Sort();
        allWeaponList.Sort();
        allAccessoryList.Sort();

        if (!weaponDict.ContainsKey(0))
            GetWeapon(0);
    }

    private void OnEnable()
    {
        (this as ISaveable).Register();
    }

    private void OnDisable()
    {
        (this as ISaveable).UnRegister();
    }

    public void GetSaveData(SaveData saveData)
    {
        foreach (LocalWeaponData weapon in weaponDict.Values)
        {
            if (!saveData.weaponDict.ContainsKey(weapon.id))
                saveData.weaponDict.Add(weapon.id, weapon);
            else
                saveData.weaponDict[weapon.id] = weapon;
        }
    }

    public void LoadSaveData(SaveData saveData)
    {
        foreach (int weaponID in saveData.weaponDict.Keys)
        {
            if (!weaponDict.ContainsKey(weaponID))
                weaponDict.Add(weaponID, saveData.weaponDict[weaponID]);
            else
                weaponDict[weaponID] = saveData.weaponDict[weaponID];
        }
    }

    #region 物品

    /// <summary>
    /// 获得金币
    /// </summary>
    /// <param name="number">获得数量</param>
    public void GetCoin(int number)
    {
        coin = coin + number < 999999 ? coin + number : 999999;
        //TODO: UI金币逻辑处理
    }

    /// <summary>
    /// 消耗金币
    /// </summary>
    /// <param name="number">消耗数量</param>
    /// <returns>是否成功消耗</returns>
    /// <remarks>特别重要：同时消耗多种物品（包括金币）时，不要用此方法的返回值判断是否成功消耗</remarks>
    public bool ConsumeCoin(int number)
    {
        if (coin >= number)
        {
            coin -= number;
            return true;
        }
        else
        {
            UIManager.Instance.PlayTipSequence("金币不足");
            return false;
        }
    }

    /// <summary>
    /// 获得物品
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <param name="number">获得数量</param>
    public void GetItem(int id, int number)
    {
        if (itemDict.ContainsKey(id))
            itemDict[id].number = itemDict[id].number + number < 999 ? itemDict[id].number + number : 999;
        else
            itemDict.Add(id, new LocalItemData(id, number));

        UIManager.Instance.GetItem(id, number);
    }

    [ContextMenu("获得3个测试物品")]
    public void TestGetItem()
    {
        if (itemDict.ContainsKey(0))
            itemDict[0].number = itemDict[0].number + 3 < 999 ? itemDict[0].number + 3 : 999;
        else
            itemDict.Add(0, new LocalItemData(0, 3));

        UIManager.Instance.GetItem(0, 3);
    }

    /// <summary>
    /// 消耗物品
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <param name="number">消耗数量</param>
    /// <returns>是否成功消耗</returns>
    /// <remarks>特别重要：同时消耗多种物品（包括金币）时，不要用此方法的返回值判断是否成功消耗</remarks>
    public bool ConsumeItem(int id, int number)
    {
        if (itemDict.ContainsKey(id))
        {
            UIManager.Instance.ConsumeItem(id, number);
            if (itemDict[id].number >= number)
            {
                itemDict[id].number -= number;
                return true;
            }
        }

        return false;
    }

    [ContextMenu("消耗2个测试物品")]
    public bool TestConsumeItem()
    {
        if (itemDict.ContainsKey(0))
        {
            UIManager.Instance.ConsumeItem(0, 2);
            if (itemDict[0].number >= 2)
            {
                itemDict[0].number -= 2;
                return true;
            }
        }

        return false;
    }

    #endregion

    #region 武器

    /// <summary>
    /// 获得武器
    /// </summary>
    /// <param name="id">武器ID</param>
    public void GetWeapon(int id)
    {
        if (!weaponDict.ContainsKey(id))
        {
            weaponDict.Add(id, new LocalWeaponData(id));
            UIManager.Instance.GetWeapon(id);
        }
    }

    [ContextMenu("添加测试武器")]
    public void TestGetWeapon()
    {
        if (!weaponDict.ContainsKey(0))
        {
            weaponDict.Add(0, new LocalWeaponData(0));
            UIManager.Instance.GetWeapon(0);
        }
    }

    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="id">武器ID</param>
    /// <remarks>此方法在UIManager中的EquipWeapon方法中调用</remarks>
    public void EquipWeapon(int id)
    {
        if (weaponDict.ContainsKey(id))
        {
            if (currentWeapon != null)
                currentWeapon.isEquipped = false;
            weaponDict[id].isEquipped = true;
        }

        playerData.currentWeaponStaticData = allWeaponList[id];
        playerData.currentWeaponLocalData = weaponDict[id];
    }

    /// <summary>
    /// 升级武器
    /// </summary>
    /// <param name="id">武器ID</param>
    /// <remarks>此方法在UIManager中的UpgradeWeapon方法中调用</remarks>
    public void UpgradeWeapon(int id)
    {
        if (weaponDict.ContainsKey(id) && itemDict[1] != null && itemDict[1].number >= allWeaponList[id].weaponStats[weaponDict[id].level].stoneCost && coin >= allWeaponList[id].weaponStats[weaponDict[id].level].coinCost)
        {
            ConsumeItem(1, allWeaponList[id].weaponStats[weaponDict[id].level].stoneCost);
            ConsumeCoin(allWeaponList[id].weaponStats[weaponDict[id].level].coinCost);
            weaponDict[id].level++;
        }
        else if (itemDict[1].number < allWeaponList[id].weaponStats[weaponDict[id].level].stoneCost)
            UIManager.Instance.PlayTipSequence("锻造石数量不足");
        else if (coin < allWeaponList[id].weaponStats[weaponDict[id].level].coinCost)
            UIManager.Instance.PlayTipSequence("金币不足");
    }

    #endregion
}
