using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("引用缓存")]
    [Space(16)]
    public Player player;

    [Space(16)]
    [Header("全局数值")]
    [Space(16)]

    [SerializeField][Tooltip("当前经验")] private int exp;
    [SerializeField][Tooltip("当前等级")] private int level;

    #region 经验与等级

    private int CalculateExpByLevel() => Mathf.RoundToInt((level + 25) * (level + 25) * 0.17f + (level - 3) * 2);

    public int ExpNumber() => exp;

    public void GetExp(int number)
    {
        if (level >= 85)
            return;

        int times = 0;

        exp += number;
        while (exp >= CalculateExpByLevel())
        {
            exp = exp - CalculateExpByLevel();
            level++;
            SkillManager.Instance.skillPoint++;
            times++;
            if (level >= 85)
            {
                exp = 0;
                break;
            }
        }

        if (times == 0)
            UIManager.Instance.expBar.OnExpUp(exp, CalculateExpByLevel());
        else
            UIManager.Instance.expBar.OnLevelUp(exp, CalculateExpByLevel(), times, level);
    }

    [ContextMenu("获得50经验")]
    public void Get50Exp() => GetExp(50);

    [ContextMenu("获得300经验")]
    public void Get200Exp() => GetExp(300);

    #endregion
}
