using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

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

    public void UpgradeSkill(int id)
    {
        if (skillPoint >= allSkillList[id].skillPointCost && CanUnlock(id))
        {
            
            if (!skillDict.ContainsKey(id))
            {
                skillDict.Add(id, new LocalSkillData(id));
                skillDict[id].currentSkillLevel = 1;
                UnlockAnimation(id);
                ConsumeSkillPoint(allSkillList[id].skillPointCost);
            }

            else if (skillDict.ContainsKey(id) && skillDict[id].currentSkillLevel < allSkillList[id].maxSkillLevel)
            {
                skillDict[id].currentSkillLevel++;
                ConsumeSkillPoint(allSkillList[id].skillPointCost);
            }

            else if (skillDict.ContainsKey(id) && skillDict[id].currentSkillLevel == allSkillList[id].maxSkillLevel)
            {
                UIManager.Instance.PlayTipSequence("技能等级到达上限");
            }
        }
        else
        {
            UIManager.Instance.PlayTipSequence("无法升级");
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

    #region UI部分
    public StaticSkillData activeSkill;
    public int currentSkillIndex;

    [Header("UI")]
    public Image skillImage;
    public Text skillLv, skillDes, skillName, skillNum;
    public GameObject upgradeButton;
    public List<Transform> skillTreeList;

    public void DisplayInfo()
    {
        skillImage.sprite = activeSkill.skillSpite;
        skillDes.text = activeSkill.skillDescription;
        skillName.text = activeSkill.skillName;
        if(skillDict.ContainsKey(activeSkill.skillTreeID))
        {
            skillNum.text = activeSkill.skillValue[skillDict[activeSkill.skillTreeID].currentSkillLevel - 1].ToString();
            skillLv.text = skillDict[activeSkill.skillTreeID].currentSkillLevel.ToString();
        }
        else
        {
            skillNum.text = activeSkill.skillValue[0].ToString();
            skillLv.text = 0.ToString();
        }
        
    }
    public void DoUpgrade()
    {
        UpgradeSkill(activeSkill.skillTreeID);
        DisplayInfo();
    }

    public void SelectSkill(int index)
    {
        currentSkillIndex = index;

        for (int i = 0; i < skillTreeList.Count; i++)
        {
            if (i != index)
            {
                skillTreeList[i].GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                skillTreeList[i].GetChild(1).gameObject.SetActive(true);
            }
        }
        upgradeButton.gameObject.SetActive(true);
    }

    public void UnlockAnimation(int index)
    {
        currentSkillIndex = index;

        skillTreeList[currentSkillIndex].GetChild(3).GetComponent<Image>().DOFillAmount(1, 0.75f);

        skillTreeList[currentSkillIndex].GetChild(0).GetComponent<UILineRenderer>().color = new Color(1,1,1,1);

    }




    #endregion
}
