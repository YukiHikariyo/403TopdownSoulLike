using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 史莱姆
/// </summary>
public class Slime : Enemy
{
    public EnemyState idleState;
    public EnemyState jumpState;

    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, this);
        jumpState = new SlimeJumpState(this, this);

        startState = idleState;
        defaultState = idleState;
    }
}

public class SlimeIdleState : EnemyState
{
    Slime slime;

    public SlimeIdleState(Enemy enemy, Slime slime) : base(enemy)
    {
        this.slime = slime;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Idle");
        enemy.rb.velocity = Vector2.zero;
        StateChangeTimer(1, slime.jumpState).Forget();
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

public class SlimeJumpState : EnemyState
{
    Slime slime;

    Vector2 dir;
    CancellationTokenSource jumpCTK;

    public SlimeJumpState(Enemy enemy, Slime slime) : base(enemy)
    {
        this.slime = slime;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Jump");
        OnJump().Forget();
        StateChangeTimer(1, slime.idleState).Forget();
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        enemy.Move(dir, true);
    }

    public override void OnExit()
    {
        jumpCTK.Cancel();
        changeTimerCTK.Cancel();
    }

    private async UniTask OnJump()
    {
        jumpCTK = new();
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f), cancellationToken: jumpCTK.Token);
        dir = enemy.PlayerCheck(0, false) ? enemy.CalculateTargetDirection() : Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * Vector2.right;
        enemy.isMove = true;
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f), cancellationToken: jumpCTK.Token);
        enemy.isMove = false;
    }
}