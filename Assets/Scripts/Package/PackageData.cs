using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticWeaponData : ScriptableObject
{
    [Serializable]
    public struct WeaponStats
    {
        [Tooltip("伤害")] public float damage;
        [Tooltip("暴击率")] public float critRate;
        [Tooltip("升级花费金钱数量")] public int coinCost;
        [Tooltip("升级花费锻造石数量")] public int stoneCost;
    }

    [Tooltip("ID")] public int weaponID;
    [Tooltip("名称")] public string weaponName;
    [Tooltip("图标")] public Sprite weaponIcon;
    [Tooltip("最大等级")] public int maxLevel;
    [Space(16)]
    [Tooltip("数值")] public WeaponStats[] weaponStats;   //数组索引表示武器等级
}

public class LocalWeaponData
{
    public int id;
    public int level;
    public bool isEquipped;

    public LocalWeaponData(int id, int level = 1, bool isEquipped = false)
    {
        this.id = id;
        this.level = level;
        this.isEquipped = isEquipped;
    }
}