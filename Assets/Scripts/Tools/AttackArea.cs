using BehaviorDesigner.Runtime;
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
        PlayerBlade,
        PlayerOther,
        Enemy,
        Trap,
    }

    [Tooltip("攻击者类别")] public AttackerType attackerType;
    [Space(16)]
    public Player player;
    public Enemy enemy;
    [Space(16)]
    [Tooltip("造成血量伤害")] public bool causeHealthDamage;
    [Tooltip("造成属性伤害")] public bool causeBuffDamage;
    [Tooltip("直接施加Buff")] public bool directlyAssertBuff;
    [Tooltip("直接施加Buff的概率")] public float directBuffProbability;
    [Tooltip("攻击者是否为弹幕")] public bool isBullet;
    [Tooltip("是否无视可受击状态")] public bool ignoreDamageableIndex;
    [Space(16)]
    [Tooltip("动作值索引")] public int motionValueIndex;
    [Tooltip("攻击强度索引")] public int attackPowerIndex;
    [Tooltip("属性动作值索引")] public int buffMotionValueIndex;
    [Tooltip("属性伤害的类型（玩家刀光攻击不用管）")] public BuffType buffType;
    [Tooltip("直接施加Buff的时间")] public float directBuffDuration;
    [Space(16)]
    [Tooltip("无来源伤害")] public float noSourceDamage;
    [Tooltip("无来源穿透力")] public float noSourcePenetratingPower;
    [Tooltip("无来源属性伤害")] public float noSourceBuffDamage;
    [Space(16)]
    [Tooltip("成功造成伤害后触发的事件")] public UnityEvent<IDamageable> successEvent;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable component))
        {
            IDamageable damageable = component;
            bool isSuccessful = false;
            bool isCrit = false;

            switch (attackerType)
            {
                case AttackerType.NoSource:

                    if (causeHealthDamage)
                        isSuccessful = damageable.TakeDamage(noSourceDamage, noSourcePenetratingPower, ignoreDamageableIndex);

                    if (directlyAssertBuff && CalculateProbability(directBuffProbability))
                    {
                        if (damageable is Enemy)
                            (damageable as Enemy).attackerTransform = isBullet ? transform : player.transform;
                        damageable.GetBuff(buffType, directBuffDuration);
                    }
                    else if (causeBuffDamage)
                        damageable.TakeBuffDamage(buffType, noSourceBuffDamage, ignoreDamageableIndex);

                    if (isSuccessful)
                        successEvent?.Invoke(damageable);

                    break;

                case AttackerType.PlayerBlade:
                    
                    if (causeHealthDamage)
                    {
                        isCrit = CalculateProbability(player.playerData.FinalCritRate);
                        isSuccessful = damageable.TakeDamage(player.playerData.FinalDamage * player.motionValue[motionValueIndex] * (isCrit ? player.playerData.FinalCritDamage : 1), player.playerData.FinalPenetratingPower, player.attackPower[attackPowerIndex], isBullet ? transform : player.transform, ignoreDamageableIndex);
                    }

                    if (player.playerData.currentWeaponStaticData != null)
                        if (player.playerData.currentWeaponStaticData.buffDdamageType != BuffType.None)
                            damageable.TakeBuffDamage(player.playerData.currentWeaponStaticData.buffDdamageType, player.playerData.FinalBuffDamage * player.buffMotionValue[buffMotionValueIndex], ignoreDamageableIndex);

                    if (isSuccessful)
                    {
 
                        successEvent?.Invoke(damageable);

                        if (player.passiveSkillTriggerAction.ContainsKey(PlayerPassiveSkill.TriggerType.Hit))
                            player.passiveSkillTriggerAction[PlayerPassiveSkill.TriggerType.Hit]?.Invoke(damageable);
                    }

                    if (isCrit)
                    {
                        if (player.passiveSkillTriggerAction.ContainsKey(PlayerPassiveSkill.TriggerType.Crit))
                            player.passiveSkillTriggerAction[PlayerPassiveSkill.TriggerType.Crit]?.Invoke(damageable);
                    }

                    break;

                case AttackerType.PlayerOther:

                    if (causeHealthDamage)
                    {
                        isCrit = CalculateProbability(player.playerData.FinalCritRate);
                        isSuccessful = damageable.TakeDamage(player.playerData.FinalDamage * player.motionValue[motionValueIndex] * (isCrit ? player.playerData.FinalCritDamage : 1), player.playerData.FinalPenetratingPower, player.attackPower[attackPowerIndex], isBullet ? transform : player.transform, ignoreDamageableIndex);
                    }

                    if (directlyAssertBuff && CalculateProbability(directBuffProbability))
                    {
                        if (damageable is Enemy)
                            (damageable as Enemy).attackerTransform = isBullet ? transform : player.transform;
                        damageable.GetBuff(buffType, directBuffDuration);
                    }
                    else if (causeBuffDamage)
                        damageable.TakeBuffDamage(buffType, player.playerData.FinalDamage * player.buffMotionValue[buffMotionValueIndex], ignoreDamageableIndex);

                    if (isSuccessful)
                    {
                        successEvent?.Invoke(damageable);

                        if (player.passiveSkillTriggerAction.ContainsKey(PlayerPassiveSkill.TriggerType.Hit))
                            player.passiveSkillTriggerAction[PlayerPassiveSkill.TriggerType.Hit]?.Invoke(damageable);
                    }

                    if (isCrit)
                    {
                        if (player.passiveSkillTriggerAction.ContainsKey(PlayerPassiveSkill.TriggerType.Crit))
                            player.passiveSkillTriggerAction[PlayerPassiveSkill.TriggerType.Crit]?.Invoke(damageable);
                    }

                    break;

                case AttackerType.Enemy:

                    if (causeHealthDamage)
                        isSuccessful = damageable.TakeDamage(enemy.FinalDamage * enemy.motionValue[motionValueIndex], enemy.FinalPenetratingPower, enemy.attackPower[attackPowerIndex], isBullet ? transform : enemy.transform, ignoreDamageableIndex);

                    if (directlyAssertBuff && CalculateProbability(directBuffProbability))
                        damageable.GetBuff(buffType, directBuffDuration);
                    else if (causeBuffDamage)
                        damageable.TakeBuffDamage(buffType, enemy.FinalDamage * enemy.buffMotionValue[buffMotionValueIndex], ignoreDamageableIndex);

                    if (isSuccessful)
                        successEvent?.Invoke(damageable);

                    break;

                case AttackerType.Trap:

                    //TODO: 陷阱攻击

                    break;

                default:
                    break;
            }
        }
    }

    private bool CalculateProbability(float probability) => probability >= Random.Range(0f, 1f);
}
