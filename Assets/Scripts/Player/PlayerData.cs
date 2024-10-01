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
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
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
            currentMana = Mathf.Clamp(value, 0, maxMana);
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
            currentEnergy = value < maxEnergy ? value : maxEnergy;
        }
    }

    [Space(16)]

    [SerializeField][Tooltip("基础体力恢复速度")] private float basicEnergyRecovery;
    [Tooltip("基础体力恢复速度")]
    public float BasicEnergyRecovery
    {
        get => basicEnergyRecovery;
        set => basicEnergyRecovery = value;
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

    [SerializeField][Tooltip("基础穿透力")] private float basicPenetratingPower;
    [Tooltip("基础穿透力")]
    public float BasicDamageRate
    {
        get => basicPenetratingPower;
        set => basicPenetratingPower = value;
    }

    [SerializeField][Tooltip("基础伤害减免")] private float basicReductionRate;
    [Tooltip("基础伤害减免")]
    public float BasicReductionRate
    {
        get => basicReductionRate;
        set => basicReductionRate = value;
    }

    [Space(16)]

    [SerializeField][Tooltip("天赋体力恢复速度")] private float talentEnergyRecovery;
    [Tooltip("天赋体力恢复速度")]
    public float TalentEnergyRecovery
    {
        get => talentEnergyRecovery;
        set => talentEnergyRecovery = value;
    }

    [SerializeField][Tooltip("天赋攻击力")] private float talentDamage;
    [Tooltip("天赋攻击力")]
    public float TalentDamage
    {
        get => talentDamage;
        set => talentDamage = value;
    }

    [SerializeField][Tooltip("天赋暴击率")] private float talentCritRate;
    [Tooltip("天赋暴击率")]
    public float TalentCritRate
    {
        get => talentCritRate;
        set => talentCritRate = value;
    }

    [SerializeField][Tooltip("天赋暴击伤害")] private float talentCritDamage;
    [Tooltip("天赋暴击伤害")]
    public float TalentCritDamage
    {
        get => talentCritDamage;
        set => talentCritDamage = value;
    }

    [SerializeField][Tooltip("天赋穿透力")] private float talentPenetratingPower;
    [Tooltip("天赋穿透力")]
    public float TalentDamageRate
    {
        get => talentPenetratingPower;
        set => talentPenetratingPower = value;
    }

    [SerializeField][Tooltip("天赋伤害减免")] private float talentReductionRate;
    [Tooltip("天赋伤害减免")]
    public float TalentReducitonRate
    {
        get => talentReductionRate;
        set => talentReductionRate = value;
    }

    [Space(16)]

    [SerializeField][Tooltip("最终体力恢复速度")] private float finalEnergyRecovery;
    [Tooltip("最终体力恢复速度")]
    public float FinalEnergyRecovery
    {
        get => basicEnergyRecovery + talentEnergyRecovery;
    }

    [SerializeField][Tooltip("最终攻击力")] private float finalDamage;
    [Tooltip("最终攻击力")] 
    public float FinalDamage
    {
        get => basicDamage + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].damage + talentDamage;
    }

    [SerializeField][Tooltip("最终暴击率")] private float finalCritRate;
    [Tooltip("最终暴击率")]
    public float FinalCritRate
    {
        get => basicCritRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].critRate + talentCritRate;
    }

    [SerializeField][Tooltip("最终暴击伤害")] private float finalCritDamage;
    [Tooltip("最终暴击伤害")]
    public float FinalCritDamage
    {
        get => basicCritDamage + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].critDamage + talentCritDamage;
    }

    [SerializeField][Tooltip("最终穿透力")] private float finalPenetratingPower;
    [Tooltip("最终穿透力")]
    public float FinalDamageRate
    {
        get => basicPenetratingPower + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].penetratingPower + talentPenetratingPower;
    }

    [SerializeField][Tooltip("最终伤害减免")] private float finalReductionRate;
    [Tooltip("最终伤害减免")]
    public float FinalReducitonRate
    {
        get => basicReductionRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].reductionRate + talentReductionRate;
    }

    [Space(16)]
    [Header("武器")]
    [Space(16)]

    [Tooltip("当前武器静态数据")] public StaticWeaponData currentWeaponStaticData;
    [Tooltip("当前武器本地数据")] public LocalWeaponData currentWeaponLocalData;

    [Tooltip("当前饰品静态数据")] public Dictionary<int, StaticAccessoryData> currentAccessoryStaticData = new();
    [Tooltip("当前饰品本地数据")] public Dictionary<int, LocalAccessoryData> currentAccessoryLocalData = new();

    [Space(16)]
    [Header("倍率")]
    [Space(16)]

    public float[] motionValue;

    [Tooltip("")]

    #endregion

    private void Awake()
    {
        PackageManager.Instance.playerData = this;
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
