using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PackageManager : MonoSingleton<PackageManager>, ISaveable
{
    public PlayerData playerData;

    [SerializeField][Tooltip("当前金币")] private int coin;
    [Tooltip("当前金币")]
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
        }
    }

    [Tooltip("所有物品列表")] public List<StaticItemData> allItemList;
    [Tooltip("已获得道具字典")] public Dictionary<int, LocalItemData> itemDict;

    [Tooltip("所有武器列表")] public List<StaticWeaponData> allWeaponList;
    [Tooltip("已获得武器字典")] public Dictionary<int, LocalWeaponData> weaponDict;
    public LocalWeaponData currentWeapon;

    protected override void Awake()
    {
        base.Awake();

        itemDict = new Dictionary<int, LocalItemData>();
        weaponDict = new Dictionary<int, LocalWeaponData>();

        allItemList.Sort();
        allWeaponList.Sort();

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

    public void ConsumeItem(int id, int number)
    {
        if (itemDict.ContainsKey(id))
        {
            UIManager.Instance.ConsumeItem(id, number);
            if (itemDict[id].number >= number)
                itemDict[id].number -= number;
        }
    }

    [ContextMenu("消耗2个测试物品")]
    public void TestConsumeItem()
    {
        if (itemDict.ContainsKey(0))
        {
            UIManager.Instance.ConsumeItem(0, 2);
            if (itemDict[0].number >= 2)
                itemDict[0].number -= 2;
        }
    }

    #endregion

    #region 武器

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

    public void UpgradeWeapon(int id)
    {
        if (weaponDict.ContainsKey(id))
            weaponDict[id].level++;
    }

    #endregion
}
