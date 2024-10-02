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
    public abstract void OnBuffStay();
}

public class TestBuff : BaseBuff
{
    public TestBuff(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
            (damageable as Player).playerData.maxHealthIncrement += 100;
    }

    public override void OnBuffExit()
    {
        if (damageable is Player)
            (damageable as Player).playerData.maxHealthIncrement -= 100;
    }

    public override void OnBuffStay()
    {
        damageable.TakeDamage(1 * Time.deltaTime);
    }
}
