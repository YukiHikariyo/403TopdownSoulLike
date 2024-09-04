using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : Singleton<PackageManager>, ISaveable
{
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
        LocalWeaponData newWeapon = new LocalWeaponData(Guid.NewGuid().ToString(), id);
        weaponDict.Add(id, newWeapon);
    }

    public void EquipWeapon(int id)
    {

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
            saveData.intDict.Add(weapon.uid + "id", weapon.id);
            saveData.intDict.Add(weapon.uid + "level", weapon.level);
            saveData.boolDict.Add(weapon.uid + "isEquipped", weapon.isEquipped);
        }
    }

    public void LoadSaveData(SaveData saveData)
    {
        
    }
}
