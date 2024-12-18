using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum BuffType
{
    None,
    TestBuff,
    //玩家增益
    Tough,
    Foresight,
    ForesightEndurance,
    Hot,
    //玩家异常
    Cold,
    Frozen,
    IceBurning,
    DarkErosion,
    //敌人增益
    Endurance,
    ShadowSneak,
    //敌人异常
    Dizzy,
    Burning,
    LightBurst,
}

/// <summary>
/// Buff的基类
/// </summary>
public abstract class BaseBuff
{
    protected float duration;
    protected IDamageable damageable;

    public BaseBuff(IDamageable damageable)
    {
        this.damageable = damageable;
    }

    public BaseBuff(float duration, IDamageable damageable) : this(damageable)
    {
        this.duration = duration;
    }

    public abstract void OnBuffEnter();
    public abstract void OnBuffExit();
    public abstract void OnBuffStay();
}

/// <summary>
/// 测试Buff
/// </summary>
public class TestBuff : BaseBuff
{
    float timer = 0;

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

    public override void OnBuffStay()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            damageable.TakeDamage(10, 1000);
            timer = 1;
        }
    }
}

#region 玩家增益

/// <summary>
/// 玩家增益：坚韧
/// </summary>
public class Tough : BaseBuff
{
    public Tough(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
            (damageable as Player).playerData.toughnessMultiplication += 0.25f;

        UIManager.Instance.buffIcons[0].SetActive(true);
    }

    public override void OnBuffExit()
    {
        if (damageable is Player)
            (damageable as Player).playerData.toughnessMultiplication -= 0.25f;

        UIManager.Instance.buffIcons[0].SetActive(false);
    }

    public override void OnBuffStay()
    {
        
    }
}

/// <summary>
/// 玩家增益：识破
/// </summary>
public class Foresight : BaseBuff
{
    public Foresight(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
        {
            (damageable as Player).playerData.damageMultiplication += 0.2f;
            (damageable as Player).playerData.critRateIncrement += 0.1f;
            VFXManager.Instance.PlayVFX(1, (damageable as Player).transform, (damageable as Player).transform.position, 0);
        }

        UIManager.Instance.buffIcons[1].SetActive(true);
    }

    public override void OnBuffExit()
    {
        if (damageable is Player)
        {
            (damageable as Player).playerData.damageMultiplication -= 0.2f;
            (damageable as Player).playerData.critRateIncrement -= 0.1f;
        }

        UIManager.Instance.buffIcons[1].SetActive(false);
    }

    public override void OnBuffStay()
    {

    }
}

/// <summary>
/// 玩家增益：见切临时霸体
/// </summary>
public class ForesightEndurance : BaseBuff
{
    public ForesightEndurance(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
            (damageable as Player).isEnduance = true;
    }

    public override void OnBuffExit()
    {
        if (damageable is Player)
            (damageable as Player).isEnduance = false;
    }

    public override void OnBuffStay()
    {

    }
}

/// <summary>
/// 玩家增益：灼热
/// </summary>
public class Hot : BaseBuff
{
    public Hot(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
        {
            (damageable as Player).playerData.moveSpeedMultiplication += 0.25f;
            (damageable as Player).playerData.lightRadiusMultiplication += 0.5f;
        }

        UIManager.Instance.buffIcons[2].SetActive(true);
    }

    public override void OnBuffExit()
    {
        if (damageable is Player)
        {
            (damageable as Player).playerData.moveSpeedMultiplication -= 0.25f;
            (damageable as Player).playerData.lightRadiusMultiplication -= 0.5f;
        }

        UIManager.Instance.buffIcons[2].SetActive(false);
    }

    public override void OnBuffStay()
    {

    }
}

#endregion

#region 玩家异常

/// <summary>
/// 玩家异常：寒冷
/// </summary>
public class Cold : BaseBuff
{
    public Cold(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
        {
            (damageable as Player).playerData.moveSpeedMultiplication -= 0.5f;
            (damageable as Player).playerData.energyCostMultiplication += 0.5f;
        }
    }

    public override void OnBuffExit()
    {
        if (damageable is Player)
        {
            (damageable as Player).playerData.moveSpeedMultiplication += 0.5f;
            (damageable as Player).playerData.energyCostMultiplication -= 0.5f;
        }
    }

    public override void OnBuffStay()
    {

    }
}

/// <summary>
/// 玩家异常：冰冻
/// </summary>
public class Frozen : BaseBuff
{
    public Frozen(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
        {
            //TODO: 禁用玩家操作
            (damageable as Player).playerData.reductionRateIncrement += 0.2f;
            (damageable as Player).isEnduance = true;
        }
    }

    public override void OnBuffExit()
    {
        if (damageable is Player)
        {
            //TODO: 启用玩家操作
            (damageable as Player).playerData.reductionRateIncrement -= 0.2f;
            (damageable as Player).isEnduance = false;
        }
    }

    public override void OnBuffStay()
    {

    }
}

/// <summary>
/// 玩家异常：霜焱
/// </summary>
public class IceBurning : BaseBuff
{
    float timer = 0;

    public IceBurning(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        UIManager.Instance.buffIcons[3].SetActive(true);
    }

    public override void OnBuffExit()
    {
        UIManager.Instance.buffIcons[3].SetActive(false);
    }

    public override void OnBuffStay()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            if (damageable is Player)
                damageable.TakeDamage((damageable as Player).playerData.FinalMaxHealth * 0.02f, 114514, true);

            timer = 1;
        }
    }
}

/// <summary>
/// 玩家异常：暗蚀
/// </summary>
public class DarkErosion : BaseBuff
{
    public DarkErosion(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Player)
        {
            damageable.TakeDamage((damageable as Player).playerData.FinalMaxHealth * 0.4f, 114514, true);
            VFXManager.Instance.PlayVFX(2, (damageable as Player).transform, (damageable as Player).transform.position, 0);
        }
    }

    public override void OnBuffExit()
    {

    }

    public override void OnBuffStay()
    {

    }
}

#endregion

#region 敌人增益

/// <summary>
/// 敌人增益：霸体
/// </summary>
public class Endurance : BaseBuff
{
    public Endurance(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Enemy)
            (damageable as Enemy).isEnduance = true;
    }

    public override void OnBuffExit()
    {
        if (damageable is Enemy)
            (damageable as Enemy).isEnduance = false;
    }

    public override void OnBuffStay()
    {
        
    }
}

/// <summary>
/// 敌人增益：影袭
/// </summary>
public class ShadowSneak : BaseBuff
{
    public ShadowSneak(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Enemy)
        {
            (damageable as Enemy).damageMultiplication += 0.5f;
            (damageable as Enemy).moveSpeedMultiplication += 0.5f;
        }
    }

    public override void OnBuffExit()
    {
        if (damageable is Enemy)
        {
            (damageable as Enemy).damageMultiplication -= 0.5f;
            (damageable as Enemy).moveSpeedMultiplication -= 0.5f;
        }
    }

    public override void OnBuffStay()
    {

    }
}

#endregion

#region 敌人异常

/// <summary>
/// 敌人异常：眩晕
/// </summary>
public class Dizzy : BaseBuff
{
    public Dizzy(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Enemy)
            (damageable as Enemy).ChangeState((damageable as Enemy).dizzyStunState);
    }

    public override void OnBuffExit()
    {
        if (damageable is Enemy)
            (damageable as Enemy).ChangeState((damageable as Enemy).defaultState);
    }

    public override void OnBuffStay()
    {

    }
}

/// <summary>
/// 敌人异常：燃烧
/// </summary>
public class Burning : BaseBuff
{
    float timer = 0;

    public Burning(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        VFXManager.Instance.PlayVFX(3, (damageable as Enemy).transform, (damageable as Enemy).transform.position, duration);
    }

    public override void OnBuffExit()
    {

    }

    public override void OnBuffStay()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            if (damageable is Enemy)
            {
                if (!(damageable as Enemy).isBoss)
                    damageable.TakeDamage((damageable as Enemy).MaxHealth * 0.04f, 114514, true);
                else
                    damageable.TakeDamage((damageable as Enemy).MaxHealth * 0.005f, 114514, true);
            }

            timer = 1;
        }
    }
}

/// <summary>
/// 敌人异常：光爆
/// </summary>
public class LightBurst : BaseBuff
{
    public LightBurst(float duration, IDamageable damageable) : base(duration, damageable) { }

    public override void OnBuffEnter()
    {
        if (damageable is Enemy)
        {
            if (!(damageable as Enemy).isBoss)
                damageable.TakeDamage((damageable as Enemy).MaxHealth * 0.25f, 114514, true);
            else
                damageable.TakeDamage((damageable as Enemy).MaxHealth * 0.05f, 114514, true);

            VFXManager.Instance.PlayVFX(4, (damageable as Enemy).transform, (damageable as Enemy).transform.position, 0);
        }
    }

    public override void OnBuffExit()
    {

    }

    public override void OnBuffStay()
    {

    }
}

#endregion