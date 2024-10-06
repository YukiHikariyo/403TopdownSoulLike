using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人状态基类
/// </summary>
public abstract class EnemyState
{
    protected Enemy enemy;
    protected EnemySubStateMachine subStateMachine;

    public EnemyState(Enemy enemy, EnemySubStateMachine subStateMachine)
    {
        this.enemy = enemy;
        this.subStateMachine = subStateMachine;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void PhysicsUpdate();
    public abstract void LogicUpdate();
}
