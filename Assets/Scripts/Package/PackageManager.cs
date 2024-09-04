using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : Singleton<PackageManager>, ISaveable
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

    public Dictionary<int, LocalWeaponData> weaponDict;

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
