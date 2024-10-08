using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int weaponID;
    public Image weaponIcon;
    public Image selectionTip;

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
        weaponID = id;
        weaponIcon.sprite = PackageManager.Instance.allWeaponList[id].weaponIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.SelectWeapon(this);
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
