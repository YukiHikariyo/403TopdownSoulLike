using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包管理类
/// </summary>
public class PackageManager : MonoSingleton<PackageManager>, ISaveable
{
    public PlayerData playerData;

    [SerializeField][Tooltip("当前金币")] private int coin;

    [SerializeField][Tooltip("血瓶上限")] private int maxHealthBottle;
    public int MaxHealthBottle { get => maxHealthBottle; }

    [SerializeField][Tooltip("当前血瓶")] private int currentHealthBottle;
    public int CurrentHealthBottle { get => currentHealthBottle; }

    [SerializeField][Tooltip("蓝瓶上限")] private int maxManaBottle;
    public int MaxManaBottle { get => maxManaBottle; }

    [SerializeField][Tooltip("当前血瓶")] private int currentManaBottle;
    public int CurrentManaBottle { get => currentManaBottle; }

    [Tooltip("所有物品列表")] public List<StaticItemData> allItemList;
    [Tooltip("已获得道具字典")] public Dictionary<int, LocalItemData> itemDict = new();

    [Tooltip("所有武器列表")] public List<StaticWeaponData> allWeaponList;
    [Tooltip("已获得武器字典")] public Dictionary<int, LocalWeaponData> weaponDict = new();
    public LocalWeaponData currentWeapon;

    [Tooltip("所有饰品列表")] public List<StaticAccessoryData> allAccessoryList;
    [Tooltip("已获得饰品字典")] public Dictionary<int, LocalAccessoryData> accessoryDict = new();
    public Dictionary<int, LocalAccessoryData> currentAccessory = new();

    protected override void Awake()
    {
        base.Awake();

        allItemList.Sort();
        allWeaponList.Sort();
        allAccessoryList.Sort();
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
        foreach (LocalWeaponData weapon in weaponDict.Values)
        {
            if (!saveData.weaponDict.ContainsKey(weapon.id))
                saveData.weaponDict.Add(weapon.id, weapon);
            else
                saveData.weaponDict[weapon.id] = weapon;
        }
    }

    public void LoadSaveData(SaveData saveData)
    {
        foreach (int weaponID in saveData.weaponDict.Keys)
        {
            if (!weaponDict.ContainsKey(weaponID))
                weaponDict.Add(weaponID, saveData.weaponDict[weaponID]);
            else
                weaponDict[weaponID] = saveData.weaponDict[weaponID];
        }
    }

    #region 血蓝瓶

    /// <summary>
    /// 获得血瓶上限
    /// </summary>
    [ContextMenu("获得血瓶上限")]
    public void GetMaxHealthBottle()
    {
        maxHealthBottle++;
        currentHealthBottle++;
        UIManager.Instance.HealthBottleBarUpdate();
    }

    /// <summary>
    /// 获得蓝瓶上限
    /// </summary>
    [ContextMenu("获得蓝瓶上限")]
    public void GetMaxManaBottle()
    {
        maxManaBottle++;
        currentManaBottle++;
        UIManager.Instance.ManaBottleBarUpdate();
    }

    /// <summary>
    /// 消耗血瓶恢复血量
    /// </summary>
    /// <returns>是否成功消耗</returns>
    [ContextMenu("消耗血瓶恢复血量")]
    public bool ConsumeHealthBottle()
    {
        if (currentHealthBottle > 0)
        {
            currentHealthBottle--;
            playerData.CurrentHealth += playerData.FinalMaxHealth * 0.4f;
            UIManager.Instance.HealthBottleBarUpdate();

            return true;
        }

        return false;
    }

    /// <summary>
    /// 消耗蓝瓶恢复魔力
    /// </summary>
    /// <returns>是否成功消耗</returns>
    [ContextMenu("消耗蓝瓶恢复魔力")]
    public bool ConsumeManaBottle()
    {
        if (currentManaBottle > 0)
        {
            currentManaBottle--;
            playerData.CurrentMana += playerData.FinalMaxMana * 0.4f;
            UIManager.Instance.ManaBottleBarUpdate();

            return true;
        }

        return false;
    }

    #endregion

    #region 金币

    /// <summary>
    /// 查询当前金币数量
    /// </summary>
    /// <returns>金币数量</returns>
    public int CoinNumber() => coin;

    /// <summary>
    /// 获得金币
    /// </summary>
    /// <param name="number">获得数量</param>
    public void GetCoin(int number)
    {
        coin = coin + number < 999999 ? coin + number : 999999;
        UIManager.Instance.coinValue.text = coin.ToString();
    }

    /// <summary>
    /// 消耗金币
    /// </summary>
    /// <param name="number">消耗数量</param>
    /// <returns>是否成功消耗</returns>
    /// <remarks>特别重要：同时消耗多种物品（包括金币）时，不要用此方法的返回值判断是否成功消耗</remarks>
    public bool ConsumeCoin(int number)
    {
        if (coin >= number)
        {
            coin -= number;
            UIManager.Instance.coinValue.text = coin.ToString();
            return true;
        }
        else
        {
            UIManager.Instance.PlayTipSequence("金币不足");
            return false;
        }
    }

    #endregion

    #region 物品

    /// <summary>
    /// 获得物品
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <param name="number">获得数量</param>
    public void GetItem(int id, int number)
    {
        if (itemDict.ContainsKey(id))
            itemDict[id].number = itemDict[id].number + number < 999 ? itemDict[id].number + number : 999;
        else
            itemDict.Add(id, new LocalItemData(id, number));

        UIManager.Instance.GetItem(id, number);
        UIManager.Instance.PlayTipSequence("获得物品：" + allItemList[id].itemName + "x" + number);
    }

    [ContextMenu("获得3个测试物品")]
    public void TestGetItem0() => GetItem(0, 3);

    [ContextMenu("获得10个锻造石")]
    public void TestGetItem1() => GetItem(1, 10);

    [ContextMenu("获得10个强化石")]
    public void TestGetItem2() => GetItem(2, 10);

    /// <summary>
    /// 消耗物品
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <param name="number">消耗数量</param>
    /// <returns>是否成功消耗</returns>
    /// <remarks>特别重要：同时消耗多种物品（包括金币）时，不要用此方法的返回值判断是否成功消耗</remarks>
    public bool ConsumeItem(int id, int number)
    {
        if (itemDict.ContainsKey(id))
        {
            UIManager.Instance.ConsumeItem(id, number);
            if (itemDict[id].number >= number)
            {
                itemDict[id].number -= number;
                return true;
            }
        }

        return false;
    }

    [ContextMenu("消耗2个测试物品")]
    public bool TestConsumeItem0() => ConsumeItem(0, 2);

    #endregion

    #region 武器

    /// <summary>
    /// 获得武器
    /// </summary>
    /// <param name="id">武器ID</param>
    public void GetWeapon(int id)
    {
        if (!weaponDict.ContainsKey(id))
        {
            weaponDict.Add(id, new LocalWeaponData(id));
            UIManager.Instance.GetWeapon(id);
            UIManager.Instance.OpenConfirmationPanel("获得武器：" + allWeaponList[id].weaponName);
        }
    }

    [ContextMenu("添加测试武器")]
    public void TestGetWeapon() => GetWeapon(0);

    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="id">武器ID</param>
    /// <remarks>此方法在UIManager中的EquipWeapon方法中调用</remarks>
    public void EquipWeapon(int id)
    {
        if (weaponDict.ContainsKey(id))
        {
            if (currentWeapon != null)
                currentWeapon.isEquipped = false;
            weaponDict[id].isEquipped = true;

            if (playerData.currentWeaponStaticData != null && playerData.currentWeaponStaticData.passiveSkillType != PassiveSkillType.None)
                playerData.player.RemovePassiveSkill(playerData.currentWeaponStaticData.passiveSkillType);

            playerData.currentWeaponStaticData = allWeaponList[id];
            playerData.currentWeaponLocalData = weaponDict[id];

            if (playerData.currentWeaponStaticData.passiveSkillType != PassiveSkillType.None)
                playerData.player.GetPassiveSkill(playerData.currentWeaponStaticData.passiveSkillType);

            UIManager.Instance.PlayTipSequence("装备成功");
        }
    }

    /// <summary>
    /// 升级武器
    /// </summary>
    /// <param name="id">武器ID</param>
    /// <remarks>此方法在UIManager中的UpgradeWeapon方法中调用</remarks>
    public void UpgradeWeapon(int id)
    {
        if (weaponDict.ContainsKey(id))
        {
            if (itemDict.ContainsKey(1) && itemDict[1].number >= allWeaponList[id].weaponStats[weaponDict[id].level - 1].stoneCost && coin >= allWeaponList[id].weaponStats[weaponDict[id].level - 1].coinCost)
            {
                ConsumeItem(1, allWeaponList[id].weaponStats[weaponDict[id].level - 1].stoneCost);
                ConsumeCoin(allWeaponList[id].weaponStats[weaponDict[id].level - 1].coinCost);
                weaponDict[id].level++;
                UIManager.Instance.PlayTipSequence("强化成功");
            }
            else if (!itemDict.ContainsKey(1) || itemDict[1].number < allWeaponList[id].weaponStats[weaponDict[id].level - 1].stoneCost)
                UIManager.Instance.PlayTipSequence("武器强化零件数量不足");
            else if (coin < allWeaponList[id].weaponStats[weaponDict[id].level - 1].coinCost)
                UIManager.Instance.PlayTipSequence("金币不足");
        }
    }

    #endregion

    #region 配件（饰品）

    /// <summary>
    /// 获得饰品
    /// </summary>
    /// <param name="id">饰品ID</param>
    public void GetAccessory(int id)
    {
        if (!accessoryDict.ContainsKey(id))
        {
            accessoryDict.Add(id, new LocalAccessoryData(id));
            UIManager.Instance.GetAccessory(id);
            UIManager.Instance.OpenConfirmationPanel("获得配件：" + allAccessoryList[id].accessoryName);
        }
    }

    [ContextMenu("添加测试饰品")]
    public void GetTestAccessory() => GetAccessory(0);

    /// <summary>
    /// 装备饰品
    /// </summary>
    /// <param name="id">饰品ID</param>
    /// <param name="position">装备位置(1~3)</param>
    /// <remarks>此方法在UIManager中的EquipAccessory方法调用</remarks>
    public void EquipAccessory(int id, int position)
    {
        float healthPercent = playerData.CalculateHealthPercent();
        float manaPercent = playerData.CalculateManaPercent();
        float energyPercent = playerData.CalculateEnergyPercent();

        int rm = 0;

        if (accessoryDict.ContainsKey(id))
        {
            if (currentAccessory.ContainsKey(position))
                currentAccessory[position].equipPosition = 0;
            else
                currentAccessory.Add(position, accessoryDict[id]);

            foreach (int p in playerData.currentAccessoryStaticData.Keys)
            {
                if (playerData.currentAccessoryStaticData[p].accessoryID == id)
                    rm = p;
            }

            if (rm != 0)
            {
                playerData.player.RemovePassiveSkill(playerData.currentAccessoryStaticData[rm].passiveSkillType);
                playerData.currentAccessoryStaticData.Remove(rm);
                playerData.currentAccessoryLocalData.Remove(rm);
            }

            if (playerData.currentAccessoryStaticData.ContainsKey(position))
            {
                if (playerData.currentAccessoryStaticData[position].passiveSkillType != PassiveSkillType.None)
                    playerData.player.RemovePassiveSkill(playerData.currentAccessoryStaticData[position].passiveSkillType);

                playerData.currentAccessoryStaticData[position] = allAccessoryList[id];
            }
            else
                playerData.currentAccessoryStaticData.Add(position, allAccessoryList[id]);

            if (playerData.currentAccessoryStaticData[position].passiveSkillType != PassiveSkillType.None)
                playerData.player.GetPassiveSkill(playerData.currentAccessoryStaticData[position].passiveSkillType);

            if (playerData.currentAccessoryLocalData.ContainsKey(position))
                playerData.currentAccessoryLocalData[position] = accessoryDict[id];
            else
                playerData.currentAccessoryLocalData.Add(position, accessoryDict[id]);

            UIManager.Instance.PlayTipSequence("成功装备到" + position + "号位");
        }

        playerData.OnMaxHealthChange(healthPercent);
        playerData.OnMaxManaChange(manaPercent);
        playerData.OnMaxEnergyChange(energyPercent);
    }

    /// <summary>
    /// 强化饰品
    /// </summary>
    /// <param name="id">饰品ID</param>
    /// <remarks>此方法在UIManager中的UpgradeAccessory方法调用</remarks>
    public void UpgradeAccessory(int id)
    {
        if (accessoryDict.ContainsKey(id))
        {
            if (itemDict.ContainsKey(2) && itemDict[2].number >= allAccessoryList[id].accessoryStats[accessoryDict[id].level - 1].stoneCost && coin >= allAccessoryList[id].accessoryStats[accessoryDict[id].level - 1].coinCost)
            {
                float healthPercent = playerData.CurrentHealth / playerData.FinalMaxHealth;
                float manaPercent = playerData.CurrentMana / playerData.FinalMaxMana;
                float energyPercent = playerData.CurrentEnergy / playerData.FinalMaxEnergy;

                ConsumeItem(2, allAccessoryList[id].accessoryStats[accessoryDict[id].level - 1].stoneCost);
                ConsumeCoin(allAccessoryList[id].accessoryStats[accessoryDict[id].level - 1].coinCost);
                accessoryDict[id].level++;
                UIManager.Instance.PlayTipSequence("强化成功");

                playerData.OnMaxHealthChange(healthPercent);
                playerData.OnMaxManaChange(manaPercent);
                playerData.OnMaxEnergyChange(energyPercent);
            }
            else if (!itemDict.ContainsKey(2) || itemDict[2].number < allAccessoryList[id].accessoryStats[accessoryDict[id].level - 1].stoneCost)
                UIManager.Instance.PlayTipSequence("配件强化零件数量不足");
            else if (coin < allAccessoryList[id].accessoryStats[accessoryDict[id].level - 1].coinCost)
                UIManager.Instance.PlayTipSequence("金币不足");
        }
    }

    #endregion
}
