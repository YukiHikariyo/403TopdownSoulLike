using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 玩家主体类
/// 用于联系其他玩家类
/// </summary>
public class Player : MonoBehaviour, IDamageable
{
    public PlayerData playerData;

    [Space(16)]
    [Header("固定属性")]
    [Space(16)]

    [Tooltip("动作值")] public float[] motionValue;
    [Tooltip("攻击强度")] public float[] attackPower;
    [Tooltip("Buff动作值")] public float[] buffMotionValue;
    [Tooltip("可受击状态索引")] public int damageableIndex;    //0表示可受击，1表示无敌帧，2表示见切判定窗口

    [Space(16)]
    [Header("Buff方法相关")]
    [Space(16)]

    private UnityAction buffAction;
    private Dictionary<BuffType, BaseBuff> currentBuffDict = new();
    private Dictionary<BuffType, float> maxBuffHealth = new();
    private Dictionary<BuffType, float> currentBuffHealth = new();
    private Dictionary<BuffType, Func<BuffType, float, CancellationToken, UniTask>> OnBuffFunc = new();
    private Dictionary<BuffType, CancellationTokenSource> buffCTK = new();

    [Space(16)]
    [Header("受击事件")]
    [Space(16)]

    [Tooltip("受击无硬直")] public UnityEvent<Transform> noStunEvent;
    [Tooltip("受击小硬直")] public UnityEvent<Transform> smallStunEvent;
    [Tooltip("受击中硬直")] public UnityEvent<Transform> normalStunEvent;
    [Tooltip("受击大硬直")] public UnityEvent<Transform> bigStunEvent;

    [Space(16)]
    [Header("被动技能")]
    [Space(16)]

    public Dictionary<PassiveSkillType, BasePassiveSkill> currentPassiveSkillDict = new();
    private Dictionary<PassiveSkillType, CancellationTokenSource> passiveSkillCTK = new();
    private Dictionary<PassiveSkillType, Func<float, CancellationToken, UniTask>> OnIntervalPassiveSkillFunc = new();
    public Dictionary<PlayerPassiveSkill.TriggerType, UnityAction> passiveSkillTriggerAction = new();

    #region 生命周期

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();

        //TODO: 初始化Buff血量
    }

    private void Update()
    {
        buffAction?.Invoke();
    }

    #endregion

    #region IDamageable接口方法

    public void TakeDamage(float damage, float penetratingPower,float attackPower, Transform attackerTransform, bool ignoreDamageableIndex = false)
    {
        TakeDamage(damage, penetratingPower, ignoreDamageableIndex);

        //玩家受击硬直部分，数值待定
        if (damageableIndex == 0 || ignoreDamageableIndex)
        {
            float stunValue = attackPower - playerData.FinalToughness;
            if (stunValue <= 0)
                noStunEvent?.Invoke(attackerTransform);
            else if (stunValue > 0 && stunValue <= 10)
                smallStunEvent?.Invoke(attackerTransform);
            else if (stunValue > 10 && stunValue <= 20)
                normalStunEvent?.Invoke(attackerTransform);
            else
                bigStunEvent?.Invoke(attackerTransform);
        }
    }

    public void TakeDamage(float damage, float penetratingPower, bool ignoreDamageableIndex = false)
    {
        if (damageableIndex == 0 || ignoreDamageableIndex)
            playerData.CurrentHealth -= (damage + playerData.vulnerabilityIncrement > 0 ? damage + playerData.vulnerabilityIncrement : 0) * playerData.vulnerabilityMultiplication * Mathf.Clamp01(1 - (playerData.FinalReducitonRate - penetratingPower));
    }

    public void TakeBuffDamage(BuffType buffType, float damage, bool ignoreDamageableIndex = false)
    {
        //以下为测试
        if (!currentBuffDict.ContainsKey(buffType))
        {
            if (damageableIndex == 0 || ignoreDamageableIndex)
            {
                currentBuffHealth[buffType] += damage;
                if (currentBuffHealth[buffType] > maxBuffHealth[buffType])
                {
                    currentBuffHealth[buffType] = maxBuffHealth[buffType];
                    GetBuff(buffType, 30);
                }
            }
        }
    }

    public void GetBuff(BuffType buffType, float duration)
    {
        if (!currentBuffDict.ContainsKey(buffType))
        {
            currentBuffDict.Add(buffType, buffType switch
            {
                BuffType.TestBuff => new TestBuff(duration, this),

                _ => null
            });

            if (!buffCTK.ContainsKey(buffType))
                buffCTK.Add(buffType, new());
            else
                buffCTK[buffType] = new();

            if (!OnBuffFunc.ContainsKey(buffType))
            {
                OnBuffFunc.Add(buffType, async (buffType, duration, ctk) =>
                {
                    if (currentBuffDict.ContainsKey(buffType))
                    {
                        currentBuffDict[buffType].OnBuffEnter();
                        buffAction += currentBuffDict[buffType].OnPlayerBuffStay;
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: ctk);

                    if (currentBuffDict.ContainsKey(buffType))
                    {
                        buffAction -= currentBuffDict[buffType].OnPlayerBuffStay;
                        currentBuffDict[buffType].OnBuffExit();
                        currentBuffDict.Remove(buffType);
                    }
                });
            }

            OnBuffFunc[buffType].Invoke(buffType, duration, buffCTK[buffType].Token);
        }
    }

    public void RemoveBuff(BuffType buffType)
    {
        if (currentBuffDict.ContainsKey(buffType))
        {
            if (buffCTK.ContainsKey(buffType))
                buffCTK[buffType].Cancel();

            buffAction -= currentBuffDict[buffType].OnPlayerBuffStay;
            currentBuffDict[buffType].OnBuffExit();
            currentBuffDict.Remove(buffType);
        }
    }

    [ContextMenu("给予测试Buff1")]
    public void GetTestBuff1() => GetBuff(BuffType.TestBuff, 3);

    [ContextMenu("移除测试Buff1")]
    public void RemoveTestBuff1() => RemoveBuff(BuffType.TestBuff);

    #endregion

    #region 被动技能方法

    public void GetPassiveSkill(PassiveSkillType skillType)
    {
        if (!currentPassiveSkillDict.ContainsKey(skillType))
        {
            currentPassiveSkillDict.Add(skillType, skillType switch
            {
                PassiveSkillType.TestPassiveSkill => new TestPassiveSkill(this),

                _ => null
            });

            currentPassiveSkillDict[skillType].OnEnter();

            if (currentPassiveSkillDict[skillType].triggerType == PlayerPassiveSkill.TriggerType.Interval)
            {
                if (!passiveSkillCTK.ContainsKey(skillType))
                    passiveSkillCTK.Add(skillType, new());
                else
                    passiveSkillCTK[skillType] = new();

                if (!OnIntervalPassiveSkillFunc.ContainsKey(skillType))
                {
                    OnIntervalPassiveSkillFunc.Add(skillType, async (interval, ctk) =>
                    {
                        while (true)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: ctk);
                            currentPassiveSkillDict[skillType].OnTrigger();
                        }
                    });
                }

                OnIntervalPassiveSkillFunc[skillType].Invoke(currentPassiveSkillDict[skillType].interval, passiveSkillCTK[skillType].Token);
            }
            else
            {
                if (!passiveSkillTriggerAction.ContainsKey(currentPassiveSkillDict[skillType].triggerType))
                    passiveSkillTriggerAction.Add(currentPassiveSkillDict[skillType].triggerType, currentPassiveSkillDict[skillType].OnTrigger);
                else
                    passiveSkillTriggerAction[currentPassiveSkillDict[skillType].triggerType] += currentPassiveSkillDict[skillType].OnTrigger;
            }
        }
    }

    public void RemovePassiveSkill(PassiveSkillType skillType)
    {
        if (currentPassiveSkillDict.ContainsKey(skillType))
        {
            if (currentPassiveSkillDict[skillType].triggerType == PlayerPassiveSkill.TriggerType.Interval)
            {
                if (passiveSkillCTK.ContainsKey(skillType))
                    passiveSkillCTK[skillType].Cancel();
            }    
            else
                passiveSkillTriggerAction[currentPassiveSkillDict[skillType].triggerType] -= currentPassiveSkillDict[skillType].OnTrigger;

            currentPassiveSkillDict[skillType].OnExit();
            currentPassiveSkillDict.Remove(skillType);
        }
    }

    [ContextMenu("获得测试被动技能")]
    public void GetTestPassiveSkill() => GetPassiveSkill(PassiveSkillType.TestPassiveSkill);

    [ContextMenu("移除测试被动技能")]
    public void RemoveTestPassiveSkill() => RemovePassiveSkill(PassiveSkillType.TestPassiveSkill);

    #endregion
}


