using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SkillTreeDot : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public StaticSkillData thisSkillData;
    private Image thisSkillImage;
    private Image thisSkillShadow;
    public Image selectionTip;

    private Sequence pointerEnterSequence;
    private Sequence pointerExitSequence;
    public void OnPointerClick(PointerEventData eventData)
    {
        SkillManager.Instance.activeSkill = thisSkillData;
        SkillManager.Instance.SelectSkill(thisSkillData.skillTreeID);
        SkillManager.Instance.DisplayInfo();
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


    private void Awake()
    {
        thisSkillImage = gameObject.transform.GetChild(2).GetComponent<Image>();
        thisSkillShadow = gameObject.transform.GetChild(1).GetComponent<Image>();
        thisSkillImage.sprite = thisSkillData.skillSpite;
        thisSkillShadow.sprite = thisSkillData.skillSpite;
        thisSkillImage.type = Image.Type.Filled;
        thisSkillImage.fillMethod = Image.FillMethod.Vertical;
        thisSkillImage.fillAmount = 0;
        thisSkillShadow.color = new Color(0, 0, 0, 1);

        pointerEnterSequence = DOTween.Sequence();
        pointerExitSequence = DOTween.Sequence();
        pointerEnterSequence.Pause();
        pointerExitSequence.Pause();
        pointerEnterSequence.SetAutoKill(false);
        pointerExitSequence.SetAutoKill(false);
        pointerEnterSequence.Append(selectionTip.DOFade(1, 0.5f));
        pointerExitSequence.Append(selectionTip.DOFade(0, 0.5f));
    }

}
