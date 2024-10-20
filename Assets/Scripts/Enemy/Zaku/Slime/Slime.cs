using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
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

    }
}

public class SlimeJumpState : EnemyState
{
    Slime slime;

    Vector2 dir;

    public SlimeJumpState(Enemy enemy, Slime slime) : base(enemy)
    {
        this.slime = slime;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Jump");
        dir = Vector2.right;
        StateChangeTimer(1, slime.idleState).Forget();
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        enemy.Move(dir);
    }

    public override void OnExit()
    {

    }
}