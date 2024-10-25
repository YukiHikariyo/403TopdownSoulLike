using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    //GameManager
    public Dictionary<string, int> savedExpLevelDict = new();

    //PackageManager
    public Dictionary<string, int> savedBottleDict = new();    
    public Dictionary<string, LocalItemData> savedItemDict = new();
    public Dictionary<string, LocalWeaponData> savedWeaponDict = new();
    public Dictionary<string, LocalAccessoryData> savedAccessoryDict = new();

    //SkillManager
    public Dictionary<string, int> savedSkillPointDict = new();
    public Dictionary<string, LocalSkillData> savedSkillDict = new();

    //PlayerData
    public Dictionary<string, bool> savedMagicDict = new();
    public Dictionary<string, float> savedPositionDict = new();

    //Interactable
    public Dictionary<string, bool> savedInteractableObjectDict = new();

    //EnemySpawner
    public Dictionary<string, bool> savedSpawnerDict = new();
}
