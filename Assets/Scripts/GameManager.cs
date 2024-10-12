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

    private int CalculateExpByLevel() => Mathf.RoundToInt((level + 24) * (level + 24) * 0.17f + (level - 4) * 2);

    public int ExpNumber() => exp;

    public void GetExp(int number)
    {
        if (level >= 85)
            return;

        int times = 0;
        int maxExp = CalculateExpByLevel();

        exp += number;
        while (exp >= maxExp)
        {
            exp = exp - maxExp;
            level++;
            times++;
            if (level >= 85)
            {
                exp = 0;
                break;
            }
        }

        if (times == 0)
            UIManager.Instance.expBar.OnExpUp(exp, maxExp);
        else
            UIManager.Instance.expBar.OnLevelUp(exp, maxExp, times, level);
    }

    #endregion
}
