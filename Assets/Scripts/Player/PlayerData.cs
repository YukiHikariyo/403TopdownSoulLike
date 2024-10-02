using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, ISaveable
{
    #region 变量与属性

    [Header("基本数值")]
    [Space(16)]

    [SerializeField][Tooltip("基础最大生命值")] private float basicMaxHealth;
    [Tooltip("基础最大生命值")]
    public float BasicMaxHealth
    {
        get => basicMaxHealth;
        set
        {
            float percent = currentHealth / FinalMaxHealth;
            basicMaxHealth = value;
            CurrentHealth = FinalMaxHealth * percent;
        }
    }

    [Tooltip("最终最大生命值")]
    public float FinalMaxHealth
    {
        get
        {
            float finalMaxHealth = basicMaxHealth + maxHealthIncrement;
            for (int i = 1; i <= 3; i++)
            {
                if (currentAccessoryLocalData.ContainsKey(i))
                    finalMaxHealth += currentAccessoryStaticData[i].accessoryStats[currentAccessoryLocalData[i].level - 1].maxHealth;
            }
            return finalMaxHealth * maxHealthMultiplication;
        }
    }

    [SerializeField][Tooltip("当前生命值")] private float currentHealth;
    [Tooltip("当前生命值")]
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, FinalMaxHealth);
        }
    }

    [SerializeField][Tooltip("基础最大魔力值")] private float basicMaxMana;
    [Tooltip("基础最大魔力值")]
    public float BasicMaxMana
    {
        get => basicMaxMana;
        set
        {
            float percent = currentMana / FinalMaxMana;
            basicMaxMana = value;
            CurrentMana = FinalMaxMana * percent;
        }
    }

    [Tooltip("最终最大魔力值")]
    public float FinalMaxMana
    {
        get
        {
            float finalMaxMana = basicMaxMana + maxManaIncrement;
            for (int i = 1; i <= 3; i++)
            {
                if (currentAccessoryLocalData.ContainsKey(i))
                    finalMaxMana += currentAccessoryStaticData[i].accessoryStats[currentAccessoryLocalData[i].level - 1].maxMana;
            }
            return finalMaxMana * maxManaMultiplication;
        }
    }

    [SerializeField][Tooltip("当前魔力值")] private float currentMana;
    [Tooltip("当前魔力值")]
    public float CurrentMana
    {
        get => currentMana;
        set
        {
            currentMana = Mathf.Clamp(value, 0, FinalMaxMana);
        }
    }

    [SerializeField][Tooltip("基础最大体力值")] private float basicMaxEnergy;

    [Tooltip("基础最大体力值")]
    public float BasicMaxEnergy
    {
        get => basicMaxEnergy;
        set
        {
            float percent = basicMaxEnergy / FinalMaxEnergy;
            basicMaxEnergy = value;
            CurrentEnergy = FinalMaxEnergy * percent;
        }
    }

    [Tooltip("最终最大体力值")]
    public float FinalMaxEnergy
    {
        get
        {
            float finalMaxEnergy = basicMaxEnergy + maxEnergyIncrement;
            for (int i = 1; i <= 3; i++)
            {
                if (currentAccessoryLocalData.ContainsKey(i))
                    finalMaxEnergy += currentAccessoryStaticData[i].accessoryStats[currentAccessoryLocalData[i].level - 1].maxEnergy;
            }
            return finalMaxEnergy + maxEnergyMultiplication;
        }
    }

    [SerializeField][Tooltip("当前体力值")] private float currentEnergy;
    [Tooltip("当前体力值")]
    public float CurrentEnergy
    {
        get => currentEnergy;
        set
        {
            currentEnergy = value < FinalMaxEnergy ? value : FinalMaxEnergy;
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
    public float BasicPenetratingPower
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

    [SerializeField][Tooltip("基础韧性")] private float basicToughness;
    [Tooltip("基础韧性")]
    public float BasicToughness
    {
        get => basicToughness;
        set => basicToughness = value;
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
    public float TalentPenetratingPower
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

    [SerializeField][Tooltip("动作韧性")] private float motionToughness;
    public float MotionToughness
    {
        get => motionToughness;
        set => motionToughness = value;
    }

    [Tooltip("最终体力恢复速度")]
    public float FinalEnergyRecovery
    {
        get => (basicEnergyRecovery + talentEnergyRecovery + energyRecoveryIncrement > 0 ? basicEnergyRecovery + talentEnergyRecovery + energyRecoveryIncrement : 0) * energyRecoveryMultiplication;
    }

    [Tooltip("最终攻击力")] 
    public float FinalDamage
    {
        get => (basicDamage + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].damage + talentDamage + damageIncrement > 0 ? basicDamage + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].damage + talentDamage + damageIncrement : 0) * damageMultiplication;
    }

    [Tooltip("最终暴击率")]
    public float FinalCritRate
    {
        get => (basicCritRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].critRate + talentCritRate + critRateIncrement > 0 ? basicCritRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].critRate + talentCritRate + critRateIncrement : 0) * critRateMultiplication;
    }

    [Tooltip("最终暴击伤害")]
    public float FinalCritDamage
    {
        get => (basicCritDamage + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].critDamage + talentCritDamage + critDamageIncrement > 0 ? basicCritDamage + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].critDamage + talentCritDamage + critDamageIncrement : 0) * critDamageMultiplication;
    }

    [Tooltip("最终穿透力")]
    public float FinalPenetratingPower
    {
        get => (basicPenetratingPower + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].penetratingPower + talentPenetratingPower + penetratingPowerIncrement > 0 ? basicPenetratingPower + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].penetratingPower + talentPenetratingPower + penetratingPowerIncrement : 0) * penetratingPowerMultiplication;
    }

    [Tooltip("最终伤害减免")]
    public float FinalReducitonRate
    {
        get => (basicReductionRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].reductionRate + talentReductionRate + reductionIncrement > 0 ? basicReductionRate + currentWeaponStaticData.weaponStats[currentWeaponLocalData.level - 1].reductionRate + talentReductionRate + reductionIncrement : 0) * reductionMultiplication;
    }

    [Tooltip("最终韧性")]
    public float FinalToughness
    {
        get
        {
            float finalToughness = basicToughness + motionToughness + toughnessIncrement;
            for (int i = 1; i <= 3; i++)
            {
                if (currentAccessoryLocalData.ContainsKey(i))
                    finalToughness += currentAccessoryStaticData[i].accessoryStats[currentAccessoryLocalData[i].level - 1].toughness;
            }
            return finalToughness * toughnessMultiplication;
        }
    }

    [Space(16)]

    [Tooltip("动作值")] public float[] motionValue;

    [Space(16)]
    [Header("Buff增量（可为负）")]
    [Space(16)]

    //修改以下三个增量时记得调用OnMax???Change
    [Tooltip("最大生命值增量")] public float maxHealthIncrement;
    [Tooltip("最大魔力值增量")] public float maxManaIncrement;
    [Tooltip("最大体力值增量")] public float maxEnergyIncrement;
    //End
    [Tooltip("体力恢复增量")] public float energyRecoveryIncrement;
    [Tooltip("体力消耗增量")] public float energyCostIncrement;
    [Tooltip("移动速度增量")] public float moveSpeedIncrement;
    [Tooltip("攻击力增量")] public float damageIncrement;
    [Tooltip("暴击率增量")] public float critRateIncrement;
    [Tooltip("暴击伤害增量")] public float critDamageIncrement;
    [Tooltip("穿透力增量")] public float penetratingPowerIncrement;
    [Tooltip("伤害减免增量")] public float reductionIncrement;
    [Tooltip("韧性增量")] public float toughnessIncrement;

    [Space(16)]
    [Header("Buff倍率")]
    [Space(16)]

    //修改以下三个倍率时记得调用OnMax???Change
    [Tooltip("最大生命值倍率")] public float maxHealthMultiplication = 1;
    [Tooltip("最大魔力值倍率")] public float maxManaMultiplication = 1;
    [Tooltip("最大体力值倍率")] public float maxEnergyMultiplication = 1;
    //End
    [Tooltip("体力恢复倍率")] public float energyRecoveryMultiplication = 1;
    [Tooltip("体力消耗倍率")] public float energyCostMultiplication = 1;
    [Tooltip("移动速度倍率")] public float moveSpeedMultiplication = 1;
    [Tooltip("攻击力倍率")] public float damageMultiplication = 1;
    [Tooltip("暴击率倍率")] public float critRateMultiplication = 1;
    [Tooltip("暴击伤害倍率")] public float critDamageMultiplication = 1;
    [Tooltip("穿透力倍率")] public float penetratingPowerMultiplication = 1;
    [Tooltip("伤害减免倍率")] public float reductionMultiplication = 1;
    [Tooltip("韧性倍率")] public float toughnessMultiplication = 1;

    [Tooltip("当前武器静态数据")][HideInInspector] public StaticWeaponData currentWeaponStaticData;
    [Tooltip("当前武器本地数据")] public LocalWeaponData currentWeaponLocalData;

    [Tooltip("当前饰品静态数据")] public Dictionary<int, StaticAccessoryData> currentAccessoryStaticData = new();
    [Tooltip("当前饰品本地数据")] public Dictionary<int, LocalAccessoryData> currentAccessoryLocalData = new();

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

    public void OnMaxHealthChange(float percent)
    {
        currentHealth = FinalMaxHealth * percent;
        //TODO: UI更新
    }

    public void OnMaxManaChange(float percent)
    {
        currentMana = FinalMaxMana * percent;
        //TODO: UI更新
    }
    public void OnMaxEnergyChange(float percent)
    {
        currentEnergy = FinalMaxEnergy * percent;
        //TODO: UI更新
    }
}
