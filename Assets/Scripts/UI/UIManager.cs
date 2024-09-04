using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Transform mainCanvas;
    [Space(16)]
    [Header("背包UI")]
    [Space(16)]
    public Transform packagePanel;
    public List<Transform> menuSelectionList;
    public List<Transform> menuPanelList;
    public TextMeshPro menuName;
    [Space(16)]
}
