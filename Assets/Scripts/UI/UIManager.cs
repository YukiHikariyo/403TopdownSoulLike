using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI critRateValue;
    public TextMeshProUGUI critDamageValue;
    public TextMeshProUGUI penetratingPowerValue;
    public TextMeshProUGUI reductionRateValue;

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
    private void PlayTipSequence(string tip)
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

    public void GetItem(int id, int number)
    {
        if (itemSlotDict.ContainsKey(id))
        {
            itemSlotDict[id].itemNumber = itemSlotDict[id].itemNumber + number < 999 ? number : 999;
            itemSlotDict[id].itemNumberText.text = itemSlotDict[id].itemNumber.ToString();
        }
        else
        {
            GameObject item = Instantiate(itemSlotPrefab, itemSlots);
            ItemSlotUI itemSlot = item.GetComponent<ItemSlotUI>();
            itemSlot.Initialize(id, number);
            itemSlotDict.Add(id, itemSlot);
        }
    }

    public void ConsumeItem(int id, int number)
    {
        if (itemSlotDict[id].itemNumber >= number)
        {
            itemSlotDict[id].itemNumber -= number;
            itemSlotDict[id].itemNumberText.text = itemSlotDict[id].itemNumber.ToString();
        }
        else
            PlayTipSequence(PackageManager.Instance.allItemList[id].itemName + "数量不足");
    }

    public void SelectItem(ItemSlotUI selectedItemSlot)
    {
        currentSelectedItem?.transform.GetChild(4).gameObject.SetActive(false);
        currentSelectedItem = selectedItemSlot;
        currentSelectedItem.transform.GetChild(4).gameObject.SetActive(true);

        int id = currentSelectedItem.itemID;
        selectedItemName.text = PackageManager.Instance.allItemList[id].itemName;
        selectedItemNumber.text = " 数量：" + PackageManager.Instance.itemDict[id].number + "个";
        itemDetailPanel.gameObject.SetActive(true);
    }

    #endregion

    #region 武器

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

        int id = currentSelectedWeapon.weaponID;
        int level = PackageManager.Instance.weaponDict[id].level;
        selectedWeaponName.text = PackageManager.Instance.allWeaponList[id].name;
        selectedWeaponLevel.text = "LV." + level;
        damageValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].damage + "";
        critRateValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].critRate * 100 + "%";
        critDamageValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].critDamage * 100 + "%";
        penetratingPowerValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].penetratingPower * 100 + "%";
        reductionRateValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].reductionRate * 100 + "%";
        weaponDetailPanel.gameObject.SetActive(true);
    }

    public void EquipWeapon()
    {
        currentEquippedWeapon?.transform.GetChild(4).gameObject.SetActive(false);
        currentEquippedWeapon = currentSelectedWeapon;
        currentEquippedWeapon?.transform.GetChild(4).gameObject.SetActive(true);

        PackageManager.Instance.EquipWeapon(currentSelectedWeapon.weaponID);
    }

    public void UpgradeWeapon()
    {

    }

    #endregion

    #endregion
}
