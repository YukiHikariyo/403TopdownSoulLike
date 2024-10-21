using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveComponent : MonoBehaviour, ISaveable
{
    public Animator animator;
    protected PlayerStateMachine stateMachine;
    protected bool state;

    private void OnEnable()
    {
        (this as ISaveable).Register();
    }

    private void OnDisable()
    {
        (this as ISaveable).UnRegister();
    }

    /// <summary>
    /// 当前状态
    /// </summary>
    public bool State
    {
        get => state;
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
        if (!saveData.savedInteractableObjectDict.ContainsKey(gameObject.name))
            saveData.savedInteractableObjectDict.Add(gameObject.name, State);
        else
            saveData.savedInteractableObjectDict[gameObject.name] = State;
    }

    public void LoadSaveData(SaveData saveData) 
    {
        if (saveData.savedInteractableObjectDict.ContainsKey(gameObject.name))
            State = saveData.savedInteractableObjectDict[gameObject.name];
    }

    #endregion
}
