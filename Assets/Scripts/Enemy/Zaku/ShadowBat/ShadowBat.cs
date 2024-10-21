using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBat : Enemy
{
    [Space(16)]
    [Header("暗影蝙蝠")]
    [Space(16)]

    public GameObject attackObj;
    public GameObject shadowAttackObj;

    public EnemyState idleState;
    public EnemyState chaseState;
    public EnemyState attackState;
    public EnemyState shadowAttackState;
    public EnemyState shadowSneakState;

    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadowBatIdleState(this, this);
        chaseState = new ShadowBatChaseState(this, this);
        attackState = new ShadowBatAttackState(this, this);
        shadowAttackState = new ShadowBatShadowAttackState(this, this);
        shadowSneakState = new ShadowBatShadowSneakState(this, this);

        startState = idleState;
        defaultState = chaseState;
    }

    public void RotateAttackSprite() => attackObj.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle());
    public void RotateShadowAttackSprite() => shadowAttackObj.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle());

    public void ShadowSneak()
    {
        Vector2 position;
        int count = 0;

        do
        {
            position = target.transform.position + Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * 5;
            count++;
            if (count > 10)
                return;

        } while (ObstacleCheck(position, 0.5f));

        transform.position = position;
    }
}

public class ShadowBatIdleState : EnemyState
{
    ShadowBat shadowBat;

    public ShadowBatIdleState(Enemy enemy, ShadowBat shadowBat) : base(enemy)
    {
        this.shadowBat = shadowBat;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Idle");
        enemy.rb.velocity = Vector2.zero;
    }

    public override void LogicUpdate()
    {
        if (enemy.PlayerCheck(1, false))
            enemy.ChangeState(shadowBat.shadowAttackState);

        //TODO: 光照后眩晕
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

public class ShadowBatChaseState : EnemyState
{
    ShadowBat shadowBat;

    public ShadowBatChaseState(Enemy enemy, ShadowBat shadowBat) : base(enemy)
    {
        this.shadowBat = shadowBat;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Chase");
        enemy.OnSeekPath().Forget();
    }

    public override void LogicUpdate()
    {
        if (enemy.PlayerCheck(0, false))
            enemy.ChangeState(shadowBat.attackState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.pathDirection);
    }

    public override void OnExit()
    {
        enemy.pathCTK.Cancel();
    }
}

public class ShadowBatAttackState : EnemyState
{
    ShadowBat shadowBat;

    public ShadowBatAttackState(Enemy enemy, ShadowBat shadowBat) : base(enemy)
    {
        this.shadowBat = shadowBat;
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
            enemy.ChangeState(shadowBat.chaseState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.CalculateTargetDirection(), true);
    }

    public override void OnExit()
    {
        shadowBat.attackObj.SetActive(false);
        enemy.isMove = false;
        enemy.moveSpeedIncrement -= 3;
    }
}

public class ShadowBatShadowAttackState : EnemyState
{
    ShadowBat shadowBat;

    public ShadowBatShadowAttackState(Enemy enemy, ShadowBat shadowBat) : base(enemy)
    {
        this.shadowBat = shadowBat;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.moveSpeedIncrement += 5;
        shadowBat.shadowAttackObj.transform.rotation = Quaternion.Euler(0, 0, enemy.CalculateTargetAngle());
        enemy.anim.Play("ShadowAttack");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(shadowBat.shadowSneakState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.CalculateTargetDirection(), true);
    }

    public override void OnExit()
    {
        shadowBat.shadowAttackObj.SetActive(false);
        enemy.isMove = false;
        enemy.moveSpeedIncrement -= 5;
    }
}

public class ShadowBatShadowSneakState : EnemyState
{
    ShadowBat shadowBat;

    public ShadowBatShadowSneakState(Enemy enemy, ShadowBat shadowBat) : base(enemy)
    {
        this.shadowBat = shadowBat;
    }

    public override void OnEnter()
    {
        enemy.damageableIndex = 1;
        enemy.rb.velocity = Vector2.zero;
        enemy.anim.Play("ShadowSneak");
        StateChangeTimer(2, shadowBat.chaseState).Forget();
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        enemy.damageableIndex = 0;
        changeTimerCTK.Cancel();
    }
}