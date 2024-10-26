using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 敌人状态基类
/// </summary>
public abstract class EnemyState
{
    protected Enemy enemy;
    public EnemyState lastState;
    protected CancellationTokenSource changeTimerCTK;

    public EnemyState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void PhysicsUpdate();
    public abstract void LogicUpdate();

    /// <summary>
    /// 定时切换到下个状态
    /// </summary>
    /// <param name="changeTime">定时时间</param>
    /// <param name="nextState">下个状态</param>
    protected async UniTask StateChangeTimer(float changeTime, EnemyState nextState)
    {
        changeTimerCTK = new();
        await UniTask.Delay(TimeSpan.FromSeconds(changeTime), cancellationToken: changeTimerCTK.Token);
        enemy.ChangeState(nextState);
    }
}

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(Enemy enemy) : base(enemy)
    {
    }

    public override void OnEnter()
    {
        if (enemy.spawner != null)
            enemy.spawner.isDead = true;
        enemy.damageableIndex = 1;
        enemy.rb.velocity = Vector2.zero;
        enemy.rb.AddForce((enemy.transform.position - enemy.attackerTransform.position).normalized * 10, ForceMode2D.Impulse);
        enemy.anim.Play("Dead");
        enemy.collider.enabled = false;
    }

    public override void LogicUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}

public class EnemyDizzyState : EnemyState
{
    public EnemyDizzyState(Enemy enemy) : base(enemy)
    {
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.rb.AddForce((enemy.transform.position - enemy.attackerTransform.position).normalized * 10, ForceMode2D.Impulse);
        enemy.anim.Play("Dizzy");
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        
    }
}

public class EnemySmallStunState : EnemyState
{
    public EnemySmallStunState(Enemy enemy) : base(enemy)
    {
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.rb.AddForce((enemy.transform.position - enemy.attackerTransform.position).normalized * 5, ForceMode2D.Impulse);
        enemy.anim.Play("SmallStun");
        StateChangeTimer(0.5f, enemy.defaultState).Forget();
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        changeTimerCTK.Cancel();
    }
}

public class EnemyNormalStunState : EnemyState
{
    public EnemyNormalStunState(Enemy enemy) : base(enemy)
    {
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.rb.AddForce((enemy.transform.position - enemy.attackerTransform.position).normalized * 10, ForceMode2D.Impulse);
        enemy.anim.Play("NormalStun");
        StateChangeTimer(1.2f, enemy.defaultState).Forget();
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        changeTimerCTK.Cancel();
    }
}