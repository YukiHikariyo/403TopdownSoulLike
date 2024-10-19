using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveComponent : MonoBehaviour,ISaveable
{
    public Animator animator;

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
    public virtual void Initialization()
    {

    }

    protected virtual void SwitchState()
    {

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
