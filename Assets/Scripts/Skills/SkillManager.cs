using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SkillManager : MonoSingleton<SkillManager>, ISaveable
{
    public PlayerData playerData;

    [Tooltip("技能点")] public int skillPoint;

    [Tooltip("所有天赋列表")] public List<StaticSkillData> allSkillList;
    [Tooltip("已点亮天赋字典")] public Dictionary<int, LocalSkillData> skillDict = new();

    protected override void Awake()
    {
        base.Awake();

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

    public void GetSaveData(SaveData saveData)
    {
        if (!saveData.savedSkillPointDict.ContainsKey("SkillPoint"))
            saveData.savedSkillPointDict.Add("SkillPoint", skillPoint);
        else
            saveData.savedSkillPointDict["SkillPoint"] = skillPoint;

        foreach (LocalSkillData skill in skillDict.Values)
        {
            if (!saveData.savedSkillDict.ContainsKey(skill.id.ToString()))
                saveData.savedSkillDict.Add(skill.id.ToString(), skill);
            else
                saveData.savedSkillDict[skill.id.ToString()] = skill;
        }
    }

    public void LoadSaveData(SaveData saveData)
    {
        skillDict.Clear();

        if (saveData.savedSkillPointDict.ContainsKey("SkillPoint"))
            skillPoint = saveData.savedSkillPointDict["SkillPoint"];

        foreach (string skillID in saveData.savedSkillDict.Keys)
        {
            if (!skillDict.ContainsKey(int.Parse(skillID)))
            {
                for (int i = 0; i < saveData.savedSkillDict[skillID].currentSkillLevel; i++)
                {
                    UpgradeSkill(int.Parse(skillID), true);
                }
            }
        }

        Initialize();
    }

    public void Initialize()
    {
        foreach (var skillDot in skillDotList)
            skillDot.GetComponent<SkillTreeDot>().Initialize();

        detailPanel.SetActive(false);
    }

    public bool CanUnlock(int id)
    {
        if (allSkillList[id].preIDs.Length > 0)
        {
            for (int i = 0; i < allSkillList[id].preIDs.Length; i++)
            {
                if (!skillDict.ContainsKey(allSkillList[id].preIDs[i]))
                {
                    return false;
                }

                else
                {
                    return true;
                }

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

    public void UpgradeSkill(int id, bool ignoreLimitation = false)
    {
        if (ignoreLimitation || (skillPoint >= allSkillList[id].skillPointCost && CanUnlock(id)))
        { 
            if (!skillDict.ContainsKey(id))
            {
                skillDict.Add(id, new LocalSkillData(id));
                skillDict[id].currentSkillLevel = 1;
                UnlockAnimation(id);

                if (!ignoreLimitation)
                    ConsumeSkillPoint(allSkillList[id].skillPointCost);

                if (allSkillList[id].skillType == SkillType.Value)
                {
                    switch (allSkillList[id].valueType)
                    {
                        case ValueType.MaxHealth:
                            playerData.BasicMaxHealth += allSkillList[id].skillValue[0];
                            break;
                        case ValueType.MaxMana:
                            playerData.BasicMaxMana += allSkillList[id].skillValue[0];
                            break;
                        case ValueType.MaxEnergy:
                            playerData.BasicMaxEnergy += allSkillList[id].skillValue[0];
                            break;
                        case ValueType.EnergyRecovery:
                            playerData.TalentEnergyRecovery += allSkillList[id].skillValue[0];
                            break;
                        case ValueType.Damage:
                            playerData.TalentDamage += allSkillList[id].skillValue[0];
                            break;
                        case ValueType.CritRate:
                            playerData.TalentCritRate += allSkillList[id].skillValue[0];
                            break;
                        case ValueType.CritDamage:
                            playerData.TalentCritDamage += allSkillList[id].skillValue[0];
                            break;
                        case ValueType.PenetratingPower:
                            playerData.TalentPenetratingPower += allSkillList[id].skillValue[0];
                            break;
                        case ValueType.ReductionRate:
                            playerData.TalentReducitonRate += allSkillList[id].skillValue[0];
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    playerData.player.GetPassiveSkill(allSkillList[id].passiveSkillType);   //解锁对应被动
                }
            }
            else if (skillDict.ContainsKey(id) && skillDict[id].currentSkillLevel < allSkillList[id].maxSkillLevel)
            {

                int level = ++skillDict[id].currentSkillLevel;

                if (!ignoreLimitation)
                    ConsumeSkillPoint(allSkillList[id].skillPointCost);

                switch (allSkillList[id].valueType)
                {
                    case ValueType.MaxHealth:
                        playerData.BasicMaxHealth += allSkillList[id].skillValue[level - 1];
                        break;
                    case ValueType.MaxMana:
                        playerData.BasicMaxMana += allSkillList[id].skillValue[level - 1];
                        break;
                    case ValueType.MaxEnergy:
                        playerData.BasicMaxEnergy += allSkillList[id].skillValue[level - 1];
                        break;
                    case ValueType.EnergyRecovery:
                        playerData.TalentEnergyRecovery += allSkillList[id].skillValue[level - 1];
                        break;
                    case ValueType.Damage:
                        playerData.TalentDamage += allSkillList[id].skillValue[level - 1];
                        break;
                    case ValueType.CritRate:
                        playerData.TalentCritRate += allSkillList[id].skillValue[level - 1];
                        break;
                    case ValueType.CritDamage:
                        playerData.TalentCritDamage += allSkillList[id].skillValue[level - 1];
                        break;
                    case ValueType.PenetratingPower:
                        playerData.TalentPenetratingPower += allSkillList[id].skillValue[level - 1];
                        break;
                    case ValueType.ReductionRate:
                        playerData.TalentReducitonRate += allSkillList[id].skillValue[level - 1];
                        break;
                    default:
                        break;
                }
            }
            else if (skillDict.ContainsKey(id) && skillDict[id].currentSkillLevel == allSkillList[id].maxSkillLevel)
            {
                UIManager.Instance.PlayTipSequence("技能等级到达上限");
            }
        }
        else
        {
            UIManager.Instance.PlayTipSequence("改造点不足，无法升级");
        }
    }

    public bool ConsumeSkillPoint(int number)
    {
        if (skillPoint >= number)
        {
            skillPoint -= number;
            skillPointText.text = skillPoint.ToString();
            return true;
        }
        else
        {
            UIManager.Instance.PlayTipSequence("技能点不足");
            return false;
        }
    }

    #region UI部分
    public StaticSkillData activeSkill;
    public int currentSkillIndex;

    [Header("UI")]
    public GameObject detailPanel;
    public List<Transform> skillDotList;
    public TextMeshProUGUI skillLv, skillDes, skillName, skillCost, skillPointText;
    public GameObject upgradeButton;

    public void DisplayInfo()
    {
        skillDes.text = activeSkill.skillDescription;
        skillName.text = activeSkill.skillName;
        skillCost.text = activeSkill.skillPointCost.ToString();
        if (skillDict.ContainsKey(activeSkill.skillDotID))
        {
            skillLv.text = "LV." + skillDict[activeSkill.skillDotID].currentSkillLevel;
        }
        else
        {
            skillLv.text = "LV.0";
        }

        if (CanUnlock(activeSkill.skillDotID))
            upgradeButton.SetActive(true);
        else
            upgradeButton.SetActive(false);

        if (skillDict.ContainsKey(activeSkill.skillDotID) && skillDict[activeSkill.skillDotID].currentSkillLevel >= activeSkill.maxSkillLevel)
            upgradeButton.SetActive(false);

        detailPanel.SetActive(true);
    }
    public void DoUpgrade()
    {
        UpgradeSkill(activeSkill.skillDotID);
        DisplayInfo();
    }

    public void SelectSkill(int index, StaticSkillData selectedSkill)
    {
        activeSkill = selectedSkill;
        currentSkillIndex = index;

        for (int i = 0; i < skillDotList.Count; i++)
        {
            if (i != index)
            {
                skillDotList[i].GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                skillDotList[i].GetChild(2).gameObject.SetActive(true);
            }
        }

        DisplayInfo();
    }

    public void UnlockAnimation(int index)
    {
        currentSkillIndex = index;

        skillDotList[currentSkillIndex].GetChild(4).GetComponent<Image>().DOFillAmount(1, 0.75f);

        skillDotList[currentSkillIndex].GetChild(0).GetComponent<UILineRenderer>().color = new Color(0.9137f,0.6078f,0.1098f,1);

    }

    #endregion
}
