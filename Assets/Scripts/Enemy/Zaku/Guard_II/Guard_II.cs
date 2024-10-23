using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Guard_II : Enemy
{
    [Space(16)]
    [Header("Guard_II")]
    [Space(16)]

    public GameObject attackObj1;
    public GameObject attackObj2;
    public GameObject chargeAttackObj;

    public EnemyState patrolState;
    public EnemyState chaseState;
    public EnemyState attack1State;
    public EnemyState attack2State;
    public EnemyState chargeState;
    public EnemyState chargeAttackState;

    protected override void Awake()
    {
        base.Awake();

        patrolState = new Guard_II_PatrolState(this, this);
        chaseState = new Guard_II_ChaseState(this, this);
        attack1State = new Guard_II_Attack1State(this, this);
        attack2State = new Guard_II_Attack2State(this, this);
        chargeState = new Guard_II_ChargeState(this, this);
        chargeAttackState = new Guard_II_ChargeAttackState(this, this);

        startState = patrolState;
        defaultState = patrolState;
    }

    public void RotateAttack1Sprite() => attackObj1.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle());
    public void RotateAttack2Sprite() => attackObj2.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle());
    public void RotateChargeAttackSprite() => chargeAttackObj.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle());
}

public class Guard_II_PatrolState : EnemyState
{
    Guard_II guard_II;

    Vector2 dir;
    CancellationTokenSource patrolCTK;

    public Guard_II_PatrolState(Enemy enemy, Guard_II guard_II) : base(enemy)
    {
        this.guard_II = guard_II;
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
            enemy.ChangeState(guard_II.chaseState);
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

public class Guard_II_ChaseState : EnemyState
{
    Guard_II guard_II;

    float waitTimer;

    public Guard_II_ChaseState(Enemy enemy, Guard_II guard_II) : base(enemy)
    {
        this.guard_II = guard_II;
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
            {
                if (enemy.CalculateProbability(0.75f))
                    enemy.ChangeState(guard_II.attack1State);
                else
                    enemy.ChangeState(guard_II.chargeState);
            }
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

public class Guard_II_Attack1State : EnemyState
{
    Guard_II guard_II;

    public Guard_II_Attack1State(Enemy enemy, Guard_II guard_II) : base(enemy)
    {
        this.guard_II = guard_II;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.moveSpeedIncrement += 3;
        enemy.anim.Play("Attack1");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
        {
            if (enemy.PlayerCheck(1, false))
                enemy.ChangeState(guard_II.attack2State);
            else
                enemy.ChangeState(guard_II.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.CalculateTargetDirection(), true);
    }

    public override void OnExit()
    {
        guard_II.attackObj1.SetActive(false);
        enemy.isMove = false;
        enemy.moveSpeedIncrement -= 3;
    }
}

public class Guard_II_Attack2State : EnemyState
{
    Guard_II guard_II;

    public Guard_II_Attack2State(Enemy enemy, Guard_II guard_II) : base(enemy)
    {
        this.guard_II = guard_II;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.moveSpeedIncrement += 3;
        enemy.anim.Play("Attack2");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
        {
            if (enemy.PlayerCheck(2, false))
            {
                if (enemy.CalculateProbability(0.75f))
                    enemy.ChangeState(guard_II.chargeState);
                else
                    enemy.ChangeState(guard_II.chaseState);
            }
            else
                enemy.ChangeState(guard_II.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.CalculateTargetDirection(), true);
    }

    public override void OnExit()
    {
        guard_II.attackObj2.SetActive(false);
        enemy.isMove = false;
        enemy.moveSpeedIncrement -= 3;
    }
}

public class Guard_II_ChargeState : EnemyState
{
    Guard_II guard_II;

    public Guard_II_ChargeState(Enemy enemy, Guard_II guard_II) : base(enemy)
    {
        this.guard_II = guard_II;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.anim.Play("Charge");
        StateChangeTimer(2, guard_II.chargeAttackState).Forget();
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

public class Guard_II_ChargeAttackState : EnemyState
{
    Guard_II guard_II;

    public Guard_II_ChargeAttackState(Enemy enemy, Guard_II guard_II) : base(enemy)
    {
        this.guard_II = guard_II;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.moveSpeedIncrement += 14;
        enemy.anim.Play("ChargeAttack");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(guard_II.chaseState);
    }

    public override void PhysicsUpdate()
    {
        enemy.Move(enemy.CalculateTargetDirection(), true);
    }

    public override void OnExit()
    {
        guard_II.chargeAttackObj.SetActive(false);
        enemy.isMove = false;
        enemy.moveSpeedIncrement -= 14;
    }
}