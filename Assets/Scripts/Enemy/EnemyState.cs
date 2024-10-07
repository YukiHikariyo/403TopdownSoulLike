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
    public EnemyState lastState;

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

public class VoidState : EnemyState
{
    public VoidState(Enemy enemy, EnemySubStateMachine subStateMachine) : base(enemy, subStateMachine)
    {
    }

    public override void LogicUpdate()
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void PhysicsUpdate()
    {
        
    }
}
