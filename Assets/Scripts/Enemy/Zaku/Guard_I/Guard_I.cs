using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class Guard_I : Enemy
{
    [Space(16)]
    [Header("Guard_I")]
    [Space(16)]

    public GameObject attackObj1;
    public GameObject attackObj2;

    public EnemyState patrolState;
    public EnemyState chaseState;
    public EnemyState attack1State;
    public EnemyState attack2State;

    protected override void Awake()
    {
        base.Awake();

        patrolState = new Guard_I_PatrolState(this, this);
        chaseState = new Guard_I_ChaseState(this, this);
        attack1State = new Guard_I_Attack1State(this, this);
        attack2State = new Guard_I_Attack2State(this, this);

        startState = patrolState;
        defaultState = patrolState;
    }

    public void RotateAttack1Sprite() => attackObj1.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle(transform));
    public void RotateAttack2Sprite() => attackObj2.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle(transform));
}

public class Guard_I_PatrolState : EnemyState
{
    Guard_I guard_I;

    Vector2 dir;
    CancellationTokenSource patrolCTK;

    public Guard_I_PatrolState(Enemy enemy, Guard_I guard_I) : base(enemy)
    {
        this.guard_I = guard_I;
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
            enemy.ChangeState(guard_I.chaseState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(dir);
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
            await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: patrolCTK.Token);
        }
    }
}

public class Guard_I_ChaseState : EnemyState
{
    Guard_I guard_I;

    float waitTimer;

    public Guard_I_ChaseState(Enemy enemy, Guard_I guard_I) : base(enemy)
    {
        this.guard_I = guard_I;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Chase");
        enemy.moveSpeedIncrement += 0.5f;
        waitTimer = 0.5f;
        enemy.OnSeekPath().Forget();
    }

    public override void LogicUpdate()
    {
        if (waitTimer > 0)
            waitTimer -= Time.deltaTime;
        else
        {
            if (enemy.PlayerCheck(1, false))
                enemy.ChangeState(guard_I.attack1State);
        }
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.pathDirection);
    }

    public override void OnExit()
    {
        enemy.moveSpeedIncrement -= 0.5f;
        enemy.pathCTK.Cancel();
    }
}

public class Guard_I_Attack1State : EnemyState
{
    Guard_I guard_I;

    public Guard_I_Attack1State(Enemy enemy, Guard_I guard_I) : base(enemy)
    {
        this.guard_I = guard_I;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.MotionToughness += 5;
        enemy.moveSpeedIncrement += 3;
        enemy.anim.Play("Attack1");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
        {
            if (enemy.PlayerCheck(1, false))
                enemy.ChangeState(guard_I.attack2State);
            else
                enemy.ChangeState(guard_I.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.CalculateTargetDirection(enemy.transform), true);
    }

    public override void OnExit()
    {
        guard_I.attackObj1.SetActive(false);
        enemy.isMove = false;
        enemy.moveSpeedIncrement -= 3;
        enemy.MotionToughness -= 5;
    }
}

public class Guard_I_Attack2State : EnemyState
{
    Guard_I guard_I;

    public Guard_I_Attack2State(Enemy enemy, Guard_I guard_I) : base(enemy)
    {
        this.guard_I = guard_I;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.MotionToughness += 5;
        enemy.moveSpeedIncrement += 3;
        enemy.anim.Play("Attack2");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(guard_I.chaseState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.CalculateTargetDirection(enemy.transform), true);
    }

    public override void OnExit()
    {
        guard_I.attackObj2.SetActive(false);
        enemy.isMove = false;
        enemy.moveSpeedIncrement -= 3;
        enemy.MotionToughness -= 5;
    }
}