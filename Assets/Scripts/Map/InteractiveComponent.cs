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
            Initialization();
        }
    }

    /// <summary>
    /// 初始化函数,存档加载时调用
    /// </summary>
    public virtual void Initialization()
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

    /// <summary>
    /// 状态切换逻辑，玩家在游戏内互动时触发
    /// </summary>
    public virtual void SwitchState()
    {
        
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
