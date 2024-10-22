using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GearSpider : Enemy
{
    [Space(16)]
    [Header("GearSpider")]
    [Space(16)]

    public GameObject attackObj;

    public EnemyState idleState;
    public EnemyState moveState;
    public EnemyState attackState;

    protected override void Awake()
    {
        base.Awake();

        idleState = new GearSpiderIdleState(this, this);
        moveState = new GearSpiderMoveState(this, this);
        attackState = new GearSpiderAttackState(this, this);

        startState = idleState;
        defaultState = moveState;
    }

    public void RotateAttackSprite() => attackObj.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle());
}

public class GearSpiderIdleState : EnemyState
{
    GearSpider gearSpider;

    public GearSpiderIdleState(Enemy enemy, GearSpider gearSpider) : base(enemy)
    {
        this.gearSpider = gearSpider;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Idle");
        enemy.rb.velocity = Vector2.zero;
        StateChangeTimer(1, gearSpider.moveState).Forget();
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

public class GearSpiderMoveState : EnemyState
{
    GearSpider gearSpider;

    Vector2 dir;

    public GearSpiderMoveState(Enemy enemy, GearSpider gearSpider) : base(enemy)
    {
        this.gearSpider = gearSpider;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Move");
        StateChangeTimer(1, gearSpider.idleState).Forget();
        dir = enemy.PlayerCheck(0, false) ? enemy.CalculateTargetDirection() : Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * Vector2.right;
    }

    public override void LogicUpdate()
    {
        if (enemy.PlayerCheck(1, false))
            enemy.ChangeState(gearSpider.attackState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(dir);
    }

    public override void OnExit()
    {
        changeTimerCTK.Cancel();
    }
}

public class GearSpiderAttackState : EnemyState
{
    GearSpider gearSpider;

    public GearSpiderAttackState(Enemy enemy, GearSpider gearSpider) : base(enemy)
    {
        this.gearSpider = gearSpider;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.moveSpeedIncrement += 3;
        enemy.anim.Play("Attack");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(gearSpider.moveState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.CalculateTargetDirection(), true);
    }

    public override void OnExit()
    {
        gearSpider.attackObj.SetActive(false);
        enemy.isMove = false;
        enemy.moveSpeedIncrement -= 3;
    }
}