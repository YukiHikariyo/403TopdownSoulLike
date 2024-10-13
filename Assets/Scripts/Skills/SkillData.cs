using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/Skill/SkillData")]
public class StaticSkillData : ScriptableObject, IComparable<StaticSkillData>
{
    //可能需要显示在UI上的数据
    
    [Tooltip("最大等级")] public int maxSkillLevel;
    [Tooltip("需求技能点")] public int skillPointCost;//需求技能点
    [Tooltip("节点贴图")] public Sprite skillSpite;
    [Tooltip("天赋名称")]  public string skillName;

    //内部数据
    [Tooltip("ID")] public int skillDotID;//节点ID
    [Tooltip("天赋类型")] public SkillType skillType;//节点天赋类型
    [Tooltip("数值类型")] public ValueType valueType;//节点数值类型
    [Tooltip("改变数值大小")] public float[] skillValue;//节点数值（分等级）
    [Tooltip("解锁魔法ID")] public int magicID;//解锁魔法ID
    [Tooltip("解锁被动技能")] public PassiveSkillType passiveSkillType;//解锁被动ID


    //技能描述
    [TextArea(1,8)]
    public string skillDescription;//固定的文字描述    

    //技能前置
    [Tooltip("前置天赋ID")] public int[] preIDs;

    public int CompareTo(StaticSkillData other) => skillDotID - other.skillDotID;
}

public class LocalSkillData
{
    public int id;
    public int currentSkillLevel;
    public LocalSkillData(int id,int currentSkillLevel = 0)
    {
        this.currentSkillLevel = currentSkillLevel;
        this.id = id;
    }

}
public enum SkillType
{
    Value, UnlockMagic, UnlockPassiveSkill
}
public enum ValueType
{
    MaxHealth, MaxMana, MaxEnergy, EnergyRecovery, Damage, CritRate, CritDamage, PenetratingPower, ReductionRate
}
