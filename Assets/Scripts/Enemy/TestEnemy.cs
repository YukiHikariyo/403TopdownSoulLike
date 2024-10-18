using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    private EnemyState testEnemyChaseState;

    protected override void Awake()
    {
        base.Awake();

        testEnemyChaseState = new TestEnemyChaseState(this, this);

        startState = testEnemyChaseState;
        defaultState = testEnemyChaseState;
    }
}

public class TestEnemyChaseState : EnemyState
{
    TestEnemy testEnemy;

    public TestEnemyChaseState(Enemy enemy, TestEnemy testEnemy) : base(enemy)
    {
        this.testEnemy = testEnemy;
    }

    public override void OnEnter()
    {
        enemy.OnSeekPath().Forget();
    }

    public override void LogicUpdate()
    {
        
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
