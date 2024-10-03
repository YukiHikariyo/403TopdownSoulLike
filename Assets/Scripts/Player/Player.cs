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

    private UnityEvent buffEvent = new();
    private Dictionary<BuffType, BaseBuff> currentBuffDict = new();
    private Dictionary<BuffType, float> maxBuffHealth = new();
    private Dictionary<BuffType, float> currentBuffHealth = new();
    private Dictionary<BuffType, Func<BuffType, float, CancellationToken, UniTaskVoid>> OnBuffFunc = new();
    private Dictionary<BuffType, CancellationTokenSource> buffCTK = new();

    [Space(16)]
    [Header("固定属性")]
    [Space(16)]

    [Tooltip("动作值")] public float[] motionValue;
    [Tooltip("攻击强度")] public float[] attackPower;
    [Tooltip("Buff动作值")] public float[] buffMotionValue;

    [Space(16)]
    [Header("受击事件")]
    [Space(16)]

    [Tooltip("受击无硬直")] public UnityEvent<Transform> noStunEvent;
    [Tooltip("受击小硬直")] public UnityEvent<Transform> smallStunEvent;
    [Tooltip("受击中硬直")] public UnityEvent<Transform> normalStunEvent;
    [Tooltip("受击大硬直")] public UnityEvent<Transform> bigStunEvent;

    private void Awake()
    {
        if (playerData == null)
            playerData = GetComponent<PlayerData>();
    }

    private void Update()
    {
        buffEvent?.Invoke();
    }

    public void TakeDamage(float damage, float penetratingPower,float attackPower, Transform attackerTransform)
    {
        TakeDamage(damage, penetratingPower);

        //玩家受击硬直部分，数值待定
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

    public void TakeDamage(float damage, float penetratingPower)
    {
        playerData.CurrentHealth -= (damage + playerData.vulnerabilityIncrement > 0 ? damage + playerData.vulnerabilityIncrement : 0) * playerData.vulnerabilityMultiplication * Mathf.Clamp01(1 - (playerData.FinalReducitonRate - penetratingPower));
    }

    public void TakeBuffDamage(BuffType buffType, float damage)
    {
        if (!currentBuffDict.ContainsKey(buffType))
        {
            currentBuffHealth[buffType] += damage;
            if (currentBuffHealth[buffType] > maxBuffHealth[buffType])
            {
                currentBuffHealth[buffType] = maxBuffHealth[buffType];
                GetBuff(buffType, 30);
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
                        buffEvent.AddListener(currentBuffDict[buffType].OnBuffStay);
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: ctk);

                    if (currentBuffDict.ContainsKey(buffType))
                    {
                        buffEvent.RemoveListener(currentBuffDict[buffType].OnBuffStay);
                        currentBuffDict[buffType].OnBuffExit();
                        currentBuffDict.Remove(buffType);
                    }
                });
            }

            OnBuffFunc[buffType].Invoke(buffType, duration, buffCTK[buffType].Token);
        }
    }

    [ContextMenu("给予测试Buff1")]
    public void GetTestBuff1() => GetBuff(BuffType.TestBuff, 3);

    public void RemoveBuff(BuffType buffType)
    {
        if (currentBuffDict.ContainsKey(buffType))
        {
            if (buffCTK.ContainsKey(buffType))
                buffCTK[buffType].Cancel();

            currentBuffDict[buffType].OnBuffExit();
            currentBuffDict.Remove(buffType);
        }
    }

    [ContextMenu("移除测试Buff1")]
    public void RemoveTestBuff1() => RemoveBuff(BuffType.TestBuff);
}
