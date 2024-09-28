using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AccessorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int accessoryID;
    public Image accessoryIcon;
    public Image selectionTip;
    public TextMeshProUGUI equipPosition;

    private Sequence pointerEnterSequence;
    private Sequence pointerExitSequence;

    private void Awake()
    {
        pointerEnterSequence = DOTween.Sequence();
        pointerExitSequence = DOTween.Sequence();
        pointerEnterSequence.Pause();
        pointerExitSequence.Pause();
        pointerEnterSequence.SetAutoKill(false);
        pointerExitSequence.SetAutoKill(false);
        pointerEnterSequence.Append(selectionTip.DOFade(1, 0.5f));
        pointerExitSequence.Append(selectionTip.DOFade(0, 0.5f));
    }

    public void Initialize(int id)
    {
        accessoryID = id;
        accessoryIcon.sprite = PackageManager.Instance.allAccessoryList[id].accessoryIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.SelectAccessory(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerExitSequence.Pause();
        pointerEnterSequence.Restart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEnterSequence.Pause();
        pointerExitSequence.Restart();
    }
}
