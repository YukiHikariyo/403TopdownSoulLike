using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveComponent : ISaveable
{


    private void OnEnable()
    {
        (this as ISaveable).Register();
    }

    private void OnDisable()
    {
        (this as ISaveable).UnRegister();
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
