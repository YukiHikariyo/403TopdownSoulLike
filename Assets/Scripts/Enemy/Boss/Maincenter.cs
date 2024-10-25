using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Maincenter : Enemy
{
    [Space(16)]
    [Header("Maincenter")]
    [Space(16)]

    public GameObject attackObj1_1;
    public GameObject attackObj1_2;
    public GameObject attackObj1_3;
    public GameObject attackObj2;

    [Space(16)]

    public GameObject darkBulletPrefab;
    public GameObject fastDarkBulletPrefab;
    public Transform attackCenter;

    [Space(16)]

    public float bigStunValue;

    public EnemyState entryState;
    public EnemyState chaseState;
    public EnemyState backJumpState;
    public EnemyState attack1_1State;
    public EnemyState attack1_2State;
    public EnemyState attack1_3State;
    public EnemyState attack2State;
    public EnemyState attack3State;

    [HideInInspector] public Vector3 landPosition;

    protected override void Awake()
    {
        base.Awake();

        startState = entryState;
        defaultState = chaseState;
    }

    public override bool TakeDamage(float damage, float penetratingPower, float attackPower, Transform attackerTransform, bool ignoreDamageableIndex = false)
    {
        bool returnValue = base.TakeDamage(damage, penetratingPower, attackPower, attackerTransform, ignoreDamageableIndex);

        if (totalDamage >= bigStunValue)
        {
            totalDamage = 0;
            OnStunFunc(3).Forget();
            if (currentState != bigStunState)
                ChangeState(bigStunState);
        }

        return returnValue;
    }

    public void RotateAttack1_1Sprite() => attackObj1_1.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle(transform));
    public void RotateAttack1_2Sprite() => attackObj1_2.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle(transform));
    public void RotateAttack1_3Sprite() => attackObj1_3.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle(transform));

    public void CalculateLandPosition() => landPosition = target.transform.position;

    public void Attack3_1()
    {
        for (int i = -1; i <= 1; i++)
        {
            GameObject darkBullet = Instantiate(darkBulletPrefab, attackCenter.position, Quaternion.identity);
            darkBullet.GetComponent<EnemyBullet>().Initialize(this, 5, 5, CalculateTargetAngle(attackCenter) + i * 30, 0, 10, 4, true, true, false, 0, 0, BuffType.DarkErosion);
        }
    }

    public void Attack3_2()
    {
        for (int i = -3; i <= 3; i += 2)
        {
            GameObject darkBullet = Instantiate(darkBulletPrefab, attackCenter.position, Quaternion.identity);
            darkBullet.GetComponent<EnemyBullet>().Initialize(this, 5, 5, CalculateTargetAngle(attackCenter) + i * 15, -i * 20, 10, 4, true, true, false, 0, 0, BuffType.DarkErosion);
        }
    }

    public void Attack3_3()
    {
        GameObject fastDarkBullet = Instantiate(fastDarkBulletPrefab, attackCenter.position, Quaternion.identity);
        fastDarkBullet.GetComponent<EnemyBullet>().Initialize(this, 0, 50, CalculateTargetAngle(attackCenter), 0, 10, 5, true, true, false, 0, 0, BuffType.DarkErosion);
    }
}

public class MaincenterEntryState : EnemyState
{
    Maincenter maincenter;

    public MaincenterEntryState(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {
        enemy.damageableIndex = 1;
        enemy.anim.Play("Entry");
    }

    public override void LogicUpdate()
    {
        if (enemy.isAnimExit)
            enemy.ChangeState(maincenter.chaseState);
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        enemy.damageableIndex = 0;
    }
}

public class MaincenterChaseState : EnemyState
{
    Maincenter maincenter;

    float waitTimer;
    float stayTimer;

    public MaincenterChaseState(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("Chase");
        waitTimer = 0.5f;
        stayTimer = 0;
        enemy.OnSeekPath().Forget();
    }

    public override void LogicUpdate()
    {
        stayTimer += Time.deltaTime;
        if (enemy.CalculateProbability(stayTimer * 0.01f))
        {
            if (enemy.CalculateProbability(0.65f))
                enemy.ChangeState(maincenter.attack3State);
            else
                enemy.ChangeState(maincenter.attack2State);
        }

        if (waitTimer > 0)
            waitTimer -= Time.deltaTime;
        else
        {
            if (enemy.PlayerCheck(0, false))
                enemy.ChangeState(maincenter.attack1_1State);
        }
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

public class MaincenterBackJumpState : EnemyState
{
    Maincenter maincenter;

    public MaincenterBackJumpState(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("BackJump");
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

public class MaincenterBigStunState : EnemyState
{
    Maincenter maincenter;

    public MaincenterBigStunState(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {
        enemy.rb.velocity = Vector2.zero;
        enemy.rb.AddForce((enemy.transform.position - enemy.attackerTransform.position).normalized * 20, ForceMode2D.Impulse);
        enemy.anim.Play("BigStun");
        StateChangeTimer(5, enemy.defaultState).Forget();
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

public class MaincenterAttack1_1State : EnemyState
{
    Maincenter maincenter;

    public MaincenterAttack1_1State(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {

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

public class MaincenterAttack1_2State : EnemyState
{
    Maincenter maincenter;

    public MaincenterAttack1_2State(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {

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

public class MaincenterAttack1_3State : EnemyState
{
    Maincenter maincenter;

    public MaincenterAttack1_3State(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {

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

public class MaincenterAttack2State : EnemyState
{
    Maincenter maincenter;

    public MaincenterAttack2State(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {

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

public class MaincenterAttack3State : EnemyState
{
    Maincenter maincenter;

    public MaincenterAttack3State(Enemy enemy, Maincenter maincenter) : base(enemy)
    {
        this.maincenter = maincenter;
    }

    public override void OnEnter()
    {

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