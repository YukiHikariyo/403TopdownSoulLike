using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// UI管理类
/// </summary>
public class UIManager : MonoSingleton<UIManager>
{
    public RectTransform mainCanvas;

    public Image tipPanel;
    public TextMeshProUGUI tipText;
    public Sequence tipSequence;

    [Space(16)]
    [Header("背包UI")]
    [Space(16)]

    public RectTransform packagePanel;
    public bool isPackageOpen;
    [Space(16)]
    public BasePanel currentMenuPanel;
    public List<Transform> menuSelectionList;
    public List<BasePanel> menuPanelList;
    public string[] menuNameList = { "玩家属性", "物品", "武器", "饰品", "天赋树", "设置" };
    public TextMeshProUGUI menuName;
    public int currentMenuIndex;
    public TextMeshProUGUI coinValue;

    [Space(16)]
    [Header("物品")]
    [Space(16)]

    public RectTransform itemSlots;
    public GameObject itemSlotPrefab;
    public ItemSlotUI currentSelectedItem;
    public Dictionary<int, ItemSlotUI> itemSlotDict;
    [Space(16)]
    public RectTransform itemDetailPanel;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemNumber;

    [Space(16)]
    [Header("武器")]
    [Space(16)]

    public RectTransform weaponSlots;
    public GameObject weaponSlotPrefab;
    public WeaponSlotUI currentSelectedWeapon;
    public WeaponSlotUI currentEquippedWeapon;
    [Space(16)]
    public RectTransform weaponDetailPanel;
    public TextMeshProUGUI selectedWeaponName;
    public TextMeshProUGUI selectedWeaponLevel;
    [Space(16)]
    public TextMeshProUGUI weaponStoneValue;
    public TextMeshProUGUI weaponCoinValue;
    public TextMeshProUGUI weaponDamageValue;
    public TextMeshProUGUI weaponCritRateValue;
    public TextMeshProUGUI weaponCritDamageValue;
    public TextMeshProUGUI weaponPenetratingPowerValue;
    public TextMeshProUGUI weaponReductionRateValue;

    [Space(16)]
    [Header("饰品")]
    [Space(16)]

    public RectTransform accessorySlots;
    public GameObject accessorySlotPrefab;
    public AccessorySlotUI currentSelectedAccessory;
    public Dictionary<int, AccessorySlotUI> currentEquippedAccessory = new();
    [Space(16)]
    public RectTransform accessoryDetailPanel;
    public TextMeshProUGUI selectedAccessoryName;
    public TextMeshProUGUI selectedAccessoryLevel;
    [Space(16)]
    public TextMeshProUGUI accessoryStoneValue;
    public TextMeshProUGUI accessoryCoinValue;
    public TextMeshProUGUI accessoryHealthValue;
    public TextMeshProUGUI accessoryManaValue;
    public TextMeshProUGUI accessoryEnergyValue;
    public TextMeshProUGUI accessoryToughnessValue;
    public TextMeshProUGUI accessoryReductionRateValue;

    protected override void Awake()
    {
        base.Awake();

        itemSlotDict = new Dictionary<int, ItemSlotUI>();

        InitializeTipSequence();
    }

    private void Update()
    {
        if (!isPackageOpen && Input.GetKeyDown(KeyCode.Escape))
            OpenPackage();
    }

    /// <summary>
    /// 初始化提示弹窗动画序列
    /// </summary>
    private void InitializeTipSequence()
    {
        tipSequence = DOTween.Sequence();
        tipSequence.Pause();
        tipSequence.SetAutoKill(false);
        tipSequence.SetEase(Ease.Linear);

        tipSequence.AppendCallback(() => 
        {
            tipPanel.gameObject.SetActive(false);
            tipPanel.color = new Color(tipPanel.color.r, tipPanel.color.g, tipPanel.color.b, 0);
            tipPanel.transform.position = new Vector3(tipPanel.transform.position.x, mainCanvas.rect.height * mainCanvas.lossyScale.y * 0.5f);
            tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, 0);
            tipPanel.gameObject.SetActive(true); 
        });
        tipSequence.Insert(0.1f, tipPanel.DOFade(0.3f, 0.5f));
        tipSequence.Insert(0.1f, tipPanel.transform.DOMoveY(mainCanvas.rect.height * mainCanvas.lossyScale.y * 0.625f, 0.5f));
        tipSequence.Insert(0.1f, tipText.DOFade(1, 0.5f));
        tipSequence.Insert(2.6f, tipPanel.DOFade(0, 0.5f));
        tipSequence.Insert(2.6f, tipPanel.transform.DOMoveY(mainCanvas.rect.height * mainCanvas.lossyScale.y * 0.75f, 0.5f));
        tipSequence.Insert(2.6f, tipText.DOFade(0, 0.5f));
        tipSequence.InsertCallback(3.1f, () => { tipPanel.gameObject.SetActive(false); });
    }

    /// <summary>
    /// 播放提示弹窗动画序列
    /// </summary>
    /// <param name="tip">提示内容</param>
    public void PlayTipSequence(string tip)
    {
        tipText.text = tip;
        tipSequence.Restart();
    }

    #region 背包

    /// <summary>
    /// 打开背包
    /// </summary>
    public void OpenPackage()
    {
        if (!currentMenuPanel)
            currentMenuPanel = menuPanelList[0];

        coinValue.text = PackageManager.Instance.CoinNumber().ToString();
        currentMenuPanel.OnOpen();
        packagePanel.gameObject.SetActive(true);
        isPackageOpen = true;
    }

    /// <summary>
    /// 关闭背包
    /// </summary>
    public void ClosePackage()
    {
        packagePanel.gameObject.SetActive(false);
        currentMenuPanel.OnClose();
        isPackageOpen = false;
    }

    /// <summary>
    /// 选择菜单
    /// </summary>
    /// <param name="index">菜单索引</param>
    public void SelectMenu(int index)
    {
        currentMenuIndex = index;

        for (int i = 0; i < menuSelectionList.Count; i++)
        {
            if (i != index)
            {
                menuSelectionList[i].GetChild(0).gameObject.SetActive(false);
                menuSelectionList[i].GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                menuSelectionList[i].GetChild(0).gameObject.SetActive(true);
                menuSelectionList[i].GetChild(1).gameObject.SetActive(false);
            }
        }

        menuName.text = menuNameList[index];

        currentMenuPanel.gameObject.SetActive(false);
        currentMenuPanel.OnClose();
        currentMenuPanel = menuPanelList[index];
        menuPanelList[index].OnOpen();
        menuPanelList[index].gameObject.SetActive(true);
    }

    #region 物品

    public void ItemInfUpdate(int id)
    {
        if (currentSelectedItem != null)
        {
            selectedItemName.text = PackageManager.Instance.allItemList[currentSelectedItem.itemID].itemName;
            selectedItemNumber.text = " 数量：" + PackageManager.Instance.itemDict[currentSelectedItem.itemID].number + "个";
        }
        
        itemSlotDict[id].itemNumber = PackageManager.Instance.itemDict[id].number;
        itemSlotDict[id].itemNumberText.text = itemSlotDict[id].itemNumber.ToString();
    }

    public void GetItem(int id, int number)
    {
        if (itemSlotDict.ContainsKey(id))
        {
            
        }
        else
        {
            GameObject item = Instantiate(itemSlotPrefab, itemSlots);
            ItemSlotUI itemSlot = item.GetComponent<ItemSlotUI>();
            itemSlot.Initialize(id, number);
            itemSlotDict.Add(id, itemSlot);
        }

        ItemInfUpdate(id);
    }

    public void ConsumeItem(int id, int number)
    {
        if (itemSlotDict[id].itemNumber >= number)
        {
            
        }
        else
            PlayTipSequence(PackageManager.Instance.allItemList[id].itemName + "数量不足");

        ItemInfUpdate(id);
    }

    public void SelectItem(ItemSlotUI selectedItemSlot)
    {
        currentSelectedItem?.transform.GetChild(4).gameObject.SetActive(false);
        currentSelectedItem = selectedItemSlot;
        currentSelectedItem.transform.GetChild(4).gameObject.SetActive(true);

        ItemInfUpdate(currentSelectedItem.itemID);
        itemDetailPanel.gameObject.SetActive(true);
    }

    #endregion

    #region 武器

    public void CurrentWeaponInfUpdate()
    {
        if (currentSelectedWeapon == null)
            return;

        int id = currentSelectedWeapon.weaponID;
        int level = PackageManager.Instance.weaponDict[id].level;
        selectedWeaponName.text = PackageManager.Instance.allWeaponList[id].name;
        selectedWeaponLevel.text = "LV." + level + (level >= PackageManager.Instance.allWeaponList[id].maxLevel ? "\n（满级）" : "");
        weaponStoneValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].stoneCost + "";
        weaponCoinValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].coinCost + "";
        weaponDamageValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].damage + "";
        weaponCritRateValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].critRate * 100 + "%";
        weaponCritDamageValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].critDamage * 100 + "%";
        weaponPenetratingPowerValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].penetratingPower * 100 + "%";
        weaponReductionRateValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].reductionRate * 100 + "%";

        weaponStoneValue.color = (PackageManager.Instance.itemDict.ContainsKey(1) && PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].stoneCost <= PackageManager.Instance.itemDict[1].number) ? Color.green : Color.red;
        weaponCoinValue.color = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].coinCost <= PackageManager.Instance.CoinNumber() ? Color.green : Color.red;
    }

    public void GetWeapon(int id)
    {
        GameObject weapon = Instantiate(weaponSlotPrefab, weaponSlots);
        weapon.GetComponent<WeaponSlotUI>().Initialize(id);
    }

    public void SelectWeapon(WeaponSlotUI selectedWeaponSlot)
    {
        currentSelectedWeapon?.transform.GetChild(3).gameObject.SetActive(false);
        currentSelectedWeapon = selectedWeaponSlot;
        currentSelectedWeapon.transform.GetChild(3).gameObject.SetActive(true);

        CurrentWeaponInfUpdate();
        weaponDetailPanel.gameObject.SetActive(true);
    }

    public void EquipWeapon()
    {
        currentEquippedWeapon?.transform.GetChild(4).gameObject.SetActive(false);
        currentEquippedWeapon = currentSelectedWeapon;
        currentEquippedWeapon?.transform.GetChild(4).gameObject.SetActive(true);

        PackageManager.Instance.EquipWeapon(currentSelectedWeapon.weaponID);

        PlayTipSequence("装备成功");
    }

    public void UpgradeWeapon()
    {
        if (PackageManager.Instance.weaponDict[currentSelectedWeapon.weaponID].level < PackageManager.Instance.allWeaponList[currentSelectedWeapon.weaponID].maxLevel)
        {
            PackageManager.Instance.UpgradeWeapon(currentSelectedWeapon.weaponID);
            CurrentWeaponInfUpdate();
        }
        else
            PlayTipSequence("当前武器已强化至最高级，无法继续强化");
    }

    #endregion

    #region 饰品

    public void CurrentAccessoryInfUpdate()
    {
        if (currentSelectedAccessory == null)
            return;

        int id = currentSelectedAccessory.accessoryID;
        int level = PackageManager.Instance.accessoryDict[id].level;
        selectedAccessoryName.text = PackageManager.Instance.allAccessoryList[id].name;
        selectedAccessoryLevel.text = "LV." + level + (level >= PackageManager.Instance.allAccessoryList[id].maxLevel ? "\n（满级）" : "");
        accessoryStoneValue.text = PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].stoneCost + "";
        accessoryCoinValue.text = PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].coinCost + "";
        accessoryHealthValue.text = PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].maxHealth + "";
        accessoryManaValue.text = PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].maxMana * 100 + "";
        accessoryEnergyValue.text = PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].maxEnergy * 100 + "";
        accessoryToughnessValue.text = PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].toughness * 100 + "";
        accessoryReductionRateValue.text = PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].reductionRate * 100 + "%";

        accessoryStoneValue.color = (PackageManager.Instance.itemDict.ContainsKey(2) && PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].stoneCost <= PackageManager.Instance.itemDict[2].number) ? Color.green : Color.red;
        accessoryCoinValue.color = PackageManager.Instance.allAccessoryList[id].accessoryStats[level - 1].coinCost <= PackageManager.Instance.CoinNumber() ? Color.green : Color.red;
    }

    public void GetAccessory(int id)
    {
        GameObject accessory = Instantiate(accessorySlotPrefab, accessorySlots);
        accessory.GetComponent<AccessorySlotUI>().Initialize(id);
    }

    public void SelectAccessory(AccessorySlotUI selectedAccessorySlot)
    {
        currentSelectedAccessory?.transform.GetChild(3).gameObject.SetActive(false);
        currentSelectedAccessory = selectedAccessorySlot;
        currentSelectedAccessory.transform.GetChild(3).gameObject.SetActive(true);

        CurrentAccessoryInfUpdate();
        accessoryDetailPanel.gameObject.SetActive(true);
    }

    private void EquipAccessory(int position)
    {
        if (currentEquippedAccessory.ContainsKey(position))
        {
            currentEquippedAccessory[position].transform.GetChild(4).gameObject.SetActive(false);
            currentEquippedAccessory[position] = currentSelectedAccessory;
        }
        else
        {
            currentEquippedAccessory.Add(position, currentSelectedAccessory);
        }

        currentEquippedAccessory[position].equipPosition.text = position + "号位";
        currentEquippedAccessory[position].transform.GetChild(4).gameObject.SetActive(true);

        PackageManager.Instance.EquipAccessory(currentSelectedAccessory.accessoryID, position);

        PlayTipSequence("已装备到" + position + "号饰品位");
    }
    public void EquipAccessory1() => EquipAccessory(1);
    public void EquipAccessory2() => EquipAccessory(2);
    public void EquipAccessory3() => EquipAccessory(3);

    public void UpgradeAccessory()
    {
        if (PackageManager.Instance.accessoryDict[currentSelectedAccessory.accessoryID].level < PackageManager.Instance.allAccessoryList[currentSelectedAccessory.accessoryID].maxLevel)
        {
            PackageManager.Instance.UpgradeAccessory(currentSelectedAccessory.accessoryID);
            CurrentAccessoryInfUpdate();
        }
        else
            PlayTipSequence("当前饰品已强化至最高级，无法继续强化");
    }

    #endregion

    #endregion
}
