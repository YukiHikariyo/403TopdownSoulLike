using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    public Transform mainCanvas;

    [Space(16)]
    [Header("背包UI")]
    [Space(16)]

    public Transform packagePanel;
    public bool isPackageOpen;
    [Space(16)]
    public BasePanel currentMenuPanel;
    public List<Transform> menuSelectionList;
    public List<BasePanel> menuPanelList;
    public string[] menuNameList = { "玩家属性", "道具", "武器", "饰品", "天赋树", "设置" };
    public TextMeshProUGUI menuName;
    public int currentMenuIndex;

    [Space(16)]
    [Header("武器")]
    [Space(16)]

    public Transform weaponSlots;
    public GameObject weaponSlotPrefab;
    public WeaponSlotUI currentSelectedWeapon;
    public WeaponSlotUI currentEquippedWeapon;
    [Space(16)]
    public Transform detailPanel;
    public TextMeshProUGUI selectedWeaponName;
    public TextMeshProUGUI selectedWeaponLevel;
    [Space(16)]
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI critRateValue;
    public TextMeshProUGUI critDamageValue;
    public TextMeshProUGUI damageRateValue;
    public TextMeshProUGUI reductionRateValue;

    private void Update()
    {
        if (!isPackageOpen && Input.GetKeyDown(KeyCode.Escape))
            OpenPackage();
    }

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

    #region 武器

    public void AddWeapon(int id)
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
        damageRateValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].damageRate * 100 + "%";
        reductionRateValue.text = PackageManager.Instance.allWeaponList[id].weaponStats[level - 1].reductionRate * 100 + "%";
        detailPanel.gameObject.SetActive(true);
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
}
