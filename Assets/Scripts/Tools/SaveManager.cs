using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Audio;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;

public class SaveManager : MonoSingleton<SaveManager>
{
    private List<ISaveable> saveableList = new List<ISaveable>();
    private SaveData saveData = new SaveData();
    private SettingsData settingsData = new SettingsData();
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

    public void SaveSettings()
    {
        //保存设置
        if (!settingsData.savedVolumeDict.ContainsKey("MainVolume"))
            settingsData.savedVolumeDict.Add("MainVolume", AudioManager.Instance.currentMainSlider.value);
        else
            settingsData.savedVolumeDict["MainVolume"] = AudioManager.Instance.currentMainSlider.value;

        if (!settingsData.savedVolumeDict.ContainsKey("MusicVolume"))
            settingsData.savedVolumeDict.Add("MusicVolume", AudioManager.Instance.currentMusicSlider.value);
        else
            settingsData.savedVolumeDict["MusicVolume"] = AudioManager.Instance.currentMusicSlider.value;

        if (!settingsData.savedVolumeDict.ContainsKey("SFXVolume"))
            settingsData.savedVolumeDict.Add("SFXVolume", AudioManager.Instance.currentSFXSlider.value);
        else
            settingsData.savedVolumeDict["SFXVolume"] = AudioManager.Instance.currentSFXSlider.value;

        //写文件
        var resultPath = jsonFolder + "SettingsData.sav";
        var jsonData = JsonConvert.SerializeObject(settingsData, Formatting.Indented);

        if (!File.Exists(resultPath))
            Directory.CreateDirectory(jsonFolder);
        File.WriteAllText(resultPath, jsonData);
    }

    public void LoadSettings()
    {
        //读文件
        var resultPath = jsonFolder + "SettingsData.sav";
        if (!File.Exists(resultPath))
            return;

        var stringData = File.ReadAllText(resultPath);
        settingsData = JsonConvert.DeserializeObject<SettingsData>(stringData);

        //加载设置
        if (settingsData.savedVolumeDict.ContainsKey("MainVolume"))
            AudioManager.Instance.currentMainSlider.value = settingsData.savedVolumeDict["MainVolume"];

        if (settingsData.savedVolumeDict.ContainsKey("MusicVolume"))
            AudioManager.Instance.currentMusicSlider.value = settingsData.savedVolumeDict["MusicVolume"];

        if (settingsData.savedVolumeDict.ContainsKey("SFXVolume"))
            AudioManager.Instance.currentSFXSlider.value = settingsData.savedVolumeDict["SFXVolume"];

        AudioManager.Instance.UpdateVolume();
    }
}
