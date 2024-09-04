using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : Singleton<GameManager>
{
    private List<ISaveable> saveableList = new List<ISaveable>();
    public SaveData saveData = new SaveData();

    public void RegisterSaveable(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
            saveableList.Add(saveable);
    }
    public void UnRegisterSaveable(ISaveable saveable)
    {
        if (saveableList.Contains(saveable))
            saveableList.Remove(saveable);
    }
}
