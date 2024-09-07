using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    private List<ISaveable> saveableList = new List<ISaveable>();
    private SaveData saveData = new SaveData();

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

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }
}
