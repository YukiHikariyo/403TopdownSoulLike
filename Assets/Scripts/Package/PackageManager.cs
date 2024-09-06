using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoSingleton<PackageManager>, ISaveable
{
    private int coin;
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
        }
    }

    public List<StaticWeaponData> allWeaponList;
    public Dictionary<int, LocalWeaponData> weaponDict;

    protected override void Awake()
    {
        base.Awake();

        weaponDict = new Dictionary<int, LocalWeaponData>();
        allWeaponList.Sort();
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
            foreach (var weapon in weaponDict.Values)
                weapon.isEquipped = false;

            weaponDict[id].isEquipped = true;
        }
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
        foreach (int weaponID in GameManager.Instance.saveData.weaponDict.Keys)
        {
            if (!weaponDict.ContainsKey(weaponID))
                weaponDict.Add(weaponID, GameManager.Instance.saveData.weaponDict[weaponID]);
            else
                weaponDict[weaponID] = GameManager.Instance.saveData.weaponDict[weaponID];
        }
    }
}
