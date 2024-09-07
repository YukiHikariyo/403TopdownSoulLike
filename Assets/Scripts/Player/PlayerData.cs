using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, ISaveable
{
    #region 变量与属性

    [Header("基本数值")]
    [Space(16)]

    [SerializeField][Tooltip("最大生命值")] private float maxHealth;
    [Tooltip("最大生命值")]
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
        }
    }

    [SerializeField][Tooltip("当前生命值")] private float currentHealth;
    [Tooltip("当前生命值")]
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value > 0 ? value : 0;
        }
    }

    [SerializeField][Tooltip("最大魔力值")] private float maxMana;
    [Tooltip("最大魔力值")]
    public float MaxMana
    {
        get => maxMana;
        set
        {
            maxMana = value;
        }
    }

    [SerializeField][Tooltip("当前魔力值")] private float currentMana;
    [Tooltip("当前魔力值")]
    public float CurrentMana
    {
        get => currentMana;
        set
        {
            currentMana = value > 0 ? value : 0;
        }
    }

    [SerializeField][Tooltip("最大体力值")] private float maxEnergy;

    [Tooltip("最大体力值")]
    public float MaxEnergy
    {
        get => maxEnergy;
        set
        {
            maxEnergy = value;
        }
    }

    [SerializeField][Tooltip("当前体力值")] private float currentEnergy;
    [Tooltip("当前体力值")]
    public float CurrentEnergy
    {
        get => currentEnergy;
        set
        {
            currentEnergy = value;
        }
    }

    [SerializeField][Tooltip("基础攻击力")] private float basicDamage;
    [Tooltip("基础攻击力")]
    public float BasicDamage
    {
        get => basicDamage;
        set => basicDamage = value;
    }

    [SerializeField][Tooltip("基础暴击率")] private float basicCritRate;
    [Tooltip("基础暴击率")]
    public float BasicCritRate
    {
        get => basicCritRate;
        set => basicCritRate = value;
    }

    [SerializeField][Tooltip("基础暴击伤害")] private float basicCritDamage;
    [Tooltip("基础暴击伤害")]
    public float BasicCritDamage
    {
        get => basicCritDamage;
        set => basicCritDamage = value;
    }

    [SerializeField][Tooltip("基础攻击倍率")] private float basicDamageRate;
    [Tooltip("基础攻击倍率")]
    public float BasicDamageRate
    {
        get => basicDamageRate;
        set => basicDamageRate = value;
    }

    [SerializeField][Tooltip("基础伤害减免")] private float basicReductionRate;
    [Tooltip("基础伤害减免")]
    public float BasicReductionRate
    {
        get => basicReductionRate;
        set => basicReductionRate = value;
    }

    [Space(16)]

    [SerializeField][Tooltip("最终攻击力")] private float finalDamage;
    [Tooltip("最终攻击力")] 
    public float FinalDamage
    {
        get => basicDamage + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].damage;
    }

    [SerializeField][Tooltip("最终暴击率")] private float finalCritRate;
    [Tooltip("最终暴击率")]
    public float FinalCritRate
    {
        get => basicCritRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].critRate;
    }

    [SerializeField][Tooltip("最终暴击上海")] private float finalCritDamage;
    [Tooltip("最终暴击伤害")]
    public float FinalCritDamage
    {
        get => basicCritDamage + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].critDamage;
    }

    [SerializeField][Tooltip("最终攻击倍率")] private float finalDamageRate;
    [Tooltip("最终攻击倍率")]
    public float FinalDamageRate
    {
        get => basicDamageRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].damageRate;
    }

    [SerializeField][Tooltip("最终伤害减免")] private float finalReductionRate;
    [Tooltip("最终伤害减免")]
    public float FinalReducitonRate
    {
        get => basicReductionRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].reductionRate;
    }

    [Space(16)]
    [Header("武器")]
    [Space(16)]

    [Tooltip("当前武器静态数据")] public StaticWeaponData currentWeaponStaticData;
    [Tooltip("当前武器本地数据")] public LocalWeaponData currentWeaponLocalData;

    #endregion

    private void Awake()
    {
        PackageManager.Instance.playerData = this;
        if (PackageManager.Instance.currentWeapon == null)
            PackageManager.Instance.EquipWeapon(0);
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
        
    }

    public void LoadSaveData(SaveData saveData)
    {
        
    }
}
