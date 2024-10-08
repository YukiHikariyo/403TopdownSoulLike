using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    TestBuff,
}

public abstract class BaseBuff
{
    protected float duration;
    protected IDamageable damageable;

    public BaseBuff(float duration, IDamageable damageable)
    {
        this.duration = duration;
        this.damageable = damageable;
    }

    public abstract void OnBuffEnter();
    public abstract void OnBuffExit();
    public abstract void OnPlayerBuffStay();
    public abstract void OnEnemyBuffStay();
}

public class TestBuff : BaseBuff
{
    public TestBuff(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
            (damageable as Player).playerData.MaxHealthIncrement -= 50;
        else if (damageable is Enemy)
            (damageable as Enemy).CurrentHealth -= 50;
    }

    public override void OnBuffExit()
    {
        if (damageable is Player)
            (damageable as Player).playerData.MaxHealthIncrement += 50;
    }

    public override void OnPlayerBuffStay()
    {
        damageable.TakeDamage(10 * Time.deltaTime, 1000);
    }

    public override void OnEnemyBuffStay()
    {
        damageable.TakeDamage(20 * Time.deltaTime, 1000);
    }
}
