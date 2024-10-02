using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    TestBuff1,
    TestBuff2,
    TestBuff3,
}

public abstract class BaseBuff
{
    float duration;
    IDamageable damageable;

    public BaseBuff(float duration, IDamageable damageable)
    {
        this.duration = duration;
        this.damageable = damageable;
    }

    public abstract void OnBuffEnter();
    public abstract void OnBuffExit();
    public abstract void OnBuffStay();
}

public class TestBuff1 : BaseBuff
{
    public TestBuff1(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        
    }

    public override void OnBuffExit()
    {
        
    }

    public override void OnBuffStay()
    {
        
    }
}
