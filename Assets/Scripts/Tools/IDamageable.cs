using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, float attackPower, Transform attackerTransform);  //有来源伤害
    void TakeDamage(float damage);  //无来源伤害
    void TakeBuffDamage(BuffType buffType, float damage);
    void GetBuff(BuffType buffType, float duration);
    void RemoveBuff(BuffType buffType);
}
