using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel_II : Enemy
{
    [Space(16)]
    [Header("Sentinel_II")]
    [Space(16)]

    public GameObject darkBulletPrefab;
    public GameObject fastDarkBulletPrefab;
    public Transform attackCenter;

    public EnemyState idleState;
    public EnemyState attack1State;
    public EnemyState attack2State;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Sentinel_II_IdleState(this, this);
        attack1State = new Sentinel_II_Attack1State(this, this);
        attack2State = new Sentinel_II_Attack2State(this, this);

        startState = idleState;
        defaultState = idleState;
    }

    public void Attack1()
    {
        GameObject darkBullet = Instantiate(darkBulletPrefab, attackCenter.position, Quaternion.identity);
        darkBullet.GetComponent<EnemyBullet>().Initialize(this, 5, 5, CalculateTargetAngle(attackCenter), 0, 10, 0, true, true, false, 0, 0, BuffType.DarkErosion);
    }

    public void Attack2()
    {
        GameObject fastDarkBullet = Instantiate(fastDarkBulletPrefab, attackCenter.position, Quaternion.identity);
        fastDarkBullet.GetComponent<EnemyBullet>().Initialize(this, 0, 50, CalculateTargetAngle(attackCenter), 0, 10, 1, true, true, false, 0, 0, BuffType.DarkErosion);
    }
}

public class Sentinel_II_IdleState : EnemyState
{
    Sentinel_II sentinel_II;

    public Sentinel_II_IdleState(Enemy enemy, Sentinel_II sentinel_II) : base(enemy)
    {
        this.sentinel_II = sentinel_II;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Idle");
    }

    public override void LogicUpdate()
    {
        if (enemy.PlayerCheck(0, false))
        {
            if (enemy.CalculateProbability(0.5f))
                enemy.ChangeState(sentinel_II.attack1State);
            else
                enemy.ChangeState(sentinel_II.attack2State);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

public class Sentinel_II_Attack1State : EnemyState
{
    Sentinel_II sentinel_II;

    public Sentinel_II_Attack1State(Enemy enemy, Sentinel_II sentinel_II) : base(enemy)
    {
        this.sentinel_II = sentinel_II;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Attack1");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(sentinel_II.idleState);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

public class Sentinel_II_Attack2State : EnemyState
{
    Sentinel_II sentinel_II;

    public Sentinel_II_Attack2State(Enemy enemy, Sentinel_II sentinel_II) : base(enemy)
    {
        this.sentinel_II = sentinel_II;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Attack2");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(sentinel_II.idleState);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}