using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 静态武器数据
/// </summary>
public class StaticWeaponData : ScriptableObject, IComparable<StaticWeaponData>
{
    [Serializable]
    public struct WeaponStats
    {
        [Tooltip("攻击力")] public float damage;
        [Tooltip("暴击率")] public float critRate;
        [Tooltip("暴击伤害")] public float critDamage;
        [Tooltip("穿透力")] public float penetratingPower;
        [Tooltip("伤害减免")] public float reductionRate;
        [Tooltip("升级花费金钱数量")] public int coinCost;
        [Tooltip("升级花费锻造石数量")] public int stoneCost;
    }

    [Tooltip("ID")] public int weaponID;
    [Tooltip("名称")] public string weaponName;
    [Tooltip("图标")] public Sprite weaponIcon;
    [Tooltip("最大等级")] public int maxLevel;
    [Space(16)]
    [Tooltip("数值")] public WeaponStats[] weaponStats;   //数组索引表示武器等级

    public int CompareTo(StaticWeaponData other) => weaponID - other.weaponID;
}

/// <summary>
/// 本地武器数据
/// </summary>
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

/// <summary>
/// 静态物品数据
/// </summary>
public class StaticItemData : ScriptableObject, IComparable<StaticItemData>
{
    [Tooltip("ID")] public int itemID;
    [Tooltip("名称")] public string itemName;
    [Tooltip("图标")] public Sprite itemIcon;

    public int CompareTo(StaticItemData other) => itemID - other.itemID;
}

/// <summary>
/// 本地物品数据
/// </summary>
public class LocalItemData
{
    public int id;
    public int number;

    public LocalItemData(int id, int number = 1)
    {
        this.id = id;
        this.number = number < 999 ? number : 999;  //物品最大数量999
    }
}