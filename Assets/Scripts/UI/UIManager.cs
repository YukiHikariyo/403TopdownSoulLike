using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    public Transform mainCanvas;
    [Space(16)]
    [Header("背包UI")]
    public BasePanel currentPanel;
    [Space(16)]
    public Transform packagePanel;
    public bool isPackageOpen;
    public List<Transform> menuSelectionList;
    public List<BasePanel> menuPanelList;
    public string[] menuNameList = { "玩家属性", "道具", "武器", "饰品", "天赋树", "设置" };
    public TextMeshProUGUI menuName;
    public int currentMenuIndex;
    [Space(16)]
    public TextMeshProUGUI selectedWeaponName;
    public TextMeshProUGUI selectedWeaponLevel;

    private void Update()
    {
        if (!isPackageOpen && Input.GetKeyDown(KeyCode.Escape))
            OpenPackage();
    }

    public void OpenPackage()
    {
        currentPanel.OnOpen();
        packagePanel.gameObject.SetActive(true);
        isPackageOpen = true;
    }

    public void ClosePackage()
    {
        packagePanel.gameObject.SetActive(false);
        currentPanel.OnClose();
        isPackageOpen = false;
    }

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

        currentPanel.gameObject.SetActive(false);
        currentPanel.OnClose();
        currentPanel = menuPanelList[index];
        menuPanelList[index].OnOpen();
        menuPanelList[index].gameObject.SetActive(true);
    }
}
