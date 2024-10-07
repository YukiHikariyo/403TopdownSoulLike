using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;

/// <summary>
/// 敌人子状态机基类
/// </summary>
public class EnemySubStateMachine
{
    protected Enemy enemy;
    public EnemyState currentState;
    protected EnemyState defaultState;
    public EnemyState whenExitState;    //退出时的状态
    public EnemySubStateMachine lastSubSM;

    public EnemySubStateMachine(Enemy enemy)
    {
        this.enemy = enemy;

        //子类记得在此处实例化状态
        //子类记得在此处设置默认状态
    }

    public virtual void OnEnter(bool continueState)
    {
        currentState = (continueState && whenExitState != null) ? whenExitState : defaultState;
        currentState.OnEnter();
    }
    public virtual void OnExit() => currentState.OnExit();
    public virtual void PhysicsUpdate() => currentState.PhysicsUpdate();
    public virtual void LogicUpdate() => currentState.LogicUpdate();

    public void ChangeState(EnemyState newState)
    {
        EnemyState lastState = currentState;
        currentState.OnExit();
        currentState = newState;
        currentState.lastState = lastState;
        currentState.OnEnter();
    }
}

public class PublicSM : EnemySubStateMachine
{
    public EnemyState voidState;

    public PublicSM(Enemy enemy) : base(enemy)
    {
        voidState = new VoidState(enemy, this);
        defaultState = voidState;
    }
}
