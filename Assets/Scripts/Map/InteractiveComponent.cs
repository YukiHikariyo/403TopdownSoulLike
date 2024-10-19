using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveComponent : MonoBehaviour,ISaveable
{
    public Animator animator;
    protected PlayerStateMachine stateMachine;
    protected bool state;
   
    /// <summary>
    /// 当前状态
    /// </summary>
    public bool State
    {
        get
        { return state; }
        set
        {
            state = value;
            SwitchState();
        }
    }
    /// <summary>
    /// 初始化函数
    /// </summary>
    public virtual void Initialization()
    {

    }
    /// <summary>
    /// 状态切换逻辑
    /// </summary>
    protected virtual void SwitchState()
    {
        if (state)
        {
            animator.Play("Enabled");
        }
        else
        {
            animator.Play("Disabled");
        }
    }
    #region 存档接口
    public void GetSaveData(SaveData saveData)
    {

    }

    public void LoadSaveData(SaveData saveData) 
    {

    }
    #endregion
}
