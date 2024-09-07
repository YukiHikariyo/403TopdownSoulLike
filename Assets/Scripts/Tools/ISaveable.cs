using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void Register() => SaveManager.Instance.RegisterSaveable(this);
    public void UnRegister() => SaveManager.Instance.UnRegisterSaveable(this);
    public void GetSaveData(SaveData saveData);
    public void LoadSaveData(SaveData saveData);
}
