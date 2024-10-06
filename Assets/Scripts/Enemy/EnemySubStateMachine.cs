using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人子状态机基类
/// </summary>
public class EnemySubStateMachine
{
    protected Enemy enemy;
    protected EnemyState currentState;
    protected EnemyState defaultState;

    public EnemySubStateMachine(Enemy enemy)
    {
        this.enemy = enemy;

        //子类记得在此处实例化状态
        //子类记得在此处设置默认状态
    }

    public virtual void OnEnter()
    {
        currentState = defaultState;
        currentState.OnEnter();
    }
    public virtual void OnExit() => currentState.OnExit();
    public virtual void PhysicsUpdate() => currentState.PhysicsUpdate();
    public virtual void LogicUpdate() => currentState.LogicUpdate();

    public void ChangeState(EnemyState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
