using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPassiveSkill;

namespace PlayerPassiveSkill
{
    public enum TriggerType
    {
        [Tooltip("间隔固定时间触发")] Interval,
        [Tooltip("命中触发")] Hit,
        [Tooltip("暴击触发")] Crit,
        [Tooltip("击杀触发")] Kill,
        [Tooltip("受伤触发")] GetHit,   //仅限受到有来源伤害
        [Tooltip("闪避触发")] Dodge,    //仅限闪避有来源伤害
        [Tooltip("见切触发")] Foresight,    //仅限见切有来源伤害
        [Tooltip("改变属性")] ChangeValue,
    }

}

public enum PassiveSkillType
{
    None,
    //天赋树部分
    TestPassiveSkill,
    ForesightEnhance,
    LongerForesightEnhance,
    LongerForesightTime,
    FastCharge,
    FasterCharge,
    FastestCharge,
    //武器部分
    IronShellFurnance,
    FirePenalty,
    //饰品部分
    CombustionEngine,
    EnergyPump,
    AdvancedThruster,
    OverloadDevice,
    ShockAbsorber,
}

public abstract class BasePassiveSkill
{
    protected Player player;
    public TriggerType triggerType;
    public float interval;

    public BasePassiveSkill(Player player)
    {
        this.player = player;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnTrigger(IDamageable damageable); //传入的IDamageable只在击中和击杀触发时有用，其他只是占位用

    protected bool CalculateProbability(float probability) => probability >= Random.Range(0, 1);
}

public class TestPassiveSkill : BasePassiveSkill
{
    public TestPassiveSkill(Player player) : base(player)
    {
        triggerType = TriggerType.GetHit;
        interval = 1;
    }

    public override void OnEnter()
    {
        player.playerData.MaxHealthMultiplication += 2;
    }

    public override void OnExit()
    {
        player.playerData.MaxHealthMultiplication -= 2;
    }

    public override void OnTrigger(IDamageable damageable)
    {
        player.playerData.CurrentHealth += 50;
    }
}

#region 天赋

public class ForesightEnhance : BasePassiveSkill
{
    public ForesightEnhance(Player player) : base(player)
    {
        triggerType = TriggerType.Foresight;
    }

    public override void OnEnter()
    {
        player.RemovePassiveSkill(PassiveSkillType.LongerForesightEnhance);
    }

    public override void OnExit()
    {
        
    }

    public override void OnTrigger(IDamageable damageable)
    {
        player.GetBuff(BuffType.Foresight, 4);
        player.GetBuff(BuffType.ForesightEndurance, 1);
    }
}

public class LongerForesightEnhance : BasePassiveSkill
{
    public LongerForesightEnhance(Player player) : base(player)
    {
        triggerType = TriggerType.Foresight;
    }

    public override void OnEnter()
    {
        player.RemovePassiveSkill(PassiveSkillType.ForesightEnhance);
    }

    public override void OnExit()
    {

    }

    public override void OnTrigger(IDamageable damageable)
    {
        player.GetBuff(BuffType.Foresight, 8);
        player.GetBuff(BuffType.ForesightEndurance, 1);
    }
}

public class LongerForesightTime : BasePassiveSkill
{
    public LongerForesightTime(Player player) : base(player)
    {
        triggerType = TriggerType.ChangeValue;
    }

    public override void OnEnter()
    {
        player.playerController.PerfectCheckTime *= 1.5f;
    }

    public override void OnExit()
    {
        player.playerController.PerfectCheckTime *= 1.5f;
    }

    public override void OnTrigger(IDamageable damageable)
    {
        
    }
}

public class FastCharge : BasePassiveSkill
{
    public FastCharge(Player player) : base(player)
    {
        triggerType = TriggerType.ChangeValue;
    }

    public override void OnEnter()
    {
        player.playerData.chargeSpeedMultiplication += 0.1f;
    }

    public override void OnExit()
    {
        player.playerData.chargeSpeedMultiplication -= 0.1f;
    }

    public override void OnTrigger(IDamageable damageable)
    {

    }
}

public class FasterCharge : BasePassiveSkill
{
    public FasterCharge(Player player) : base(player)
    {
        triggerType = TriggerType.ChangeValue;
    }

    public override void OnEnter()
    {
        player.playerData.chargeSpeedMultiplication += 0.15f;
    }

    public override void OnExit()
    {
        player.playerData.chargeSpeedMultiplication -= 0.15f;
    }

    public override void OnTrigger(IDamageable damageable)
    {

    }
}

public class FastestCharge : BasePassiveSkill
{
    public FastestCharge(Player player) : base(player)
    {
        triggerType = TriggerType.ChangeValue;
    }

    public override void OnEnter()
    {
        player.playerData.chargeSpeedMultiplication += 0.25f;
    }

    public override void OnExit()
    {
        player.playerData.chargeSpeedMultiplication -= 0.25f;
    }

    public override void OnTrigger(IDamageable damageable)
    {

    }
}

#endregion

#region 武器

public class IronShellFurnanceSkill : BasePassiveSkill
{
    public IronShellFurnanceSkill(Player player) : base(player)
    {
        triggerType = TriggerType.GetHit;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnTrigger(IDamageable damageable)
    {
        if (CalculateProbability(0.1f))
            player.playerData.CurrentHealth += player.playerData.FinalMaxHealth * 0.2f;
    }
}

public class FirePenaltySkill : BasePassiveSkill
{
    public FirePenaltySkill(Player player) : base(player)
    {
        triggerType = TriggerType.Crit;
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void OnTrigger(IDamageable damageable)
    {
        damageable.GetBuff(BuffType.Burning, 5.1f);
    }
}

#endregion

#region 饰品

public class CombustionEngineSkill : BasePassiveSkill
{
    public CombustionEngineSkill(Player player) : base(player)
    {
        triggerType = TriggerType.Interval;
        interval = 10;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnTrigger(IDamageable damageable)
    {
        player.playerData.CurrentHealth += player.playerData.FinalMaxHealth * 0.01f;
    }
}

public class EnergyPumpSkill : BasePassiveSkill
{
    public EnergyPumpSkill(Player player) : base(player)
    {
        triggerType = TriggerType.Kill;
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void OnTrigger(IDamageable damageable)
    {
        player.playerData.CurrentMana += player.playerData.FinalMaxMana * 0.02f;
    }
}

public class AdvancedThrusterSkill : BasePassiveSkill
{
    public AdvancedThrusterSkill(Player player) : base(player)
    {
        triggerType = TriggerType.ChangeValue;
    }

    public override void OnEnter()
    {
        player.playerData.energyCostMultiplication -= 0.1f;
    }

    public override void OnExit()
    {
        player.playerData.energyCostMultiplication += 0.1f;
    }

    public override void OnTrigger(IDamageable damageable)
    {
        
    }
}

public class OverloadDeviceSkill : BasePassiveSkill
{
    public OverloadDeviceSkill(Player player) : base(player)
    {
        triggerType = TriggerType.Foresight;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnTrigger(IDamageable damageable)
    {
        player.playerData.CurrentEnergy += player.playerData.FinalMaxEnergy * 0.2f;
    }
}

public class ShockAbsorberSkill : BasePassiveSkill
{
    public ShockAbsorberSkill(Player player) : base(player)
    {
        triggerType = TriggerType.GetHit;
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void OnTrigger(IDamageable damageable)
    {
        player.GetBuff(BuffType.Tough, 5);
    }
}

#endregion