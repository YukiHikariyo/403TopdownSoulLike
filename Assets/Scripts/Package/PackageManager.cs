using System;
using System.Collections;
using System.Collections.Generic;
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

    [Tooltip("游戏中所有武器列表")] public List<StaticWeaponData> allWeaponList;
    [Tooltip("已获得武器字典")] public Dictionary<int, LocalWeaponData> weaponDict;
    public LocalWeaponData currentWeapon;

    protected override void Awake()
    {
        base.Awake();

        weaponDict = new Dictionary<int, LocalWeaponData>();
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

    public void GetWeapon(int id)
    {
        LocalWeaponData newWeapon = new LocalWeaponData(id);
        weaponDict.Add(id, newWeapon);
        UIManager.Instance.AddWeapon(id);
    }

    [ContextMenu("添加测试武器")]
    public void TestGetWeapon()
    {
        LocalWeaponData newWeapon = new LocalWeaponData(0);
        weaponDict.Add(0, newWeapon);
        UIManager.Instance.AddWeapon(0);
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
}
