using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人状态基类
/// </summary>
public abstract class EnemyState
{
    protected Enemy enemy;
    public EnemyState lastState;

    public EnemyState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void PhysicsUpdate();
    public abstract void LogicUpdate();
}