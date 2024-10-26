using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WeaponStats
{
    [Tooltip("攻击力")] public float damage;
    [Tooltip("暴击率")] public float critRate;
    [Tooltip("暴击伤害")] public float critDamage;
    [Tooltip("穿透力")] public float penetratingPower;
    [Tooltip("伤害减免")] public float reductionRate;
    [Tooltip("属性攻击倍率")] public float buffDamageMultiplication;
    [Tooltip("升级花费金钱数量")] public int coinCost;
    [Tooltip("升级花费锻造石数量")] public int stoneCost;
}

/// <summary>
/// 静态武器数据
/// </summary>
public class StaticWeaponData : ScriptableObject, IComparable<StaticWeaponData>
{
    
    [Tooltip("ID")] public int weaponID;
    [Tooltip("名称")] public string weaponName;
    [Tooltip("图标")] public Sprite weaponIcon;
    [Tooltip("最大等级")] public int maxLevel;
    [Space(16)]
    [Tooltip("数值")] public WeaponStats[] weaponStats;   //数组索引表示武器等级
    [Tooltip("属性伤害类型")] public BuffType buffDdamageType;
    [Tooltip("被动技能")] public PassiveSkillType passiveSkillType;
    [Space(16)]
    [Tooltip("描述")][TextArea(3, 6)] public string desctiption;

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
    [Tooltip("能否使用")] public bool isUseable;
    [Space(16)]
    [Tooltip("描述")][TextArea(3, 6)] public string desctiption;

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

[Serializable]
public struct AccessoryStats
{
    [Tooltip("生命值")] public float maxHealth;
    [Tooltip("魔力值")] public float maxMana;
    [Tooltip("体力值")] public float maxEnergy;
    [Tooltip("韧性")] public float toughness;
    [Tooltip("伤害减免")] public float reductionRate;
    [Tooltip("升级花费金钱数量")] public int coinCost;
    [Tooltip("升级花费强化石数量")] public int stoneCost;
}


/// <summary>
/// 静态饰品数据
/// </summary>
public class StaticAccessoryData : ScriptableObject, IComparable<StaticAccessoryData>
{
    [Tooltip("ID")] public int accessoryID;
    [Tooltip("名称")] public string accessoryName;
    [Tooltip("图标")] public Sprite accessoryIcon;
    [Tooltip("最大等级")] public int maxLevel;
    [Space(16)]
    [Tooltip("数值")] public AccessoryStats[] accessoryStats;   //数组索引表示饰品等级
    [Tooltip("被动技能")] public PassiveSkillType passiveSkillType;
    [Space(16)]
    [Tooltip("描述")][TextArea(3, 6)] public string desctiption;

    public int CompareTo(StaticAccessoryData other) => accessoryID - other.accessoryID;
}

public class LocalAccessoryData
{
    public int id;
    public int level;
    public int equipPosition;

    public LocalAccessoryData(int id, int level = 1, int equipPosition = 0)
    {
        this.id = id;
        this.level = level;
        this.equipPosition = equipPosition;
    }
}