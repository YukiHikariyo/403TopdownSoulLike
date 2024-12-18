using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveComponent : MonoBehaviour, ISaveable
{
    public Animator animator;
    public bool alwaysInteractive = false;
    public bool showTips = true;
    public PlayerStateMachine stateMachine;
    protected bool state;
    protected bool upperState;


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

    public bool UpperState { get; set; }

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
    /// 状态切换逻辑，玩家在游戏内互动时触发，返回值表示是否成功改变组件State
    /// </summary>
    public virtual bool SwitchState()
    {
        return true;
    }

    #region 存档接口
    public void GetSaveData(SaveData saveData)
    {
        if (!saveData.savedInteractableObjectDict.ContainsKey(gameObject.name))
            saveData.savedInteractableObjectDict.Add(gameObject.name, State);
        else
            saveData.savedInteractableObjectDict[gameObject.name] = State;

        if (!saveData.savedInteractableObjectDict.ContainsKey(gameObject.name + "Upper"))
            saveData.savedInteractableObjectDict.Add(gameObject.name + "Upper", UpperState);
        else
            saveData.savedInteractableObjectDict[gameObject.name + "Upper"] = UpperState;
    }

    public void LoadSaveData(SaveData saveData) 
    {
        if (saveData.savedInteractableObjectDict.ContainsKey(gameObject.name + "Upper"))
            UpperState = saveData.savedInteractableObjectDict[gameObject.name + "Upper"];

        if (saveData.savedInteractableObjectDict.ContainsKey(gameObject.name))
            State = saveData.savedInteractableObjectDict[gameObject.name];
    }

    #endregion
}
