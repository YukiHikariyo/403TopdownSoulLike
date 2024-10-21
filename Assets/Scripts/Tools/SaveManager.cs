using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveManager : MonoSingleton<SaveManager>
{
    private List<ISaveable> saveableList = new List<ISaveable>();
    private SaveData saveData = new SaveData();
    private string jsonFolder;

    protected override void Awake()
    {
        base.Awake();

        jsonFolder = Application.persistentDataPath + "/Save/";
    }

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
        foreach (var saveable in saveableList)
            saveable.GetSaveData(saveData);

        var resultPath = jsonFolder + "SaveData.sav";
        var jsonData = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        if (!File.Exists(resultPath))
            Directory.CreateDirectory(jsonFolder);
        File.WriteAllText(resultPath, jsonData);
    }

    public void LoadGame()
    {
        var resultPath = jsonFolder + "SaveData.sav";
        if (!File.Exists(resultPath))
            return;


        var stringData = File.ReadAllText(resultPath);
        var jsonData = JsonConvert.DeserializeObject<SaveData>(stringData);

        foreach (var saveable in saveableList)
            saveable.LoadSaveData(jsonData);
    }
}
