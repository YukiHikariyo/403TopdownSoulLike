using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    //Load顺序：GameManager → UIManager → PlayerData → PackageManager → SkillManager

    //GameManager
    public Dictionary<string, int> savedExpLevelDict = new();

    //PackageManager
    public Dictionary<string, int> savedBottleDict = new();    
    public Dictionary<int, LocalItemData> savedItemDict = new();
    public Dictionary<int, LocalWeaponData> savedWeaponDict = new();
    public Dictionary<int, LocalAccessoryData> savedAccessoryDict = new();

    //PlayerData
    public Dictionary<string, float> savedPlayerFloatDict = new();
}
