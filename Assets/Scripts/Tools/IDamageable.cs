using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可受伤类型接口
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// 受到有来源伤害
    /// </summary>
    /// <param name="damage">最终伤害</param>
    /// <param name="penetratingPower">最终穿透力</param>
    /// <param name="attackPower">攻击强度</param>
    /// <param name="attackerTransform">攻击者的Transform</param>
    /// <param name="ignoreDamageableIndex">是否无视可受伤状态</param>
    /// <returns>是否成功造成伤害</returns>
    bool TakeDamage(float damage, float penetratingPower, float attackPower, Transform attackerTransform, bool ignoreDamageableIndex = false);

    /// <summary>
    /// 受到无来源伤害
    /// </summary>
    /// <param name="damage">最终伤害</param>
    /// <param name="penetratingPower">最终穿透力</param>
    /// <param name="ignoreDamageableIndex">是否无视可受伤状态</param>
    /// <returns>是否成功造成伤害</returns>
    bool TakeDamage(float damage, float penetratingPower, bool ignoreDamageableIndex = false);

    /// <summary>
    /// 受到累积性Buff伤害
    /// </summary>
    /// <param name="buffType">Buff种类</param>
    /// <param name="damage">Buff累积条伤害</param>
    /// <param name="ignoreDamageableIndex">是否无视可受伤状态</param>
    /// <returns>是否成功造成伤害</returns>
    bool TakeBuffDamage(BuffType buffType, float damage, bool ignoreDamageableIndex = false);

    /// <summary>
    /// 获得Buff
    /// </summary>
    /// <param name="buffType">Buff种类</param>
    /// <param name="duration">持续时间</param>
    void GetBuff(BuffType buffType, float duration);

    /// <summary>
    /// 移除Buff
    /// </summary>
    /// <param name="buffType">Buff种类</param>
    void RemoveBuff(BuffType buffType);
}
