using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, float attackPower);
    void TakeBuffDamage(float damage);
    void GetBuff(float duration);
}
