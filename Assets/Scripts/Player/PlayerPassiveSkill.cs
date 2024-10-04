using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPassiveSkill;

namespace PlayerPassiveSkill
{
    public enum TriggerType
    {
        [Tooltip("间隔固定时间触发")] Interval,
        [Tooltip("攻击触发")] Attack,
        [Tooltip("命中触发")] Hit,
        [Tooltip("击杀触发")] Kill,
        [Tooltip("受伤触发")] GetHit,
        [Tooltip("改变属性")] Change,
    }

}

public enum PassiveSkillType
{
    TestPassiveSkill,
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
    public abstract void OnTrigger();
}

public class TestPassiveSkill : BasePassiveSkill
{
    public TestPassiveSkill(Player player) : base(player)
    {
        triggerType = TriggerType.Interval;
        interval = 1;
    }

    public override void OnEnter()
    {
        float percent = player.playerData.CalculateHealthPercent();
        player.playerData.maxHealthMultiplication = 2;
        player.playerData.OnMaxHealthChange(percent);
    }

    public override void OnExit()
    {
        float percent = player.playerData.CalculateHealthPercent();
        player.playerData.maxHealthMultiplication = 1;
        player.playerData.OnMaxHealthChange(percent);
    }

    public override void OnTrigger()
    {
        player.playerData.CurrentHealth += 1;
    }
}
