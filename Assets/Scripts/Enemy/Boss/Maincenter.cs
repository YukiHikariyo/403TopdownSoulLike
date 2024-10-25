using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maincenter : Enemy
{
    [Space(16)]
    [Header("Maincenter")]
    [Space(16)]

    public GameObject attackObj1_1;
    public GameObject attackObj1_2;
    public GameObject attackObj1_3;
    public GameObject attackObj2;

    public GameObject darkBulletPrefab;
    public GameObject fastDarkBulletPrefab;

    public EnemyState entryState;
    public EnemyState chaseState;
    public EnemyState backJumpState;
    public EnemyState attack1_1State;
    public EnemyState attack1_2State;
    public EnemyState attack1_3State;
    public EnemyState attack2State;
    public EnemyState attack3State;

    protected override void Awake()
    {
        base.Awake();

        startState = entryState;
        defaultState = chaseState;
    }

    public void RotateAttack1_1Sprite() => attackObj1_1.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle(transform));
    public void RotateAttack1_2Sprite() => attackObj1_2.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle(transform));
    public void RotateAttack1_3Sprite() => attackObj1_3.transform.rotation = Quaternion.Euler(0, 0, CalculateTargetAngle(transform));

    public void Attack3_1()
    {

    }

    public void Attack3_2()
    {

    }

    public void Attack3_3()
    {

    }
}
