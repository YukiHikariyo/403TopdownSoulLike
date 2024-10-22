using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class Patroller_I : Enemy
{
    [Space(16)]
    [Header("Patroller_I")]
    [Space(16)]

    public GameObject attackObj;

    public EnemyState patrolState;
    public EnemyState waitState;
    public EnemyState attackState;

    protected override void Awake()
    {
        base.Awake();

        patrolState = new Patroller_I_PatrolState(this, this);
        waitState = new Patroller_I_WaitState(this, this);
        attackState = new Patroller_I_AttackState(this, this);

        startState = patrolState;
        defaultState = patrolState;
    }
}

public class Patroller_I_PatrolState : EnemyState
{
    Patroller_I patroller_I;

    Vector2 dir;
    CancellationTokenSource patrolCTK;

    public Patroller_I_PatrolState(Enemy enemy, Patroller_I patroller_I) : base(enemy)
    {
        this.patroller_I = patroller_I;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Patrol");
        enemy.rb.velocity = Vector2.zero;
        patrolCTK = new();
        OnPatrol().Forget();
    }

    public override void LogicUpdate()
    {
        if (enemy.PlayerCheck(0, false))
            patroller_I.ChangeState(patroller_I.waitState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(dir, true);
    }
    public override void OnExit()
    {
        patrolCTK.Cancel();
    }

    private async UniTask OnPatrol()
    {
        while (true)
        {
            dir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * Vector2.right;
            enemy.isMove = true;

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: patrolCTK.Token);

            enemy.isMove = true;

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: patrolCTK.Token);
        }
    }
}

public class Patroller_I_WaitState : EnemyState
{
    Patroller_I patroller_I;

    public Patroller_I_WaitState(Enemy enemy, Patroller_I patroller_I) : base(enemy)
    {
        this.patroller_I = patroller_I;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Wait");
        enemy.rb.velocity = Vector2.zero;
        StateChangeTimer(1.5f, patroller_I.attackState).Forget();
    }

    public override void LogicUpdate()
    {
        enemy.FaceToTarget();
    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        changeTimerCTK.Cancel();
    }
}

public class Patroller_I_AttackState : EnemyState
{
    Patroller_I patroller_I;

    Vector2 dir;

    public Patroller_I_AttackState(Enemy enemy, Patroller_I patroller_I) : base(enemy)
    {
        this.patroller_I = patroller_I;
    }

    public override void OnEnter()
    {
        dir = enemy.CalculateTargetDirection();
        enemy.anim.Play("Attack");
        enemy.moveSpeedIncrement += 3;
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(patroller_I.patrolState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(dir);
    }
    public override void OnExit()
    {
        patroller_I.attackObj.SetActive(false);
        enemy.moveSpeedIncrement -= 3;
    }
}