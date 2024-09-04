using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void Register() => GameManager.Instance.RegisterSaveable(this);
    public void UnRegister() => GameManager.Instance.UnRegisterSaveable(this);
    public void GetSaveData(SaveData saveData);
    public void LoadSaveData(SaveData saveData);
}
