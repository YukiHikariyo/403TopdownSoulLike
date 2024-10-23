using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel_I : Enemy
{
    [Space(16)]
    [Header("Sentinel_I")]
    [Space(16)]

    public GameObject darkBulletPrefab;
    public Transform attackCenter;

    public EnemyState idleState;
    public EnemyState attackState;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Sentinel_I_IdleState(this, this);
        attackState = new Sentinel_I_AttackState(this, this);

        startState = idleState;
        defaultState = idleState;
    }

    public void Attack()
    {
        GameObject darkBullet = Instantiate(darkBulletPrefab, attackCenter.position, Quaternion.identity);
        darkBullet.GetComponent<EnemyBullet>().Initialize(this, 5, 10, CalculateTargetAngle(attackCenter), 0, 10, 0, true, true, false, 0, 0, BuffType.DarkErosion);
    }
}

public class Sentinel_I_IdleState : EnemyState
{
    Sentinel_I sentinel_I;

    public Sentinel_I_IdleState(Enemy enemy, Sentinel_I sentinel_I) : base(enemy)
    {
        this.sentinel_I = sentinel_I;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Idle");
    }

    public override void LogicUpdate()
    {
        if (enemy.PlayerCheck(0, false))
            enemy.ChangeState(sentinel_I.attackState);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

public class Sentinel_I_AttackState : EnemyState
{
    Sentinel_I sentinel_I;

    public Sentinel_I_AttackState(Enemy enemy, Sentinel_I sentinel_I) : base(enemy)
    {
        this.sentinel_I = sentinel_I;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Attack");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(sentinel_I.idleState);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}