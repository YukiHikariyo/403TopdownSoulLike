using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    public Transform mainCanvas;
    [Space(16)]
    [Header("背包UI")]
    public BasePanel currentMenuPanel;
    [Space(16)]
    public Transform packagePanel;
    public bool isPackageOpen;
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
    public TextMeshProUGUI selectedWeaponName;
    public TextMeshProUGUI selectedWeaponLevel;

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

        selectedWeaponName.text = PackageManager.Instance.allWeaponList[currentSelectedWeapon.weaponID].name;
        selectedWeaponLevel.text = "LV." + PackageManager.Instance.weaponDict[currentSelectedWeapon.weaponID].level;
    }

    public void EquipWeapon()
    {
        currentEquippedWeapon?.transform.GetChild(4).gameObject.SetActive(false);
        currentEquippedWeapon = currentSelectedWeapon;
        currentEquippedWeapon?.transform.GetChild(4).gameObject.SetActive(true);
    }

    #endregion
}
