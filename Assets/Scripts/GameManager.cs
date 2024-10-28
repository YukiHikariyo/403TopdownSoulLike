using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoSingleton<GameManager>, ISaveable
{
    public Player player;

    private string jsonFolder;

    [Space(16)]
    [Header("全局数值")]
    [Space(16)]

    [SerializeField][Tooltip("当前经验")] private int exp;
    [SerializeField][Tooltip("当前等级")] private int level;

    [Space(16)]
    [Header("流程控制UI")]
    [Space(16)]

    public GameObject loadingInfo;
    public Slider loadingSlider;
    public TextMeshProUGUI loadingValue;

    protected override void Awake()
    {
        base.Awake();

        jsonFolder = Application.persistentDataPath + "/Save/";
    }

    private void OnEnable()
    {
        (this as ISaveable).Register();
    }

    private void OnDisable()
    {
        (this as ISaveable).UnRegister();
    }

    public void GetSaveData(SaveData saveData)
    {
        if (!saveData.savedExpLevelDict.ContainsKey("Exp"))
            saveData.savedExpLevelDict.Add("Exp", exp);
        else
            saveData.savedExpLevelDict["Exp"] = exp;

        if (!saveData.savedExpLevelDict.ContainsKey("Level"))
            saveData.savedExpLevelDict.Add("Level", level);
        else
            saveData.savedExpLevelDict["Level"] = level;
    }

    public void LoadSaveData(SaveData saveData)
    {
        if (saveData.savedExpLevelDict.ContainsKey("Exp"))
            exp = saveData.savedExpLevelDict["Exp"];

        if (saveData.savedExpLevelDict.ContainsKey("Level"))
            level = saveData.savedExpLevelDict["Level"];

        UIManager.Instance.levelText.text = level.ToString();
        UIManager.Instance.expBar.slider.value = (float)exp / (float)CalculateExpByLevel();
    }

    #region 经验与等级

    private int CalculateExpByLevel() => Mathf.RoundToInt((level + 25) * (level + 25) * 0.17f + (level - 3) * 2);

    public int ExpNumber() => exp;

    public void GetExp(int number)
    {
        if (level >= 85)
            return;

        int times = 0;

        exp += number;
        while (exp >= CalculateExpByLevel())
        {
            exp = exp - CalculateExpByLevel();
            level++;
            SkillManager.Instance.skillPoint++;
            times++;
            if (level >= 85)
            {
                exp = 0;
                break;
            }
        }

        if (times == 0)
            UIManager.Instance.expBar.OnExpUp(exp, CalculateExpByLevel());
        else
            UIManager.Instance.expBar.OnLevelUp(exp, CalculateExpByLevel(), times, level);
    }

    [ContextMenu("获得50经验")]
    public void Get50Exp() => GetExp(50);

    [ContextMenu("获得300经验")]
    public void Get200Exp() => GetExp(300);

    #endregion

    #region 流程控制与场景切换

    public void ConfirmNewGame()
    {
        var savePath = jsonFolder + "SaveData.sav";
        if (File.Exists(savePath))
            File.Delete(savePath);

        OnLoadGameScene().Forget();
    }

    public void ContinueGame()
    {
        var savePath = jsonFolder + "SaveData.sav";
        if (File.Exists(savePath))
            OnLoadGameScene().Forget();
        else
            UIManager.Instance.PlayTipSequence("没有找到存档");
    }

    private async UniTask OnLoadGameScene()
    {
        UIManager.Instance.PlayFadeInSequence(2);

        await UniTask.Delay(TimeSpan.FromSeconds(2.5f));

        loadingInfo.SetActive(true);

        SaveManager.Instance.SaveSettings();
        AudioManager.Instance.ChangeSliders(1);
        SaveManager.Instance.LoadSettings();

        //await SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Persistent"));
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;

        while (!loadOperation.isDone)
        {
            loadingSlider.value = loadOperation.progress;
            loadingValue.text = (int)loadOperation.progress * 100 + "%";

            if (loadOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1;
                loadingValue.text = "100%";
                loadOperation.allowSceneActivation = true;

                await UniTask.NextFrame();
                break;
            }

            await UniTask.NextFrame();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));

        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));

        SaveManager.Instance.LoadGame();
        if (UIManager.Instance.isPackageOpen)
            UIManager.Instance.ClosePackage();

        player.playerData.CurrentHealth = player.playerData.FinalMaxHealth;
        player.playerData.CurrentMana = player.playerData.FinalMaxMana;
        player.playerData.CurrentEnergy = player.playerData.FinalMaxEnergy;

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        loadingInfo.SetActive(false);
        UIManager.Instance.mainMenu.SetActive(false);
        UIManager.Instance.gameInfo.SetActive(true);
        UIManager.Instance.PlayFadeOutSequence(2);
    }

    public void QuitGame()
    {
        SaveManager.Instance.SaveSettings();
        Application.Quit();
    }

    public void SaveGame()
    {
        OnSaveGame().Forget();
    }

    private async UniTask OnSaveGame()
    {
        player.playerInput.DisablePlayerInput();

        UIManager.Instance.PlayFadeInSequence(2);

        await UniTask.Delay(TimeSpan.FromSeconds(2.5f));

        SaveManager.Instance.SaveGame();

        player.playerData.CurrentHealth = player.playerData.FinalMaxHealth;
        player.playerData.CurrentMana = player.playerData.FinalMaxMana;
        player.playerData.CurrentEnergy = player.playerData.FinalMaxEnergy;

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        UIManager.Instance.PlayFadeOutSequence(2);

        await UniTask.Delay(TimeSpan.FromSeconds(2));

        player.playerInput.EnablePlayerInput();
    }

    public void BackToMainMenu()
    {
        OnBack().Forget();
    }

    private async UniTask OnBack()
    {
        player.gameObject.layer = 2;
        UIManager.Instance.PlayFadeInSequence(2);

        await UniTask.Delay(TimeSpan.FromSeconds(2.5f));

        SaveManager.Instance.SaveGame();

        SaveManager.Instance.SaveSettings();
        AudioManager.Instance.ChangeSliders(0);
        SaveManager.Instance.LoadSettings();

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Persistent"));
        await SceneManager.UnloadSceneAsync("GameScene");
        await SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
        UIManager.Instance.gameInfo.SetActive(false);
        UIManager.Instance.mainMenu.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        UIManager.Instance.PlayFadeOutSequence(2);

        await UniTask.Delay(TimeSpan.FromSeconds(2));
    }

    /// <summary>
    /// 传送玩家到指定位置，有切屏动画
    /// </summary>
    /// <param name="targetPosition">目标位置</param>
    public void TeleportPlayer(Vector3 targetPosition)
    {
        OnTeleport(targetPosition).Forget();
    }

    private async UniTask OnTeleport(Vector3 targetPosition)
    {
        player.playerInput.DisablePlayerInput();

        UIManager.Instance.PlayFadeInSequence(1);

        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

        player.transform.position = targetPosition;

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        UIManager.Instance.PlayFadeOutSequence(1);

        await UniTask.Delay(TimeSpan.FromSeconds(1));

        player.playerInput.EnablePlayerInput();
    }

    /// <summary>
    /// 玩家死亡触发的游戏流程逻辑
    /// </summary>
    [ContextMenu("死亡")]
    public void PlayerDeath()
    {
        OnPlayerDeath().Forget();
    }

    private async UniTask OnPlayerDeath()
    {
        player.gameObject.layer = 2;
        await UniTask.Delay(TimeSpan.FromSeconds(3));

        UIManager.Instance.ShowDeathText();
        UIManager.Instance.PlayFadeInSequence(2);

        await UniTask.Delay(TimeSpan.FromSeconds(10.5f));

        exp = Mathf.CeilToInt(exp * 0.5f);
        SaveManager.Instance.SaveGame();

        loadingInfo.SetActive(true);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Persistent"));
        await SceneManager.UnloadSceneAsync("GameScene");
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;

        while (!loadOperation.isDone)
        {
            loadingSlider.value = loadOperation.progress;
            loadingValue.text = (int)loadOperation.progress * 100 + "%";

            if (loadOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1;
                loadingValue.text = "100%";
                loadOperation.allowSceneActivation = true;

                await UniTask.NextFrame();
                break;
            }

            await UniTask.NextFrame();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));

        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));

        SaveManager.Instance.LoadGame();
        if (UIManager.Instance.isPackageOpen)
            UIManager.Instance.ClosePackage();

        player.playerData.CurrentHealth = player.playerData.FinalMaxHealth;
        player.playerData.CurrentMana = player.playerData.FinalMaxMana;
        player.playerData.CurrentEnergy = player.playerData.FinalMaxEnergy;

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        loadingInfo.SetActive(false);
        UIManager.Instance.mainMenu.SetActive(false);
        UIManager.Instance.gameInfo.SetActive(true);
        UIManager.Instance.PlayFadeOutSequence(2);
    }

    #endregion
}
