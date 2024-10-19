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
    public PlayerController playerController;

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
    private Dictionary<BuffType, Func<BuffType, float, CancellationToken, UniTask>> OnBuffFunc = new();
    private Dictionary<BuffType, CancellationTokenSource> buffCTK = new();
    private PlayerBar[] buffBars;

    public float[] currentBuffHealth = new float[3];
    private float[] buffHealthReduceRate = new float[3] { 10, 40, 100 };
    private bool[] isBuffStay = new bool[3];
    private int buffIndex;

    private float buffDamageTimer;

    [Space(16)]
    [Header("受击事件")]
    [Space(16)]

    [Tooltip("是否霸体")] public bool isEnduance;
    [Tooltip("受击无硬直")] public UnityEvent<Transform> noStunEvent;
    [Tooltip("受击小硬直")] public UnityEvent<Transform> smallStunEvent;
    [Tooltip("受击中硬直")] public UnityEvent<Transform> normalStunEvent;
    [Tooltip("受击大硬直")] public UnityEvent<Transform> bigStunEvent;
    [Tooltip("见切成功")] public UnityEvent foresightEvent;
    [Tooltip("玩家死亡")] public UnityEvent deathEvent;

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
        playerController = GetComponent<PlayerController>();

        buffBars = UIManager.Instance.buffBars;
    }

    private void Update()
    {
        buffAction?.Invoke();
        BuffHealthAction();

        if (buffDamageTimer > 0)
            buffDamageTimer -= Time.deltaTime;
    }

    #endregion

    #region IDamageable接口方法

    public bool TakeDamage(float damage, float penetratingPower,float attackPower, Transform attackerTransform, bool ignoreDamageableIndex = false)
    {
        //玩家受击硬直部分，数值待定
        if (damageableIndex == 0 || ignoreDamageableIndex)
        {
            playerData.CurrentHealth -= Mathf.Ceil((damage + playerData.vulnerabilityIncrement > 0 ? damage + playerData.vulnerabilityIncrement : 0) * playerData.vulnerabilityMultiplication * Mathf.Clamp01(1 - (playerData.FinalReducitonRate - penetratingPower)) * UnityEngine.Random.Range(0.85f, 1.15f));
            if (playerData.CurrentHealth <= 0)
            {
                deathEvent?.Invoke();
                return true;
            }

            float stunValue = attackPower - playerData.FinalToughness;
            if (isEnduance || stunValue <= 0)
                noStunEvent?.Invoke(attackerTransform);
            else if (stunValue > 0 && stunValue <= 10)
                smallStunEvent?.Invoke(attackerTransform);
            else if (stunValue > 10 && stunValue <= 20)
                normalStunEvent?.Invoke(attackerTransform);
            else
                bigStunEvent?.Invoke(attackerTransform);

            return true;
        }
        else if (damageableIndex == 2)
        {
            foresightEvent?.Invoke();
            return false;
        }

        return false;
    }

    public bool TakeDamage(float damage, float penetratingPower, bool ignoreDamageableIndex = false)
    {
        if (damageableIndex == 0 || ignoreDamageableIndex)
        {
            playerData.CurrentHealth -= Mathf.Ceil((damage + playerData.vulnerabilityIncrement > 0 ? damage + playerData.vulnerabilityIncrement : 0) * playerData.vulnerabilityMultiplication * Mathf.Clamp01(1 - (playerData.FinalReducitonRate - penetratingPower)) * UnityEngine.Random.Range(0.85f, 1.15f));
            if (playerData.CurrentHealth <= 0)
                deathEvent?.Invoke();
            return true;
        }

        return false;
    }

    public bool TakeBuffDamage(BuffType buffType, float damage, bool ignoreDamageableIndex = false)
    {
        if (buffDamageTimer > 0)
            return false;

        int buffBarIndex = buffType switch
        {
            BuffType.Cold => 0,
            BuffType.Frozen => 1,
            BuffType.DarkErosion => 2,

            _ => -1
        };
        PlayerBar buffBar = buffBars[buffBarIndex];

        if (!currentBuffDict.ContainsKey(buffType))
        {
            if (damageableIndex == 0 || ignoreDamageableIndex)
            {
                if (!buffBar.gameObject.activeSelf)
                    buffBar.gameObject.SetActive(true);

                currentBuffHealth[buffBarIndex] += damage;
                buffBar.OnCurrentValueChange(currentBuffHealth[buffBarIndex], 100, true);

                if (currentBuffHealth[buffBarIndex] >= 100)
                {
                    currentBuffHealth[buffBarIndex] = 100;
                    isBuffStay[buffBarIndex] = true;
                    GetBuff(buffType);
                }

                buffDamageTimer = 1;
                return true;
            }
        }

        return false;
    }

    public void GetBuff(BuffType buffType, float duration = 0)
    {
        duration = buffType switch
        {
            BuffType.Cold => 10,
            BuffType.Frozen => 3,
            BuffType.DarkErosion => 1,

            _ => duration
        };  //设置某些Buff的固定时间

        if (!currentBuffDict.ContainsKey(buffType))
        {
            currentBuffDict.Add(buffType, buffType switch
            {
                BuffType.TestBuff => new TestBuff(duration, this),
                BuffType.Tough => new Tough(duration, this),
                BuffType.Foresight => new Foresight(duration, this),
                BuffType.ForesightEndurance => new ForesightEndurance(duration, this),
                BuffType.Hot => new Hot(duration, this),
                BuffType.Cold => new Cold(duration, this),
                BuffType.Frozen => new Frozen(duration, this),
                BuffType.IceBurning => new IceBurning(duration, this),
                BuffType.DarkErosion => new DarkErosion(duration, this),

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
                        buffAction += currentBuffDict[buffType].OnBuffStay;
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: ctk);

                    if (currentBuffDict.ContainsKey(buffType))
                    {
                        buffAction -= currentBuffDict[buffType].OnBuffStay;
                        currentBuffDict[buffType].OnBuffExit();
                        currentBuffDict.Remove(buffType);
                    }
                });
            }

            OnBuffFunc[buffType].Invoke(buffType, duration, buffCTK[buffType].Token);
        }
    }

    public void RemoveBuff(BuffType buffType, int index = -1)
    {
        buffType = index switch
        {
            0 => BuffType.Cold,
            1 => BuffType.Frozen,
            2 => BuffType.DarkErosion,

            _ => buffType
        };

        if (currentBuffDict.ContainsKey(buffType))
        {
            if (buffCTK.ContainsKey(buffType))
                buffCTK[buffType].Cancel();

            buffAction -= currentBuffDict[buffType].OnBuffStay;
            currentBuffDict[buffType].OnBuffExit();
            currentBuffDict.Remove(buffType);
        }
    }

    public void BuffHealthAction()
    {
        for (buffIndex = 0; buffIndex < currentBuffHealth.Length; buffIndex++)
        {
            if (currentBuffHealth[buffIndex] > 0 && !isBuffStay[buffIndex])
            {
                currentBuffHealth[buffIndex] -= 2.5f * Time.deltaTime;
                buffBars[buffIndex].OnCurrentValueChange(currentBuffHealth[buffIndex], 100, true);

                if (currentBuffHealth[buffIndex] <= 0)
                {
                    currentBuffHealth[buffIndex] = 0;
                    buffBars[buffIndex].gameObject.SetActive(false);
                }
            }
            else if (currentBuffHealth[buffIndex] > 0 && isBuffStay[buffIndex])
            {
                currentBuffHealth[buffIndex] -= buffHealthReduceRate[buffIndex] * Time.deltaTime;
                buffBars[buffIndex].OnCurrentValueChange(currentBuffHealth[buffIndex], 100, true);

                if (currentBuffHealth[buffIndex] <= 0)
                {
                    currentBuffHealth[buffIndex] = 0;
                    buffBars[buffIndex].gameObject.SetActive(false);
                    isBuffStay[buffIndex] = false;
                }
            }
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
                PassiveSkillType.ForesightEnhance => new ForesightEnhance(this),
                PassiveSkillType.LongerForesightEnhance => new LongerForesightEnhance(this),
                PassiveSkillType.LongerForesightTime => new LongerForesightTime(this),
                PassiveSkillType.FastCharge => new FastCharge(this),
                PassiveSkillType.FasterCharge => new FasterCharge(this),
                PassiveSkillType.FastestCharge => new FastestCharge(this),

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


