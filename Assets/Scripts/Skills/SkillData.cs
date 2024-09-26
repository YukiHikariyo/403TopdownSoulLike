using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSkillData : ScriptableObject
{
    //可能需要显示在UI上的数据
    
    [Tooltip("最大等级")] public int maxSkillLevel;
    [Tooltip("需求技能点")] public int skillPointCost;//需求技能点
    [Tooltip("节点贴图")] public Sprite skillSpite;
    [Tooltip("天赋名称")]  public string skillName;

    //内部数据
    [Tooltip("ID")] public int skillTreeID;//节点ID
    [Tooltip("天赋类型")] public SkillType skillType;//节点天赋类型
    [Tooltip("数值类型")] public ValueType skillValueType;//节点数值类型
    [Tooltip("改变数值大小")] public float[] skillValue;//节点数值（分等级）
    [Tooltip("解锁技能ID")] public int skillID;//解锁魔法ID


    //技能描述
    [TextArea(1,8)]
    public string skillDescription;//固定的文字描述    
}

public class LocalSkillData
{
    public int id;
    public int currentSkillLevel;
    public bool canUnlock;
    public bool isUnlocked;
    public LocalSkillData(int id,int currentSkillLevel = 0,bool canUnlock = false,bool isUnlocked = false)
    {
        this.currentSkillLevel = currentSkillLevel;
        this.id = id;
        this.canUnlock = canUnlock;
        this.isUnlocked = isUnlocked;
    }

}
public enum SkillType
{
    Value,Unlock
}
public enum ValueType
{
    MaxHealth, MaxMana, MaxEnergy,Damage,CritRate,CritDamage,ReductionRate,DamageRate
}
