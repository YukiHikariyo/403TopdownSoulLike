using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 所有敌人的父类
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("基本属性")]
    [Space(16)]

    public bool isBoss;

    [Tooltip("最大血量")][SerializeField] private float maxHealth;
    [Tooltip("最大血量")]
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    [Tooltip("当前血量")][SerializeField] private float currentHealth; 
    [Tooltip("当前血量")]
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            //TODO: 血条UI处理
        }
    }

    [Tooltip("基础移速")][SerializeField] private float basicMoveSpeed;
    [Tooltip("基础移速")]
    public float BasicMoveSpeed
    {
        get => basicMoveSpeed;
        set => basicMoveSpeed = value;
    }

    [Tooltip("最终移速")]
    public float FinalMoveSpeed
    {
        get => (basicMoveSpeed + moveSpeedIncrement > 0 ? basicMoveSpeed + moveSpeedIncrement : 0) * moveSpeedMultiplication;
    }

    [SerializeField][Tooltip("基础攻击力")] private float basicDamage;
    [Tooltip("基础攻击力")]
    public float BasicDamage
    {
        get => basicDamage;
        set => basicDamage = value;
    }

    [Tooltip("最终攻击力")]
    public float FinalDamage
    {
        get => (basicDamage + damageIncrement > 0 ? basicDamage + damageIncrement : 0) * damageMultiplication;
    }

    [SerializeField][Tooltip("基础穿透力")] private float basicPenetratingPower;
    [Tooltip("基础穿透力")]
    public float BasicPenetratingPower
    {
        get => basicPenetratingPower;
        set => basicPenetratingPower = value;
    }

    [Tooltip("最终穿透力")]
    public float FinalPenetratingPower
    {
        get => (basicPenetratingPower + penetratingPowerIncrement > 0 ? basicPenetratingPower + penetratingPowerIncrement : 0) * penetratingPowerMultiplication;
    }

    [SerializeField][Tooltip("基础伤害减免")] private float basicReductionRate;
    [Tooltip("基础伤害减免")]
    public float BasicReductionRate
    {
        get => basicReductionRate;
        set => basicReductionRate = value;
    }

    [Tooltip("最终伤害减免")]
    public float FinalReducitonRate
    {
        get => (basicReductionRate + reductionRateIncrement > 0 ? basicReductionRate + reductionRateIncrement : 0) * reductionRateMultiplication;    
    }

    [Tooltip("基础韧性")][SerializeField] private float basicToughness;
    [Tooltip("基础韧性")]
    public float BasicToughness
    {
        get => basicToughness;
        set => basicToughness = value;
    }

    [SerializeField][Tooltip("动作韧性")] private float motionToughness;
    public float MotionToughness
    {
        get => motionToughness;
        set => motionToughness = value;
    }

    [Tooltip("最终韧性")]
    public float FinalToughness
    {
        get => (basicToughness + motionToughness + toughnessIncrement > 0 ? basicToughness + motionToughness + toughnessIncrement : 0) * toughnessMultiplication;
    }

    [Space(16)]

    [Tooltip("动作值")] public float[] motionValue;
    [Tooltip("攻击强度")] public float[] attackPower;
    [Tooltip("Buff动作值")] public float[] buffMotionValue;
    [Tooltip("可受击状态索引")] public int damageableIndex;    //0表示可受击，1表示无敌帧

    [Space(16)]
    [Header("Buff增量（可为负）")]
    [Space(16)]

    [Tooltip("移动速度增量")] public float moveSpeedIncrement;
    [Tooltip("攻击力增量")] public float damageIncrement;
    [Tooltip("穿透力增量")] public float penetratingPowerIncrement;
    [Tooltip("伤害减免增量")] public float reductionRateIncrement;
    [Tooltip("易伤增量")] public float vulnerabilityIncrement;
    [Tooltip("韧性增量")] public float toughnessIncrement;

    [Space(16)]
    [Header("Buff倍率")]
    [Space(16)]

    [Tooltip("移动速度倍率")] public float moveSpeedMultiplication = 1;
    [Tooltip("攻击力倍率")] public float damageMultiplication = 1;
    [Tooltip("穿透力倍率")] public float penetratingPowerMultiplication = 1;
    [Tooltip("伤害减免倍率")] public float reductionRateMultiplication = 1;
    [Tooltip("易伤倍率")] public float vulnerabilityMultiplication = 1;
    [Tooltip("韧性倍率")] public float toughnessMultiplication = 1;

    [Space(16)]
    [Header("Buff方法相关")]
    [Space(16)]

    private UnityAction buffAction;
    private Dictionary<BuffType, BaseBuff> currentBuffDict = new();
    private Dictionary<BuffType, Func<BuffType, float, CancellationToken, UniTask>> OnBuffFunc = new();
    private Dictionary<BuffType, CancellationTokenSource> buffCTK = new();

    [SerializeField] private float[] maxBuffHealth = new float[1];
    [SerializeField] private float[] currentBuffHealth = new float[1];

    private float buffDamageTimer;

    [Space(16)]
    [Header("受击事件")]
    [Space(16)]

    [Tooltip("是否霸体")] public bool isEnduance;

    [Space(16)]
    [Header("FSM")]
    [Space(16)]

    [Tooltip("能否行动")] public bool canTakeAction = true;
    public EnemyState currentState;
    public EnemyState defaultState;
    public EnemyState deadState;
    public EnemyState smallStunState;
    public EnemyState normalStunState;
    public EnemyState bigStunState;
    public EnemyState dizzyStunState;

    #region 生命周期

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //子类记得在此处实例化状态
        //子类记得在此处设置默认状态
    }

    private void OnEnable()
    {
        //currentState.OnEnter(false);
    }

    private void OnDisable()
    {
        //currentState.OnExit();
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        //currentState.PhysicsUpdate();
    }

    private void Update()
    {
        //currentState.LogicUpdate();

        buffAction?.Invoke();

        if (buffDamageTimer > 0)
            buffDamageTimer -= Time.deltaTime;
    }

    #endregion

    #region FSM

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            newState.lastState = currentState;
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter();
    }

    #endregion

    #region IDamageable接口方法

    public bool TakeDamage(float damage, float penetratingPower, float attackPower, Transform attackerTransform, bool ignoreDamageableIndex = false)
    {
        if (damageableIndex == 0 || ignoreDamageableIndex)
        {
            CurrentHealth -= Mathf.Ceil((damage + vulnerabilityIncrement > 0 ? damage + vulnerabilityIncrement : 0) * vulnerabilityMultiplication * Mathf.Clamp01(1 - (FinalReducitonRate - penetratingPower)) * UnityEngine.Random.Range(0.85f, 1.15f));
            if (CurrentHealth < 0)
            {
                ChangeState(deadState);
                return true;
            }

            float stunValue = attackPower - FinalToughness;
            if (isEnduance || stunValue <= 0)
                OnStunFunc(0).Forget();
            else if (stunValue > 0 && stunValue <= 10)
                OnStunFunc(1).Forget();
            else
                OnStunFunc(2).Forget();

            return true;
        }

        return false;
    }

    public bool TakeDamage(float damage, float penetratingPower, bool ignoreDamageableIndex = false)
    {
        if (damageableIndex == 0 || ignoreDamageableIndex)
        {
            CurrentHealth -= Mathf.Ceil((damage + vulnerabilityIncrement > 0 ? damage + vulnerabilityIncrement : 0) * vulnerabilityMultiplication * Mathf.Clamp01(1 - (FinalReducitonRate - penetratingPower)) * UnityEngine.Random.Range(0.85f, 1.15f));
            if (CurrentHealth < 0)
            {
                //TODO: 敌人死亡事件
            }
            return true;
        }

        return false;
    }

    public bool TakeBuffDamage(BuffType buffType, float damage, bool ignoreDamageableIndex = false)
    {
        if (buffDamageTimer > 0)
            return false;

        int buffIndex = buffType switch
        {
            BuffType.LightBurst => 0,

            _ => -1
        };

        if (!currentBuffDict.ContainsKey(buffType))
        {
            if (damageableIndex == 0 || ignoreDamageableIndex)
            {
                currentBuffHealth[buffIndex] += damage;
                if (currentBuffHealth[buffIndex] > 100)
                {
                    currentBuffHealth[buffIndex] = 0;
                    GetBuff(buffType);

                    buffDamageTimer = 1;
                    return true;
                }
            }
        }

        return false;
    }

    public void GetBuff(BuffType buffType, float duration = 0)
    {
        duration = buffType switch
        {
            BuffType.LightBurst => 1,

            _ => duration
        };  //设置某些Buff的固定时间

        if (!currentBuffDict.ContainsKey(buffType))
        {
            currentBuffDict.Add(buffType, buffType switch
            {
                BuffType.TestBuff => new TestBuff(duration, this),
                BuffType.Endurance => new Endurance(duration, this),
                BuffType.ShadowSneak => new ShadowSneak(duration, this),
                BuffType.Dizzy => new Dizzy(duration, this),
                BuffType.Burning => new Burning(duration, this),
                BuffType.LightBurst => new LightBurst(duration, this),

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
            0 => BuffType.LightBurst,

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

    #endregion

    private async UniTask OnStunFunc(int index)
    {
        if (index < 0 || index > 3)
            return;

        damageableIndex = 1;

        if (index != 0)
        {
            ChangeState(index switch
            {
                1 => smallStunState,
                2 => normalStunState,
                3 => bigStunState,

                _ => null
            });
        }
        
        await UniTask.Delay(TimeSpan.FromSeconds(0.15f));

        damageableIndex = 0;
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * FinalMoveSpeed;
    }
}
