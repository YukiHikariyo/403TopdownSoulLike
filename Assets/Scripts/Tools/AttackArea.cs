using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 攻击范围类
/// 挂在要造成伤害的有Trigger的物体上
/// </summary>
public class AttackArea : MonoBehaviour
{
    public enum AttackerType
    {
        NoSource,
        Player,
        Enemy,
        Trap,
    }

    [Tooltip("攻击者类别")] public AttackerType attackerType;
    [Space(16)]
    public Player player;
    public Enemy enemy;
    [Space(16)]
    [Tooltip("造成属性伤害")] public bool causeBuffDamage;
    [Tooltip("攻击者是否为弹幕")] public bool isBullet;
    [Tooltip("是否无视可受击状态")] public bool ignoreDamageableIndex;
    [Space(16)]
    [Tooltip("动作值索引")] public int motionValueIndex;
    [Tooltip("攻击强度索引")] public int attackPowerIndex;
    [Tooltip("属性动作值索引")] public int buffMotionValueIndex;
    [Tooltip("属性伤害的类型")] public BuffType buffType;
    [Space(16)]
    [Tooltip("无来源伤害")] public float noSourceDamage;
    [Tooltip("无来源穿透力")] public float noSourcePenetratingPower;
    [Tooltip("无来源属性伤害")] public float noSourceBuffDamage;
    [Space(16)]
    [Tooltip("判定间隔")] public float damageInterval = 1;
    private float timer;
    [Space(16)]
    [Tooltip("成功造成伤害后触发的事件")] public UnityEvent<IDamageable> successEvent;

    private void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timer <= 0 && collision.TryGetComponent<IDamageable>(out IDamageable component))
        {
            IDamageable damageable = component;
            bool isSuccessful = false;

            switch (attackerType)
            {
                case AttackerType.NoSource:
                    isSuccessful = damageable.TakeDamage(noSourceDamage, noSourcePenetratingPower, ignoreDamageableIndex);
                    if (causeBuffDamage)
                        damageable.TakeBuffDamage(buffType, noSourceBuffDamage, ignoreDamageableIndex);
                    if (isSuccessful)
                        successEvent?.Invoke(damageable);
                    break;
                case AttackerType.Player:
                    isSuccessful = damageable.TakeDamage(player.playerData.FinalDamage * player.motionValue[motionValueIndex], player.playerData.FinalPenetratingPower, player.attackPower[attackPowerIndex], isBullet ? transform : player.transform, ignoreDamageableIndex);
                    if (causeBuffDamage)
                        damageable.TakeBuffDamage(buffType, player.playerData.FinalBuffDamage * player.buffMotionValue[buffMotionValueIndex], ignoreDamageableIndex);
                    if (isSuccessful)
                        successEvent?.Invoke(damageable);
                    break;
                case AttackerType.Enemy:
                    isSuccessful = damageable.TakeDamage(enemy.FinalDamage * enemy.motionValue[motionValueIndex], enemy.FinalPenetratingPower, enemy.attackPower[attackPowerIndex], isBullet ? transform : enemy.transform, ignoreDamageableIndex);
                    if (causeBuffDamage)
                        damageable.TakeBuffDamage(buffType, enemy.FinalBuffDamage * enemy.buffMotionValue[buffMotionValueIndex], ignoreDamageableIndex);
                    if (isSuccessful)
                        successEvent?.Invoke(damageable);
                    break;
                case AttackerType.Trap:
                    //TODO: 陷阱攻击
                    break;
                default:
                    break;
            }

            timer = damageInterval;
        }
    }
}
