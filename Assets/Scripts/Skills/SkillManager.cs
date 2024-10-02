using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager>, ISaveable
{
    [Tooltip("技能点")]public int skillPoint;

    public void GetSaveData(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }

    public void LoadSaveData(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }

    [Tooltip("所有天赋列表")] public List<StaticSkillData> allSkillList;
    [Tooltip("已点亮天赋字典")] public Dictionary<int, LocalSkillData> skillDict;

    protected override void Awake()
    {
        base.Awake();

        skillDict = new Dictionary<int, LocalSkillData>();
        allSkillList.Sort();
    }

    private void OnEnable()
    {
        (this as ISaveable).Register();
    }

    private void OnDisable()
    {
        (this as ISaveable).UnRegister();
    }

    public bool CanUnlock(int id)
    {
        if (allSkillList[id] != null && allSkillList[id].preIDs.Length > 0)
        {
            for (int i = 0; i < allSkillList[id].preIDs.Length; i++)
            {
                if (!skillDict.ContainsKey(allSkillList[id].preIDs[i])) 
                    return false;
                else 
                    return true;
            }
        }

        else if (allSkillList[id].preIDs.Length == 0)
        {
            return true;
        }

        else
        {
            return false;
        }

        return false;
    }

    public void UpgradeSkill(int id)
    {
        if (skillPoint >= allSkillList[id].skillPointCost && CanUnlock(id))
        {
            if (!skillDict.ContainsKey(id))
            {
                skillDict.Add(id, new LocalSkillData(id));
            }
        }

    }

    public bool ConsumeSkillPoint(int number)
    {
        if (skillPoint >= number)
        {
            skillPoint -= number;
            return true;
        }
        else
        {
            UIManager.Instance.PlayTipSequence("技能点不足");
            return false;
        }
    }
}
