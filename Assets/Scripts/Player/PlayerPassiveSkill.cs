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
    TestPassiveSkill,
    ForesightEnhance,
    LongerForesightEnhance,
    LongerForesightTime,
    FastCharge,
    FasterCharge,
    FastestCharge,
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